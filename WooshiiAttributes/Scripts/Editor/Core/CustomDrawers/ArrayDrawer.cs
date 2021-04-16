using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class ArrayDrawer : AbstractCustomDrawer
    {
        public ArrayDrawer(SerializedObject _serializedObject, SerializedProperty _property, Type _attributeType) : base (_serializedObject, _property, _attributeType)
        {
        }

        /// <summary>
        /// Get the original rect for this drawer
        /// </summary>
        /// <returns>Returns the original rect for this drawe</returns>
        protected Rect GetRect()
        {
            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += 19f;
            return rect;
        }
    }

    public class ArrayDrawer<T> : ArrayDrawer where T : ArrayAttribute
    {
        public ArrayDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property, typeof (T))
        {
        }
    }
}