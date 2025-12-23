using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer(typeof(ParagraphAttribute))]
    public class ParagraphDrawer : WooshiiDecoratorDrawer
    {
        private const float HEIGHT_PADDING = 4f;
        private static GUIStyle _smallStyle;

        private Color _textColor;
        private Color _backgroundColor;

        private ParagraphAttribute Target => attribute as ParagraphAttribute;

        public override void OnGUI(Rect position)
        {
            if (_smallStyle == null)
            {
                _smallStyle = new GUIStyle (GUI.skin.box)
                {
                    alignment = Target.Anchor,
                };

                if (ColorUtility.TryParseHtmlString (Target.TextColor, out _textColor))
                {
                    _smallStyle.normal.textColor = _textColor;
                }

                if (!ColorUtility.TryParseHtmlString (Target.BackgroundColour, out _backgroundColor))
                {
                    _backgroundColor = GUI.color;
                }
            }

            position.height = GetParagraphHeight ();

            Color oldGUIColour = GUI.color;

            GUI.backgroundColor = _backgroundColor;
            EditorGUI.LabelField (position, Target.Text, _smallStyle);
            GUI.backgroundColor = oldGUIColour;
        }

        public override float GetHeight()
        {
            return GetParagraphHeight() + HEIGHT_PADDING;
        }

        private float GetParagraphHeight()
        {
            if (_smallStyle == null)
            {
                return 19f;
            }

            return _smallStyle.CalcHeight (new GUIContent (Target.Text), EditorGUIUtility.currentViewWidth);
        }
    }
}