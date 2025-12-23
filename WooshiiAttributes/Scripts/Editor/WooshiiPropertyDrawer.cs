using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Base class for cusatom property drawers.
    /// </summary>
    public class WooshiiPropertyDrawer : PropertyDrawer
    {
        // - Fields

        protected float _lineHeight = EditorGUIUtility.singleLineHeight;

        // - Methods

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return base.GetPropertyHeight (_property, _label);
        }

        protected void DrawBackground(Rect _rect, Color _color)
        {
            GUI.color = _color;
            GUI.Box (_rect, GUIContent.none, EditorStyles.textField);
            GUI.color = Color.white;
        }
    }
}