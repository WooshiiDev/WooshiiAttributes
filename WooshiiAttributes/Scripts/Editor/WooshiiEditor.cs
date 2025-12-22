using Codice.Client.BaseCommands;
using Codice.Client.Common.GameUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WooshiiAttributes
{
    internal static class PropertyGUICache
    {
        private static readonly Dictionary<Type, Type> s_drawerLookup = new Dictionary<Type, Type>();
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

        public static GroupDrawer CreateGroup(GroupAttribute attribute, SerializedObject target)
        {
            return CreateDrawerFromDrawerType(GetDrawerTypeFromAttribute(attribute), attribute, target) as GroupDrawer;
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
            if (!s_drawerLookup.TryGetValue(guiElement.GetType(), out Type drawerType))
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
        private readonly List<GUIDrawerBase> guiDrawers = new List<GUIDrawerBase>();
        private readonly Dictionary<string, GroupDrawer> groupLookup = new Dictionary<string, GroupDrawer>();

        private bool usingGroup = false;
        private GroupDrawer currentGroup;

        public void OnEnable()
        {
            if (!PropertyGUICache.HasInitialized)
            {
                PropertyGUICache.Collect();
            }

            CreateSerializedDrawers();
            CreateMethodDrawers();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < guiDrawers.Count; ++i)
            {
                guiDrawers[i].OnGUI();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    
        private void CreateSerializedDrawers()
        {
            foreach (SerializedProperty property in SerializedUtility.GetAllVisibleProperties(serializedObject))
            {
                FieldInfo field = GetPropertyField(property);
                bool hasGroup = HasGroup(field);
               
                GUIDrawerBase drawer = PropertyGUICache.CreateDrawer(property);
                GroupDrawer group = null;
                if (hasGroup || usingGroup)
                {
                    group = GetOrCreateGroup(field.GetCustomAttribute<GroupAttribute>());
                }

                if (group != null)
                {
                    group.RegisterProperty(drawer);
                }
                else
                {
                    guiDrawers.Add(drawer);
                }
            }
        }
        
        private void CreateMethodDrawers()
        {
            foreach (MethodInfo method in ReflectionUtility.GetMethods(target.GetType()))
            {
                GUIDrawerBase drawer = PropertyGUICache.CreateDrawer(target, method);

                if (drawer == null)
                {
                    continue;
                }

                bool hasGroup = HasGroup(method);
                GroupDrawer group = null;
                if (hasGroup || usingGroup)
                {
                    group = GetOrCreateGroup(method.GetCustomAttribute<GroupAttribute>());
                }

                if (group != null)
                {
                    group.RegisterProperty(drawer);
                }
                else
                {
                    guiDrawers.Add(drawer);
                }
            }
        }
            }
        }

        private GroupDrawer GetOrCreateGroup(GroupAttribute group)
        {
            if (group == null)
            {
                return currentGroup;
            }
            
            if (!groupLookup.TryGetValue(group.GroupName, out GroupDrawer drawer))
            {
                drawer = PropertyGUICache.CreateGroup(group, serializedObject);
                guiDrawers.Add(drawer);
                groupLookup.Add(group.GroupName, drawer);
                currentGroup = drawer;
                usingGroup = true;
            }

            return drawer;
        }

        private bool HasGroup(SerializedProperty property)
        {
            return HasGroup(GetPropertyField(property));
        }

        private bool HasGroup(MemberInfo member)
        {
            GroupAttribute attribute = member.GetCustomAttribute<GroupAttribute>(true);

            if (attribute is EndGroupAttribute)
            {
                usingGroup = false;
                currentGroup = null;
                return false;
            }

            if (attribute == null)
            {
                return usingGroup;
            }

            return true;
        }

        private FieldInfo GetPropertyField(SerializedProperty property)
        {
            return ReflectionUtility.GetField(target, property.name);
        }
    }
}