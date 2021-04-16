using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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

        private Dictionary<Type, GlobalDrawer> m_globalDrawers;
        private List<PropertyAttributeDrawer> m_classPropertyDrawers;
      
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
            if (AllDrawers == null)
            {
                AllDrawers = new Dictionary<Type, Type> ();

                //TODO: [Damian] Merge type finding and change all to interfaces
                FindDrawerTypes (typeof (GlobalDrawer));
                FindDrawerTypes (typeof (ArrayDrawer));
                FindDrawerTypes (typeof (IMethodDrawer));
                FindDrawerTypes (typeof (GroupDrawer));
                FindDrawerTypes (typeof (PropertyAttributeDrawer));
            }

            m_globalDrawers = new Dictionary<Type, GlobalDrawer> ();
            m_serializedData = new List<SerializedData> ();

            m_visibleMethods = new List<IMethodDrawer> ();
            m_visibleProperties = SerializedUtility.GetAllVisibleProperties(serializedObject);

            GetMethodDrawers ();

            // We need all the properties and their corresponding fields
            int actualIndex = 0;
            for (int i = 0; i < m_visibleProperties.Count; ++i)
            {
                SerializedProperty property = m_visibleProperties[i];

                GetGlobalAttribute (property);
                GetArrayAttribute (property);
                GetGroupAttribute (property);

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

            m_classPropertyDrawers = new List<PropertyAttributeDrawer> ();

            IEnumerable<PropertyInfo> properties = ReflectionUtility.GetProperties(target);

            foreach (var property in properties)
            {
                GetPropertyAttribute (property);
            }
        }

        public override void OnInspectorGUI()
        {
            if (m_serializedData == null)
            {
                Initialize ();
            }

            m_cachedGroupDrawer = null;

            for (int i = 0; i < m_classPropertyDrawers.Count; i++)
            {
                m_classPropertyDrawers[i].OnGUI ();
            }

            EditorGUI.BeginChangeCheck ();

            for (int i = 0; i < m_serializedData.Count; i++)
            {
                SerializedData data = m_serializedData[i];
                DrawProperty (data);
            }

            foreach (GlobalDrawer drawer in m_globalDrawers.Values)
            {
                drawer.OnGUI ();
            }

            if (EditorGUI.EndChangeCheck ())
            {
                serializedObject.ApplyModifiedProperties ();
            }

            for (int i = 0; i < m_visibleMethods.Count; i++)
            {
                m_visibleMethods[i].OnGUI ();
            }

        }

        // Reflection

        private void FindDrawerTypes(Type _attributeType)
        {
            foreach (Type type in ReflectionUtility.GetTypeSubclasses (_attributeType))
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
            foreach (MethodInfo method in ReflectionUtility.GetMethods(target))
            {
                MethodButtonAttribute attribute = method.GetCustomAttribute<MethodButtonAttribute> ();

                if (attribute == null)
                {
                    continue;
                }

                MethodDrawer drawer = new MethodDrawer (attribute, target, method);

                m_visibleMethods.Add (drawer);
            }
        }

        // Serialized Data

        /// <summary>
        /// Find the global property assigned to this SerializedProperty
        /// </summary>
        /// <param name="_property">The target property</param>
        private void GetGlobalAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            // TODO: [Damian] Be able to find child data as this currently only works for root fields
            FieldInfo field = ReflectionUtility.GetField (target, _property.name);

            if (field == null)
            {
                return;
            }

            GlobalAttribute globalAttribute = field.GetCustomAttribute<GlobalAttribute> ();

            m_serializedData.Add (new SerializedData (field, _property, globalAttribute != null));

            if (globalAttribute == null)
            {
                return;
            }

            Type attributeType = globalAttribute.GetType ();

            if (!m_globalDrawers.TryGetValue (attributeType, out GlobalDrawer drawer))
            {
                drawer = CreateInstanceOfType<GlobalDrawer> (AllDrawers[attributeType], serializedObject, _property);
                m_globalDrawers.Add (attributeType, drawer);
            }

            drawer.Register (globalAttribute, _property);
        }

        private void GetArrayAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            FieldInfo field = target.GetType ().GetField (_property.name);

            if (field == null)
            {
                return;
            }

            ArrayAttribute arrayAttribute = field.GetCustomAttribute<ArrayAttribute> ();

            if (arrayAttribute == null)
            {
                return;
            }

            Type attributeType = arrayAttribute.GetType ();

            if (TryGetData (_property, out SerializedData _data))
            {
                ArrayDrawer drawer = CreateInstanceOfType<ArrayDrawer> (AllDrawers[attributeType], serializedObject, _property);

                _data.m_arrayDrawer = drawer;
            }
        }

        private void GetPropertyAttribute(PropertyInfo _property)
        {
            // Get the field based on the serialized property cached name
            ClassPropertyAttribute attribute = _property.GetCustomAttribute<ClassPropertyAttribute> ();

            if (attribute == null)
            {
                return;
            }

            Type attributeType = attribute.GetType ();

            if (AllDrawers.TryGetValue(attributeType, out Type drawerType))
            {
                PropertyAttributeDrawer drawer =  CreateInstanceOfType<PropertyAttributeDrawer>(drawerType, _property, target);
                m_classPropertyDrawers.Add (drawer);
            }
        }

        private void GetGroupAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            FieldInfo field = ReflectionUtility.GetField (target, _property.name);

            if (field == null)
            {
                return;
            }

            BeginGroupAttribute beginAttribute = field.GetCustomAttribute<BeginGroupAttribute> ();
            EndGroupAttribute endAttribute = field.GetCustomAttribute<EndGroupAttribute> ();

            if (m_cachedGroupDrawer != null)
            {
                if (endAttribute != null)
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

            Type attributeType = beginAttribute.GetType ();

            if (TryGetData (_property, out SerializedData _data))
            {
                GroupDrawer drawer = CreateInstanceOfType<GroupDrawer>(AllDrawers[attributeType], beginAttribute, serializedObject);

                _data.hasGroup = true;
                _data.m_groupDrawer = drawer;
                m_cachedGroupDrawer = drawer;
            }
        }

        // Helpers
        private void CreateGlobalDrawer<T>(T _attribute, SerializedProperty _property, Type _drawerType, params object[] _drawerArgs) where T : GlobalAttribute
        {
            if (_attribute == null)
            {
                return;
            }

            Type attributeType = _attribute.GetType ();

            if (m_globalDrawers.TryGetValue (attributeType, out GlobalDrawer drawer))
            {
                drawer = CreateInstanceOfType<GlobalDrawer> (drawer.GetType(), _property, target);
                m_globalDrawers.Add (attributeType, drawer);
            }

            drawer.Register (_attribute, _property);
        }

        private T CreateInstanceOfType<T>(Type _type, params object[] _args)
        {
            T instance = (T)Activator.CreateInstance (_type, _args);

            return instance;
        }

        private bool TryGetData(SerializedProperty _property, out SerializedData _data)
        {
            _data = m_serializedData.FirstOrDefault (t => t.m_property == _property);

            return _data != null;
        }

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
    }
}