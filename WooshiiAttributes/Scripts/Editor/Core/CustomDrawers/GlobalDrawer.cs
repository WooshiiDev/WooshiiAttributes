using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GlobalDrawer : AbstractCustomDrawer
    {
        public List<SerializedProperty> Properties { get; private set; }
        public List<GlobalAttribute> Attributes { get; private set; }

        public GlobalDrawer(SerializedObject _serializedObject, SerializedProperty _property, Type _attributeType) : base (_serializedObject, _property, _attributeType)
        {
            Properties = new List<SerializedProperty> ();
            Attributes = new List<GlobalAttribute> ();
        }

        public virtual void Register(GlobalAttribute _attribute, SerializedProperty _property)
        {
            Attributes.Add (_attribute);
            Properties.Add (_property);
        }
    }

    public class GlobalDrawer<T> : GlobalDrawer where T : GlobalAttribute
    {
        public GlobalDrawer(SerializedObject _serializedObject, SerializedProperty _property) : base (_serializedObject, _property, typeof (T))
        {

        }
    }
}