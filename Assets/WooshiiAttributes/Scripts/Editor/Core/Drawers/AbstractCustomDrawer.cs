using System;
using UnityEditor;

namespace WooshiiAttributes
{
    public abstract class AbstractCustomDrawer : ICustomPropertyDrawer
    {
        protected Type attributeType;
        public SerializedObject serializedObject;
        public SerializedProperty serializedProperty;

        // Properties
        public Type AttributeType => attributeType;

        public SerializedObject SerializedObject => serializedObject;
        public SerializedProperty SerializedProperty => serializedProperty;

        public AbstractCustomDrawer(SerializedObject parent, SerializedProperty property, Type type)
        {
            serializedObject = parent;
            serializedProperty = property;
            attributeType = type;
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