using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class GroupDrawer : IGroupDrawer
    {
        protected SerializedObject serializedObject;
        public SerializedObject SerializedObject => serializedObject;

        protected Type attributeType;
        public Type AttributeType => attributeType;

        protected List<SerializedProperty> properties;
        public List<SerializedProperty> Properties => properties;

        public SerializedProperty StartProperty => properties[0];

        public GroupDrawer(SerializedObject serializedObject)
        {
            properties = new List<SerializedProperty> ();
        }

        public virtual void OnGUI()
        {

        }

        public void RegisterProperty(SerializedProperty property)
        {
            properties.Add (property);
        }
    }

    public class GroupDrawer<T> : GroupDrawer
    {
        public T attribute;

        public GroupDrawer(T attribute, SerializedObject serializedObject) : base(serializedObject)
        {
            this.attribute = attribute;
        }
    }
}
