using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class WooshiiPropertyDrawer : PropertyDrawer
    {
        //Cached

        protected float _lineHeight = EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return base.GetPropertyHeight (_property, _label);
        }

        //Custom Methods

        protected void DrawBackground(Rect _rect, Color _color)
        {
            GUI.color = _color;
            GUI.Box (_rect, GUIContent.none, EditorStyles.textField);
            GUI.color = Color.white;
        }
    }
}