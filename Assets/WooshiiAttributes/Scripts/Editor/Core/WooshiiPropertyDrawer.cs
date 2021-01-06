using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    { 
    public class WooshiiPropertyDrawer : PropertyDrawer
        {
        //Cached
        protected float lineHeight = EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
            return base.GetPropertyHeight (property, label);
            }

        //Custom Methods
        protected void DrawBackground(Rect rect, Color color)
            {
            GUI.color = color;
            GUI.Box (rect, GUIContent.none, EditorStyles.textField);
            GUI.color = Color.white;
            }
        }
    }
