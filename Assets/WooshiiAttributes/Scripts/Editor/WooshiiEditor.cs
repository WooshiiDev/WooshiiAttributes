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
            public readonly FieldInfo field;
            public SerializedProperty property;

            public bool isGlobal = false;
            public bool hasGroup = false;

            public ArrayDrawer arrayDrawer;
            public GroupDrawer groupDrawer;

            public SerializedData(FieldInfo field, SerializedProperty property, bool hasGlobal)
            {
                this.field = field;
                this.property = property;
                this.isGlobal = hasGlobal;
            }
        }

        // Static Data

        /// <summary>
        /// Dictionary with a key value pair linking custom attributes to their corresponding drawer type
        /// </summary>
        private static Dictionary<Type, Type> AllDrawers;

        // Local Data
        private List<SerializedProperty> visibleProperties;
        private List<IMethodDrawer> visibleMethods;

        private List<SerializedData> serializedData;
        private GroupDrawer cachedGroupDrawer;

        private Dictionary<Type, GlobalDrawer> globalDrawers;
        private List<PropertyAttributeDrawer> classPropertyDrawers;
      
        // Potential conflict with same named members/types of actual variables
        private string[] excludedPropertyTypes =
            {
            "PPtr<MonoScript>",
            "ArraySize",
            };

        private string[] excludedPropertyNames =
            {
            "m_Script",
            };

        private void Awake()
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

            globalDrawers = new Dictionary<Type, GlobalDrawer> ();
            serializedData = new List<SerializedData> ();

            visibleMethods = new List<IMethodDrawer> ();
            visibleProperties = SerializedUtility.GetAllVisibleProperties(serializedObject);

            GetMethodDrawers ();

            // We need all the properties and their corresponding fields
            int actualIndex = 0;
            for (int i = 0; i < visibleProperties.Count; ++i)
            {
                SerializedProperty property = visibleProperties[i];

                GetGlobalAttribute (property);
                GetArrayAttribute (property);
                GetGroupAttribute (property);

                if (cachedGroupDrawer != null && !serializedData[actualIndex].isGlobal)
                {
                    cachedGroupDrawer.RegisterProperty (property);
                    visibleProperties.RemoveAt (i);

                    i--;
                }

                actualIndex++;
            }

            if (cachedGroupDrawer != null)
            {
                cachedGroupDrawer = null;
            }

            classPropertyDrawers = new List<PropertyAttributeDrawer> ();

            IEnumerable<PropertyInfo> properties = ReflectionUtility.GetProperties(target);

            foreach (var property in properties)
            {
                GetPropertyAttribute (property);
            }
        }

        public override void OnInspectorGUI()
        {
            if (serializedData == null)
            {
                Initialize ();
            }

            cachedGroupDrawer = null;

            for (int i = 0; i < classPropertyDrawers.Count; i++)
            {
                classPropertyDrawers[i].OnGUI ();
            }

            EditorGUI.BeginChangeCheck ();

            for (int i = 0; i < serializedData.Count; i++)
            {
                SerializedData data = serializedData[i];
                DrawProperty (data);
            }

            foreach (GlobalDrawer drawer in globalDrawers.Values)
            {
                drawer.OnGUI ();
            }

            if (EditorGUI.EndChangeCheck ())
            {
                serializedObject.ApplyModifiedProperties ();
            }

            for (int i = 0; i < visibleMethods.Count; i++)
            {
                visibleMethods[i].OnGUI ();
            }

        }

        // Reflection

        private void FindDrawerTypes(Type attributeType)
        {
            foreach (Type type in ReflectionUtility.GetTypeSubclasses (attributeType))
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

                visibleMethods.Add (drawer);
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

            serializedData.Add (new SerializedData (field, _property, globalAttribute != null));

            if (globalAttribute == null)
            {
                return;
            }

            Type attributeType = globalAttribute.GetType ();

            if (!globalDrawers.TryGetValue (attributeType, out GlobalDrawer drawer))
            {
                drawer = CreateInstanceOfType<GlobalDrawer> (AllDrawers[attributeType], serializedObject, _property);
                globalDrawers.Add (attributeType, drawer);
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

                _data.arrayDrawer = drawer;
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
                classPropertyDrawers.Add (drawer);
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

            if (cachedGroupDrawer != null)
            {
                if (endAttribute != null)
                {
                    cachedGroupDrawer.RegisterProperty (_property);
                    cachedGroupDrawer = null;
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
                _data.groupDrawer = drawer;
                cachedGroupDrawer = drawer;
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

            if (globalDrawers.TryGetValue (attributeType, out GlobalDrawer drawer))
            {
                drawer = CreateInstanceOfType<GlobalDrawer> (drawer.GetType(), _property, target);
                globalDrawers.Add (attributeType, drawer);
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
            _data = serializedData.FirstOrDefault (t => t.property == _property);

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
                cachedGroupDrawer = _data.groupDrawer;
                _data.groupDrawer.OnGUI ();
                return;
            }

            // Do not draw grouped variables
            if (cachedGroupDrawer != null && cachedGroupDrawer.Properties.Contains (_data.property))
            {
                return;
            }

            if (_data.arrayDrawer != null)
            {
                _data.arrayDrawer.OnGUI ();
            }
            else
            {

                EditorGUILayout.PropertyField (_data.property, true);
            }
        }
    }
}