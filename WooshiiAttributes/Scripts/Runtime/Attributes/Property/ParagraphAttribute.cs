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

        public ParagraphAttribute(string _text, TextAnchor _textAnchor = TextAnchor.MiddleLeft)
        {
            Text = _text;
            Anchor = _textAnchor;
        }

        public ParagraphAttribute(string _text, string _textColor = "#D2D2D2", TextAnchor _textAnchor = TextAnchor.MiddleLeft)
        {
            Text = _text;
            TextColor = _textColor;

            Anchor = _textAnchor;
        }

        public ParagraphAttribute(string _text, string _textColor = "#D2D2D2", string _backgroundColour = "#787878", TextAnchor _textAnchor = TextAnchor.MiddleLeft)
        {
            Text = _text;
            TextColor = _textColor;
            BackgroundColour = _backgroundColour;

            Anchor = _textAnchor;
        }
    }
}