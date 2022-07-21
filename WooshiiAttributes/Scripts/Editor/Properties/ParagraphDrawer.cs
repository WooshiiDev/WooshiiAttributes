using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer(typeof(ParagraphAttribute))]
    public class ParagraphDrawer : WooshiiDecoratorDrawer
    {
        private const float HEIGHT_PADDING = 4f;
        private static GUIStyle SmallStyle;

        private ParagraphAttribute Target => attribute as ParagraphAttribute;

        private Color m_textColor;
        private Color m_backgroundColor;

        public override void OnGUI(Rect _position)
        {
            Color oldBackgroundColour = GUI.backgroundColor;

            if (SmallStyle == null)
            {
                SmallStyle = new GUIStyle (GUI.skin.box)
                {
                    alignment = Target.Anchor,
                };
            }

            if (ColorUtility.TryParseHtmlString(Target.TextColor, out m_textColor))
            {
                SmallStyle.normal.textColor = m_textColor;
            }

            if (!ColorUtility.TryParseHtmlString(Target.BackgroundColour, out m_backgroundColor))
            {
                m_backgroundColor = oldBackgroundColour;
            }

            _position.height = GetParagraphHeight ();

            GUI.backgroundColor = m_backgroundColor;
            EditorGUI.LabelField (_position, Target.Text, SmallStyle);
            GUI.backgroundColor = oldBackgroundColour;
        }

        public override float GetHeight()
        {
            return GetParagraphHeight() + HEIGHT_PADDING;
        }

        private float GetParagraphHeight()
        {
            if (SmallStyle == null)
            {
                return 19f;
            }

            return SmallStyle.CalcHeight (new GUIContent (Target.Text), EditorGUIUtility.currentViewWidth);
        }
    }
}