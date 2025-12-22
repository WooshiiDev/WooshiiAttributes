using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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
            return CreateDrawer(attribute, attribute, target) as GroupDrawer;
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

            if (!s_drawerLookup.TryGetValue(attr.GetType(), out Type drawerType))
            {
                drawerType = s_fallbackDrawerType;
            }

            return (GUIDrawerBase)Activator.CreateInstance(drawerType, args);
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
                GUIDrawerBase drawer = PropertyGUICache.CreateDrawer(property);

                if (drawer == null)
                {
                    continue;
                }

                RegisterDrawer(ReflectionUtility.GetField(target, property.name), drawer);
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

                RegisterDrawer(method, drawer);
            }
        }

        private void RegisterDrawer(MemberInfo member, GUIDrawerBase drawer)
        {
            if (HasGroup(member))
            {
                currentGroup.RegisterProperty(drawer);
            }
            else
            {
                guiDrawers.Add(drawer);
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
            }

            return drawer;
        }

        private bool HasGroup(MemberInfo member)
        {
            foreach (GroupAttribute attribute in member.GetCustomAttributes<GroupAttribute>(true))
            {
                if (attribute is EndGroupAttribute)
                {
                    currentGroup = null;
                    usingGroup = false;
                }
                else
                {
                    currentGroup = GetOrCreateGroup(attribute);
                    usingGroup = true;
                }
            }

            return usingGroup;
        }
    }
}