using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    internal static class PropertyGUICache
    {
        private static Dictionary<Type, Type> s_drawerLookup = new Dictionary<Type, Type>();

        private static readonly Type s_fallbackDrawerType = typeof(SerializedPropertyDrawer);

        public static bool HasInitialized { get; private set; }

        public static void Collect()
        {
            if (HasInitialized)
            {
                return;
            }

            foreach (Type t in ReflectionUtility.GetTypeSubclasses(typeof(GUIDrawerBase), true))
            {
                GUIDrawerAttribute attribute = t.GetCustomAttribute<GUIDrawerAttribute>(true);

                if (attribute == null)
                {
                    continue;
                } 

                s_drawerLookup.Add(attribute.Element, t);
            }

            HasInitialized = true;
        }

        public static GUIDrawerBase CreateDrawer(SerializedProperty property)
        {
            return new SerializedPropertyDrawer(property);
        }

        public static GUIDrawerBase CreateDrawer(object target, MethodInfo method)
        {
            MethodButtonAttribute attr = method.GetCustomAttribute<MethodButtonAttribute>();
            return CreateDrawer(attr, attr, target, method);
        }

        private static GUIDrawerBase CreateDrawer(GUIElementAttribute attr, params object[] args)
        {
            if (attr == null)
            {
                return null;
            }

            return CreateDrawerFromDrawerType(GetDrawerTypeFromAttribute(attr), args);
        }

        private static GUIDrawerBase CreateDrawerFromDrawerType(Type drawerType, params object[] args)
        {
            if (drawerType == null)
            {
                drawerType = s_fallbackDrawerType;
            }
            
            return (GUIDrawerBase)Activator.CreateInstance(drawerType, args);
        }

        private static Type GetDrawerTypeFromAttribute(GUIElementAttribute guiElement)
        {
            if (!s_drawerLookup.TryGetValue(guiElement. GetType(), out Type drawerType))
            {
                drawerType = s_fallbackDrawerType;
            }

            return drawerType;
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor (typeof (MonoBehaviour), true)]
    public class WooshiiEditor : Editor
    {
        private List<GUIDrawerBase> guiDrawer = new List<GUIDrawerBase>();

        public void OnEnable()
        {
            if (!PropertyGUICache.HasInitialized)
            {
                PropertyGUICache.Collect();
            }

            foreach (SerializedProperty property in SerializedUtility.GetAllVisibleProperties(serializedObject))
            {
                guiDrawer.Add(PropertyGUICache.CreateDrawer(property));
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < guiDrawer.Count; ++i)
            {
                guiDrawer[i].OnGUI();
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}