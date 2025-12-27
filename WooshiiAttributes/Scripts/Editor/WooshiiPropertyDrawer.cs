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

        protected float SingleLineHeight = EditorGUIUtility.singleLineHeight;
        protected float StandardSpacing = EditorGUIUtility.standardVerticalSpacing;

        // - Methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property, _label, _property.isExpanded) + StandardSpacing;
        }

        protected void DrawBackground(Rect _rect)
        {
            Color c = GUI.color;
            GUI.Box(_rect, string.Empty, GUI.skin.box);
        }

        protected Rect GetSingleLineControlRect(Rect rect)
        {
            rect.height = SingleLineHeight;
            return rect;
        }
    }
}