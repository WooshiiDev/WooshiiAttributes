using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class WooshiiPropertyDrawer : PropertyDrawer
    {
        //Cached
        protected float m_lineHeight = EditorGUIUtility.singleLineHeight;

        //Custom Methods
        protected void DrawBackground(Rect _rect, Color _color)
        {
            GUI.color = _color;
            GUI.Box (_rect, GUIContent.none, EditorStyles.textField);
            GUI.color = Color.white;
        }
    }
}