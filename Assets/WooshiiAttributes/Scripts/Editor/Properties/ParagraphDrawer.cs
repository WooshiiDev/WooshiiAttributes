using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer(typeof(ParagraphAttribute))]
    public class ParagraphDrawer : WooshiiDecoratorDrawer
    {
        private ParagraphAttribute paragraph => attribute as ParagraphAttribute;
        private static GUIStyle smallStyle;

        private const float HEIGHT_PADDING = 4f;

        private Color textColor;
        private Color backgroundColor;

        public override void OnGUI(Rect position)
        {
            if (smallStyle == null)
            {
                smallStyle = new GUIStyle (GUI.skin.box)
                {
                    alignment = paragraph.Anchor,
                };

                if (ColorUtility.TryParseHtmlString (paragraph.TextColor, out textColor))
                {
                    smallStyle.normal.textColor = textColor;
                }

                if (!ColorUtility.TryParseHtmlString (paragraph.BackgroundColour, out backgroundColor))
                {
                    backgroundColor = GUI.color;
                }
            }

            position.height = GetParagraphHeight ();

            Color oldGUIColour = GUI.color;

            GUI.backgroundColor = backgroundColor;
            EditorGUI.LabelField (position, paragraph.Text, smallStyle);
            GUI.backgroundColor = oldGUIColour;
        }

        public override float GetHeight()
        {
            return GetParagraphHeight() + HEIGHT_PADDING;
        }

        private float GetParagraphHeight()
        {
            if (smallStyle == null)
            {
                return 19f;
            }

            return smallStyle.CalcHeight (new GUIContent (paragraph.Text), EditorGUIUtility.currentViewWidth);
        }
    }
}