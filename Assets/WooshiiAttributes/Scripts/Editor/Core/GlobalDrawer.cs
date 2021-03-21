using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GlobalDrawer : AbstractCustomDrawer
    {
        public List<SerializedProperty> Properties { get; private set; }
        public List<GlobalAttribute> Attributes { get; private set; }

        public GlobalDrawer(SerializedObject serializedObject, SerializedProperty property, Type attributeType) : base (serializedObject, property, attributeType)
        {
            Properties = new List<SerializedProperty> ();
            Attributes = new List<GlobalAttribute> ();
        }

        public virtual void Register(GlobalAttribute attribute, SerializedProperty property)
        {
            Attributes.Add (attribute);
            Properties.Add (property);
        }
    }

    public class GlobalDrawer<T> : GlobalDrawer where T : GlobalAttribute
    {
        public new List<T> Attributes { get; private set; }

        public GlobalDrawer(SerializedObject serializedObject, SerializedProperty property) : base (serializedObject, property, typeof (T))
        {
            Attributes = new List<T> ();
        }

        public override void Register(GlobalAttribute attribute, SerializedProperty property)
        {
            Attributes.Add (attribute as T);
            Properties.Add (property);
        }
    }
}