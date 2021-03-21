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

        // BindingFlags
        private readonly BindingFlags MethodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private readonly BindingFlags FieldFlags = BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;


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
            }

            globalDrawers = new Dictionary<Type, GlobalDrawer> ();
            serializedData = new List<SerializedData> ();

            visibleMethods = new List<IMethodDrawer> ();
            visibleProperties = GetAllVisibleProperties ();

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
        }

        public override void OnInspectorGUI()
        {
            if (AllDrawers == null)
            {
                Initialize ();
            }

            cachedGroupDrawer = null;

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

            foreach (IMethodDrawer method in visibleMethods)
            {
                method.OnGUI ();
            }
        }

        // Reflection

        /// <summary>
        /// Find all custom drawers of type. Primarily used for finding Global and Array Drawers.
        /// </summary>
        private Dictionary<Type, ICustomPropertyDrawer> GetAllDrawerSubclasses<T>() where T : ICustomPropertyDrawer
        {
            Dictionary<Type, ICustomPropertyDrawer> drawers = new Dictionary<Type, ICustomPropertyDrawer> ();
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
            return instance.GetType ().GetFields (FieldFlags).Where (condition);
        }

        private void FindDrawerTypes(Type attributeType)
        {
            foreach (Type type in GetTypeSubclasses (attributeType))
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

            foreach (MethodInfo method in target.GetType ().GetMethods (MethodFlags))
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

        /// <summary>
        /// Find the global property assigned to this SerializedProperty
        /// </summary>
        /// <param name="_property">The target property</param>
        private void GetGlobalAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            // TODO: [Damian] Be able to find child data as this currently only works for root fields
            FieldInfo field = target.GetType ().GetField (_property.name, FieldFlags);
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

            if (TryGetData (_property, out SerializedData _data))
            {
                Type drawerType = AllDrawers[attributeType];
                ArrayDrawer drawer = Activator.CreateInstance (drawerType, serializedObject, _property) as ArrayDrawer;

                _data.arrayDrawer = drawer;
            }
        }

        private void GetGroupAttribute(SerializedProperty _property)
        {
            // Get the field based on the serialized property cached name
            FieldInfo field = target.GetType ().GetField (_property.name);

            if (cachedGroupDrawer != null)
            {
                if (field.GetCustomAttribute<EndGroupAttribute> () != null)
                {
                    cachedGroupDrawer.RegisterProperty (_property);
                    cachedGroupDrawer = null;
                }

                return;
            }

            GroupAttribute groupAttribute = field.GetCustomAttribute<GroupAttribute> ();

            if (groupAttribute == null)
            {
                return;
            }

            Type attributeType = groupAttribute.GetType ();

            if (TryGetData (_property, out SerializedData _data))
            {
                Type drawerType = AllDrawers[attributeType];
                GroupDrawer drawer = Activator.CreateInstance (drawerType, groupAttribute, serializedObject) as GroupDrawer;

                _data.hasGroup = true;
                _data.groupDrawer = drawer;
                cachedGroupDrawer = drawer;
            }
        }

        // Helpers
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