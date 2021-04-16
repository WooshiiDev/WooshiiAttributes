using System;
using UnityEditor;

namespace WooshiiAttributes
{
    public abstract class AbstractCustomDrawer : ICustomPropertyDrawer
    {
        protected Type _attributeType;
        protected SerializedObject _serializedObject;
        protected SerializedProperty _serializedProperty;

        // Properties
        public Type AttributeType => _attributeType;

        public SerializedObject SerializedObject => _serializedObject;
        public SerializedProperty SerializedProperty => _serializedProperty;

        public AbstractCustomDrawer(SerializedObject _parent, SerializedProperty _property, Type _type)
        {
            _serializedObject = _parent;
            _serializedProperty = _property;
            _attributeType = _type;
        }

        public void OnGUI()
        {
            EditorGUI.BeginChangeCheck ();
            {
                OnGUI_Internal ();
            }
            if (EditorGUI.EndChangeCheck ())
            {
                SerializedObject.ApplyModifiedProperties ();
            }
        }

        protected virtual void OnGUI_Internal()
        {
        }
    }
}