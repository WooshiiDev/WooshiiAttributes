using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class GroupDrawer : GUIDrawerBase
    {
        protected SerializedObject m_serializedObject;
        public SerializedObject SerializedObject => m_serializedObject;

        protected Type m_attributeType;
        public Type AttributeType => m_attributeType;

        protected List<SerializedProperty> m_properties;
        public List<SerializedProperty> Properties => m_properties;

        public SerializedProperty StartProperty => m_properties[0];

        public GroupDrawer(SerializedObject _serializedObject)
        {
            m_properties = new List<SerializedProperty> ();
        }

        public override void OnGUI() { }

        public void RegisterProperty(SerializedProperty _property)
        {
            m_properties.Add (_property);
        }
    }

    public class GroupDrawer<T> : GroupDrawer
    {
        public T attribute;

        public GroupDrawer(T attribute, SerializedObject _serializedObject) : base(_serializedObject)
        {
            this.attribute = attribute;
        }
    }
}
