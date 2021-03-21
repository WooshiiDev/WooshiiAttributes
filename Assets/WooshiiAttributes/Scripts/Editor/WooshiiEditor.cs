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
            public bool hasGlobal = false;
            public ArrayDrawer drawer;

            public SerializedData(FieldInfo field, SerializedProperty property, bool hasGlobal)
            {
                this.field = field;
                this.property = property;
                this.hasGlobal = hasGlobal;
            }
        }

        // Static Data


        /// <summary>
        /// Dictionary with a key value pair linking custom attributes to their corresponding drawer type
        /// </summary>
        private static Dictionary<Type, Type> AllDrawers;
        // Local Data
        private List<SerializedProperty> visibleProperties;
        private List<SerializedData> serializedData;
        private Dictionary<Type, GlobalDrawer> globalDrawers;

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

        private void OnEnable()
        {
            // Cache all drawers and drawers through key/values
            if (AllDrawers == null)
            {
                AllDrawers = new Dictionary<Type, Type> ();

                //TODO: [Damian] Merge subclass checks into one
                foreach (Type type in GetTypeSubclasses (typeof (GlobalDrawer)))
                {
                    Type baseType = type.BaseType;

                    if (baseType.IsAbstract || type.IsGenericType)
                    {
                        continue;
                    }

                    AllDrawers.Add (baseType.GetGenericArguments ()[0], type);
                }

                foreach (Type type in GetTypeSubclasses (typeof (ArrayDrawer)))
                {
                    Type baseType = type.BaseType;

                    if (baseType.IsAbstract || type.IsGenericType)
                    {
                        continue;
                    }

                    AllDrawers.Add (baseType.GetGenericArguments ()[0], type);
                }
            }

            globalDrawers = new Dictionary<Type, GlobalDrawer> ();
            serializedData = new List<SerializedData> ();

            visibleProperties = GetAllVisibleProperties ();

            // We need all the properties and their corresponding fields
            foreach (SerializedProperty property in visibleProperties)
            {
                GetSerializedData (property);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck ();

            foreach (SerializedData data in serializedData)
            {
                if (data.hasGlobal)
                {
                    continue;
                }

                if (data.drawer != null)
                {
                    data.drawer.OnGUI ();
                }
                else
                {
                    EditorGUILayout.PropertyField (data.property, true);
                }
            }

            foreach (GlobalDrawer drawer in globalDrawers.Values)
            {
                drawer.OnGUI ();
            }

            if (EditorGUI.EndChangeCheck ())
            {
                serializedObject.ApplyModifiedProperties ();
            }
        }

        // Reflection

        /// <summary>
        /// Find all custom drawers of type. Primarily used for finding Global and Array Drawers.
        /// </summary>
        private Dictionary<Type, ICustomDrawer> GetAllDrawerSubclasses<T>() where T : ICustomDrawer
        {
            Dictionary<Type, ICustomDrawer> drawers = new Dictionary<Type, ICustomDrawer> ();
            IEnumerable<Type> types = GetTypeSubclasses (typeof (T));

            foreach (KeyValuePair<Type, Type> drawerType in AllDrawers)
            {
                //var drawer = Activator.CreateInstance (drawerType) as ICustomDrawer;
                //drawers.Add (drawerType, drawer);
            }

            return drawers;
        }

        /// <summary>
        /// Get all subclasses of a type. Will ignore abstract classes.
        /// </summary>
        /// <param name="type">The type to search for</param>
        private IEnumerable<Type> GetTypeSubclasses(Type type)
        {
            return GetType ().Assembly.GetTypes ().Where (t => t.IsSubclassOf (type) && !t.IsAbstract);
        }

        private IEnumerable<FieldInfo> GetFields(Func<FieldInfo, bool> condition)
        {
            return GetFields (target, condition);
        }

        private IEnumerable<FieldInfo> GetFields(Object instance, Func<FieldInfo, bool> condition)
        {
            return instance.GetType ().GetFields (BindingFlags.Public | BindingFlags.Default | BindingFlags.Instance).Where (condition);
        }

        // Serialized Data

        private List<SerializedProperty> GetAllVisibleProperties()
        {
            List<SerializedProperty> properties = new List<SerializedProperty> ();

            using (SerializedProperty iterator = serializedObject.GetIterator ())
            {
                if (iterator.NextVisible (true))
                {
                    do
                    {
                        string name = iterator.name;
                        string type = iterator.type;

                        if (excludedPropertyNames.Contains (name))
                        {
                            continue;
                        }

                        if (excludedPropertyTypes.Contains (type))
                        {
                            continue;
                        }

                        properties.Add (serializedObject.FindProperty (iterator.name));
                    }
                    while (iterator.NextVisible (false));
                }
            }

            return properties;
        }

        private void GetSerializedData(SerializedProperty _property)
        {
            GetGlobalAttribute (_property);
            GetArrayAttribute (_property);
        }

        /// <summary>
        /// Find the global property assigned to this SerializedProperty
        /// </summary>
        /// <param name="_property">The target property</param>
        private void GetGlobalAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            // TODO: [Damian] Be able to find child data as this currently only works for root fields
            FieldInfo field = target.GetType ().GetField (_property.name);
            GlobalAttribute globalAttribute = field.GetCustomAttribute<GlobalAttribute> ();

            serializedData.Add (new SerializedData (field, _property, globalAttribute != null));

            if (globalAttribute == null)
            {
                return;
            }

            Type attributeType = globalAttribute.GetType ();

            if (!globalDrawers.TryGetValue (attributeType, out GlobalDrawer drawer))
            {
                Type drawerType = AllDrawers[attributeType];
                drawer = Activator.CreateInstance (drawerType, serializedObject, _property) as GlobalDrawer;

                globalDrawers.Add (attributeType, drawer);
            }

            //Debug.Log (string.Format ("Registering {0} that belongs to field {1}", globalAttribute, property.name));
            drawer.Register (globalAttribute, _property);
        }

        private void GetArrayAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            FieldInfo field = target.GetType ().GetField (_property.name);
            ArrayAttribute arrayAttribute = field.GetCustomAttribute<ArrayAttribute> ();

            if (arrayAttribute == null)
            {
                return;
            }

            Type attributeType = arrayAttribute.GetType ();

            if (TryGetData(_property, out SerializedData _data))
            {
                Type drawerType = AllDrawers[attributeType];
                ArrayDrawer drawer = Activator.CreateInstance (drawerType, serializedObject, _property) as ArrayDrawer;

                _data.drawer = drawer;
            }
        }

        // Helpers
        private bool TryGetData(SerializedProperty _property, out SerializedData _data)
        {
            _data = serializedData.FirstOrDefault (t => t.property == _property);

            return _data != null;
        }
    }
}