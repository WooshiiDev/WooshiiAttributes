using System;
using UnityEngine;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ParagraphAttribute : PropertyAttribute
    {
        public string Text { get; }

        public TextAnchor Anchor { get; }

        public string TextColor { get; } = "#D2D2D2";
        public string BackgroundColour { get; } = "#787878";

        public ParagraphAttribute(string text, TextAnchor textAnchor = TextAnchor.MiddleLeft)
        {
            Text = text;
            Anchor = textAnchor;
        }

        public ParagraphAttribute(string text, string textColor = "#D2D2D2", TextAnchor textAnchor = TextAnchor.MiddleLeft)
        {
            Text = text;
            TextColor = textColor;

            Anchor = textAnchor;
        }

        public ParagraphAttribute(string text, string textColor = "#D2D2D2", string backgroundColour = "#787878", TextAnchor textAnchor = TextAnchor.MiddleLeft)
        {
            Text = text;
            TextColor = textColor;
            BackgroundColour = backgroundColour;

            Anchor = textAnchor;
        }
    }
}