using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class ArrayDrawer : AbstractCustomDrawer
    {
        public ArrayDrawer(SerializedObject serializedObject, SerializedProperty property, Type attributeType) : base (serializedObject, property, attributeType)
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
        public ArrayDrawer(SerializedObject parent, SerializedProperty property) : base (parent, property, typeof (T))
        {

        }
    }
}
