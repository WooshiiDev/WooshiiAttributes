using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
    {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (MonoBehaviour), true)]
    public class WooshiiEditor : Editor
        {
        private class AttributeField
            {
            public readonly FieldInfo field;
            public List<ArrayDrawer> arrayDrawers = new List<ArrayDrawer>();
            public bool hasGlobal = false;

            public AttributeField(FieldInfo field)
                {
                this.field = field;
                }
            }

        //Properties/Fields
        private List<SerializedProperty> visibleProperties = new List<SerializedProperty> ();
        private HashSet<AttributeField> fields = new HashSet<AttributeField>();

        //Cached
        private static Dictionary<Type, ArrayDrawer> arrayDrawers;
        private static Dictionary<Type, GlobalDrawer> globalDrawers;
        private Dictionary<Type, GlobalDrawer> localGlobalDrawers = new Dictionary<Type, GlobalDrawer>();

        private int Count => fields.Count ();

        private void OnEnable()
            {
            GetVisibleProperties (ref visibleProperties);

            //Create a cache of all drawers
            if (arrayDrawers == null)
                {
                arrayDrawers = new Dictionary<Type, ArrayDrawer> ();

                var types = FindSubclassesOfType (typeof (ArrayDrawer));
                foreach (var type in types)
                    {
                    ArrayDrawer drawer = Activator.CreateInstance (type) as ArrayDrawer;
                    arrayDrawers.Add (drawer.attributeType, drawer);
                    }
                }

            if (globalDrawers == null)
                {
                globalDrawers = new Dictionary<Type, GlobalDrawer> ();

                var types = FindSubclassesOfType (typeof (GlobalDrawer));
                foreach (var type in types)
                    {
                    GlobalDrawer drawer = Activator.CreateInstance (type) as GlobalDrawer;
                    globalDrawers.Add (drawer.AttributeType, drawer);
                    }
                }

            //Get all fields with attributes and iterate over
            var arrayFields = GetFields (t => t.GetCustomAttributes (typeof (ArrayAttribute), true).Length > 0);
            foreach (var field in arrayFields)
                {
                var arrayField = new AttributeField (field);
                var attributes = field.GetCustomAttributes<ArrayAttribute> ();

                foreach (var attribute in attributes)
                    {
                    ArrayDrawer newDrawer = arrayDrawers[attribute.GetType ()];
                    newDrawer.attribute = attribute;

                    arrayField.arrayDrawers.Add (newDrawer.Clone ());
                    }


                fields.Add (arrayField);
                }

            var globalFields = GetFields (t => t.GetCustomAttributes (typeof (GlobalAttribute), true).Length > 0);
            foreach (var field in globalFields)
                {
                var attributes = field.GetCustomAttributes<GlobalAttribute> ();
                GlobalDrawer drawer = null;

                foreach (var attribute in attributes)
                    {
                    Type type = attribute.GetType ();

                    if (!globalDrawers.ContainsKey (type))
                        continue;

                    if (!localGlobalDrawers.ContainsKey (type))
                        {
                        drawer = globalDrawers[type].Clone ();

                        localGlobalDrawers.Add (type, drawer);
                        }
                    else
                        {
                        drawer = localGlobalDrawers[type];
                        }

                    drawer.Register (attribute);
                    }

                var attrField = fields.FirstOrDefault (t => t.field == field);
                if (attrField != null)
                    attrField.hasGlobal = true;
                else
                    {
                    var newField = new AttributeField (field)
                        {
                        hasGlobal = true
                        };

                    fields.Add (newField);
                    }

                if (drawer != null)
                    drawer.Register (visibleProperties.First (t => t.name == field.Name));
                }

            foreach (var globalDrawer in localGlobalDrawers)
                globalDrawer.Value.Initalize (serializedObject);
            }

        private void OnDisable()
            {
            //Clear serialized object ref
            serializedObject.Dispose ();
            visibleProperties.Clear ();

            foreach (var globalDrawer in localGlobalDrawers)
                globalDrawer.Value.Clear ();

            localGlobalDrawers.Clear ();
            fields.Clear ();
            }

        public override void OnInspectorGUI()
            {
            //Iterate over all properties, and find any form of global drawer
            if (Count == 0)
                {
                DrawDefaultInspector ();
                return;
                }

            //serializedObject.Update ();

            for (int i = 0; i < visibleProperties.Count; i++)
                {
                var property = visibleProperties[i];
                string name = property.name;

                AttributeField attributeField = fields.FirstOrDefault (a => a.field.Name == name);

                if (attributeField == null)
                    {
                    EditorGUI.BeginChangeCheck ();

                    EditorGUILayout.PropertyField (property);

                    if (EditorGUI.EndChangeCheck())
                        {
                        serializedObject.ApplyModifiedProperties ();
                        }

                    continue;
                    }

                if (attributeField.hasGlobal)
                    continue;

                for (int j = 0; j < attributeField.arrayDrawers.Count; j++)
                    attributeField.arrayDrawers[j].OnGUI (property);
                }

            foreach (var global in localGlobalDrawers)
                global.Value.OnGUI ();

            //Draw globals last
            for (int i = 0; i < visibleProperties.Count; i++)
                {
                var property = visibleProperties[i];
                string name = property.name;

                AttributeField attributeField = fields.FirstOrDefault (e => e.field.Name == name);

                if (attributeField == null)
                    continue;

                if (!attributeField.hasGlobal)
                    continue;

                for (int j = 0; j < attributeField.arrayDrawers.Count; j++)
                    attributeField.arrayDrawers[j].OnGUI (property);
                }

            }

        private void GetVisibleProperties(ref List<SerializedProperty> property)
            {
            visibleProperties.Clear ();

            using (var iterator = serializedObject.GetIterator ())
                {
                if (iterator.NextVisible (true))
                    {
                    do
                        {
                        property.Add (serializedObject.FindProperty (iterator.name));
                        }
                    while (iterator.NextVisible (false));
                    }
                }
            }

        private IEnumerable<FieldInfo> GetFields(Func<FieldInfo, bool> condition)
            {
            return target.GetType ().GetFields 
                (BindingFlags.Public | BindingFlags.Default | BindingFlags.Instance).Where (condition);
            }

        private IEnumerable<Type> FindSubclassesOfType(Type type)
            {
            return GetType ().Assembly.GetTypes ().Where (t => t.IsSubclassOf (type));
            }
        }
    }
