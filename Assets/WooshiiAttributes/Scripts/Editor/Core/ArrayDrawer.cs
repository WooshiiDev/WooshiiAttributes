using System;
using UnityEditor;

namespace WooshiiAttributes
    {
    public class ArrayDrawer 
        {
        public readonly Type attributeType;
        public readonly SerializedProperty property;

        public ArrayAttribute attribute;

        public ArrayDrawer(Type attributeType)
            {
            this.attributeType = attributeType;
            }

        public void OnGUI(SerializedProperty property)
            {
            EditorGUI.BeginChangeCheck ();
                {
                OnGUI_Internal (property);
                }
            if (EditorGUI.EndChangeCheck ())
                {
                property.serializedObject.ApplyModifiedProperties ();
                }
            }

        protected virtual void OnGUI_Internal(SerializedProperty property)
            {

            }

        public ArrayDrawer Clone()
            {
            return MemberwiseClone () as ArrayDrawer;
            }
        }
    }
