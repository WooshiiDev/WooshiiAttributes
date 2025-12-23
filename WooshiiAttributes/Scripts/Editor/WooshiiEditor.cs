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

        static PropertyGUICache()
        {
            Collect();
        }
        
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

        public static GroupDrawer CreateGroupDrawer(GroupAttribute attribute, SerializedObject target)
        {
            return CreateDrawer<GroupDrawer>(attribute, attribute, target);
        }

        public static GUIDrawerBase CreateDrawer(SerializedProperty property)
        {
            return new SerializedPropertyDrawer(property);
        }

        public static GUIDrawerBase CreateDrawer(MethodInfo method, MethodButtonAttribute attribute, object target)
        {
            return CreateDrawer<GUIDrawerBase>(attribute, attribute, target, method);
        }

        public static T CreateDrawer<T>(GUIElementAttribute attribute, params object[] args) where T : GUIDrawerBase
        {
            if (attribute == null)
            {
                return null;
            }

            if (!s_drawerLookup.TryGetValue(attribute.GetType(), out Type drawerType))
            {
                drawerType = s_fallbackDrawerType;
            }

            return (T)Activator.CreateInstance(drawerType, args);
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor (typeof (MonoBehaviour), true)]
    public class WooshiiEditor : Editor
    {
        private readonly List<GUIDrawerBase> _guiDrawers = new List<GUIDrawerBase>();
        private readonly Dictionary<string, GroupDrawer> _groupLookup = new Dictionary<string, GroupDrawer>();

        private bool _usingGroup = false;
        private GroupDrawer _currentGroup;

        public void OnEnable()
        {
            CreateSerializedDrawers();
            CreateMethodDrawers();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < _guiDrawers.Count; ++i)
            {
                _guiDrawers[i].OnGUI();
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
                FieldInfo field = ReflectionUtility.GetField(target, property.name);

                if (field == null)
                {
                    continue;
                }

                RegisterDrawer(field, PropertyGUICache.CreateDrawer(property));
            }
        }
        
        private void CreateMethodDrawers()
        {
            foreach (MethodInfo method in ReflectionUtility.GetMethods(target.GetType()))
            {
                MethodButtonAttribute button = method.GetCustomAttribute<MethodButtonAttribute>();

                if (button == null)
                {
                    continue;
                }

                RegisterDrawer(method, PropertyGUICache.CreateDrawer(method, button, target));
            }
        }

        private void RegisterDrawer(MemberInfo member, GUIDrawerBase drawer)
        {
            if (HasGroup(member))
            {
                _currentGroup.RegisterProperty(drawer);
            }
            else
            {
                _guiDrawers.Add(drawer);
            }
        }

        private GroupDrawer GetOrCreateGroup(GroupAttribute group)
        {
            if (group == null)
            {
                return _currentGroup;
            }
            
            if (!_groupLookup.TryGetValue(group.GroupName, out GroupDrawer drawer))
            {
                drawer = PropertyGUICache.CreateGroupDrawer(group, serializedObject);
                _guiDrawers.Add(drawer);
                _groupLookup.Add(group.GroupName, drawer);
            }

            return drawer;
        }

        private bool HasGroup(MemberInfo member)
        {
            foreach (GroupAttribute attribute in member.GetCustomAttributes<GroupAttribute>(true))
            {
                if (attribute is EndGroupAttribute)
                {
                    _currentGroup = null;
                    _usingGroup = false;
                }
                else
                {
                    _currentGroup = GetOrCreateGroup(attribute);
                    _usingGroup = true;
                }
            }

            return _usingGroup;
        }
    }
}