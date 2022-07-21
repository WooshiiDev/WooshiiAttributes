using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CanEditMultipleObjects]
    [CustomEditor (typeof (MonoBehaviour), true)]
    public class WooshiiEditor : Editor
    {
        private class SerializedData
        {
            public readonly FieldInfo m_field;
            public SerializedProperty m_property;

            public bool isGlobal = false;
            public bool hasGroup = false;

            public ArrayDrawer m_arrayDrawer;
            public GroupDrawer m_groupDrawer;

            public SerializedData(FieldInfo _field, SerializedProperty _property, bool _hasGlobal)
            {
                this.m_field = _field;
                this.m_property = _property;
                this.isGlobal = _hasGlobal;
            }
        }

        // Static Data

        /// <summary>
        /// Dictionary with a key value pair linking custom attributes to their corresponding drawer type
        /// </summary>
        private static Dictionary<Type, Type> AllDrawers;

        // Local Data

        private List<SerializedProperty> m_visibleProperties;
        private List<IMethodDrawer> m_visibleMethods;

        private List<SerializedData> m_serializedData;
        private GroupDrawer m_cachedGroupDrawer;

        private Dictionary<string, GlobalDrawer> m_globalDrawers;
        private List<PropertyInfo> m_properties;

        // Object Data

        private Type m_targetType;
        private FieldInfo[] fields;
        private MethodInfo[] methods;

        // Potential conflict with same named members/types of actual variables
        private readonly string[] m_excludedPropertyTypes =
        {
            "PPtr<MonoScript>",
            "ArraySize",
        };

        private readonly string[] m_excludedPropertyNames =
        {
            "m_Script",
        };

        private void OnEnable()
        {
            Initialize ();
        }

        private void Initialize()
        {
            m_targetType = target.GetType ();

            // Get Required field data

            fields = ReflectionUtility.GetFields (target).ToArray ();
            methods = ReflectionUtility.GetMethods (target).ToArray ();

            m_properties = ReflectionUtility.GetProperties (target, condition: HasValidAttribute<ClassPropertyAttribute>).ToList ();
            m_properties.Sort ((a, b) => a.CanWrite.CompareTo (b.CanWrite));

            if (AllDrawers == null)
            {
                AllDrawers = new Dictionary<Type, Type> ();

                //TODO: [Damian] Merge type finding and change all to interfaces
                FindDrawerTypes (typeof (GlobalDrawer));
                FindDrawerTypes (typeof (ArrayDrawer));
                FindDrawerTypes (typeof (IMethodDrawer));
                FindDrawerTypes (typeof (GroupDrawer));
            }

            m_globalDrawers = new Dictionary<string, GlobalDrawer> ();
            m_serializedData = new List<SerializedData> ();

            m_visibleMethods = new List<IMethodDrawer> ();
            m_visibleProperties = SerializedUtility.GetAllVisibleProperties (serializedObject);

            GetMethodDrawers ();

            // We need all the properties and their corresponding fields
            int actualIndex = 0;
            for (int i = 0; i < m_visibleProperties.Count; ++i)
            {
                SerializedProperty property = m_visibleProperties[i];

                bool hasGlobal = GetGlobalDrawer (property);
                m_serializedData.Add (new SerializedData (GetObjectField (property), property, hasGlobal));

                GetArrayDrawer (property);
                GetGroupDrawer (property);

                if (m_cachedGroupDrawer != null && !m_serializedData[actualIndex].isGlobal)
                {
                    m_cachedGroupDrawer.RegisterProperty (property);
                    m_visibleProperties.RemoveAt (i);

                    i--;
                }

                actualIndex++;
            }

            if (m_cachedGroupDrawer != null)
            {
                m_cachedGroupDrawer = null;
            }
        }

        public override void OnInspectorGUI()
        {
            if (m_serializedData == null)
            {
                Initialize ();
            }

            m_cachedGroupDrawer = null;

            if (m_properties.Count > 0)
            {
                GUIStyle style = EditorStyles.helpBox;
                EditorGUILayout.BeginVertical (style);

                EditorGUILayout.LabelField ("Read Only Properties", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup (true);
                bool canWrite = false;

                for (int i = 0; i < m_properties.Count; i++)
                {
                    if (!canWrite && m_properties[i].CanWrite)
                    {
                        EditorGUI.EndDisabledGroup ();

                        EditorGUILayout.EndVertical ();


                        canWrite = true;

                        EditorGUILayout.BeginVertical (style);
                        EditorGUILayout.LabelField ("Writable Properties", EditorStyles.boldLabel);
                        EditorGUI.BeginDisabledGroup (true);
                    }

                    NativePropertyDrawer.OnGUI (m_properties[i], target);
                }

                if (!canWrite)
                {
                    EditorGUILayout.EndVertical ();
                }

                EditorGUILayout.EndVertical ();
                EditorGUI.EndDisabledGroup ();
            }

            EditorGUI.BeginChangeCheck ();

            foreach (GlobalDrawer drawer in m_globalDrawers.Values)
            {
                drawer.OnGUI ();
            }

            for (int i = 0; i < m_serializedData.Count; i++)
            {
                DrawProperty (m_serializedData[i]);
            }

            if (EditorGUI.EndChangeCheck ())
            {
                serializedObject.ApplyModifiedProperties ();
            }

            if (m_visibleMethods.Count > 0)
            {
                EditorGUILayout.Space ();
                EditorGUILayout.LabelField ("Exposed Methods", EditorStyles.boldLabel);

                for (int i = 0; i < m_visibleMethods.Count; i++)
                {
                    m_visibleMethods[i].OnGUI ();
                }
            }

            if (RequiresConstantRepaint())
            {
                Repaint();
            }
        }

        // GUI 

        private void DrawProperty(SerializedData _data)
        {
            if (_data.isGlobal)
            {
                return;
            }

            if (_data.hasGroup)
            {
                m_cachedGroupDrawer = _data.m_groupDrawer;
                _data.m_groupDrawer.OnGUI ();
                return;
            }

            // Do not draw grouped variables
            if (m_cachedGroupDrawer != null && m_cachedGroupDrawer.Properties.Contains (_data.m_property))
            {
                return;
            }

            if (_data.m_arrayDrawer != null)
            {
                _data.m_arrayDrawer.OnGUI ();
            }
            else
            {

                EditorGUILayout.PropertyField (_data.m_property, true);
            }
        }

        // Drawers

        private void FindDrawerTypes(Type _attributeType)
        {
            foreach (Type type in ReflectionUtility.GetTypeSubclasses (_attributeType, true))
            {
                Type baseType = type.BaseType;

                if (baseType.IsAbstract || type.IsGenericType)
                {
                    continue;
                }

                AllDrawers.Add (baseType.GetGenericArguments ()[0], type);
            }
        }

        private void GetMethodDrawers()
        {
            foreach (MethodInfo method in methods)
            {
                MethodButtonAttribute attribute = GetAttribute<MethodButtonAttribute> (method);

                if (attribute == null)
                {
                    continue;
                }

                MethodButtonDrawer drawer = new MethodButtonDrawer (attribute, target, method);

                m_visibleMethods.Add (drawer);
            }
        }

        private bool GetGlobalDrawer(SerializedProperty _property)
        {
            GlobalAttribute attribute = GetAttribute<GlobalAttribute> (_property);

            if (attribute == null)
            {
                return false;
            }

            Type type = attribute.GetType ();

            string validator = attribute.GetIdenifier ();
            GlobalDrawer drawer;

            if (m_globalDrawers.ContainsKey (validator))
            {
                drawer = m_globalDrawers[validator];
            }
            else
            {
                drawer = CreateDrawer<GlobalDrawer> (attribute, serializedObject, _property);
                m_globalDrawers.Add (validator, drawer);
            }

            drawer.Register (attribute, _property);

            return true;
        }

        private void GetArrayDrawer(SerializedProperty _property)
        {
            ArrayAttribute attribute = GetAttribute<ArrayAttribute> (_property);

            if (attribute == null)
            {
                return;
            }

            Type attributeType = attribute.GetType ();

            if (TryGetData (_property, out SerializedData _data))
            {
                ArrayDrawer drawer = CreateDrawer<ArrayDrawer> (attribute, serializedObject, _property);
                _data.m_arrayDrawer = drawer;
            }
        }

        private void GetGroupDrawer(SerializedProperty _property)
        {
            BeginGroupAttribute beginAttribute = GetAttribute<BeginGroupAttribute> (_property);
            EndGroupAttribute endAttribute = GetAttribute<EndGroupAttribute> (_property);

            if (endAttribute != null)
            {
                if (m_cachedGroupDrawer != null)
                {
                    m_cachedGroupDrawer.RegisterProperty (_property);
                    m_cachedGroupDrawer = null;
                }

                return;
            }

            if (beginAttribute == null)
            {
                return;
            }

            if (TryGetData (_property, out SerializedData data))
            {
                GroupDrawer drawer = CreateDrawer<GroupDrawer> (beginAttribute, beginAttribute, serializedObject);

                data.hasGroup = true;
                data.m_groupDrawer = drawer;

                m_cachedGroupDrawer = drawer;
            }
        }

        private T CreateDrawer<T>(Attribute _attribute, params object[] _args)
        {
            return (T)Activator.CreateInstance (AllDrawers[_attribute.GetType ()], _args);
        }

        // Helpers

        private bool TryGetData(SerializedProperty _property, out SerializedData _data)
        {
            _data = m_serializedData.FirstOrDefault (t => t.m_property == _property);

            return _data != null;
        }

        // --- Reflection

        private bool HasValidAttribute<T>(MemberInfo _member) where T : Attribute
        {
            return _member.GetCustomAttribute<T> () != null;
        }

        private T GetAttribute<T>(SerializedProperty _property) where T : Attribute
        {
            FieldInfo field = m_targetType.GetField (_property.name);

            return GetAttribute<T> (field);
        }

        private T GetAttribute<T>(MemberInfo _member) where T : Attribute
        {
            if (_member == null)
            {
                throw new ArgumentNullException ();
            }

            return _member.GetCustomAttribute<T> ();
        }

        private FieldInfo GetObjectField(string _name)
        {
            return fields.FirstOrDefault (field => field.Name == name);
        }

        private FieldInfo GetObjectField(SerializedProperty _property)
        {
            return GetObjectField (_property.name);
        }
    }
}


// Serialized Data

// >>>> DRAWER TYPES <<<<

// Globals - Can happen anywhere, on anything
//  >> Normally move, group, reorder
//  >> Do they need instances? Should re-evaluate
//  >> Single per instance

// Array - Handles Serialized Collections
//  >> Has to be on an array/collection or break from creation
//  >> Do they need instances? Should re-evaluate
//  >> Single per instance

// Group - Groups data together in some way
//  >> Needs an attribute to end the group or will end at the end of the field list
//  >> Do they need instances? Should re-evaluate
//  >> Can multiple be on one instance?
