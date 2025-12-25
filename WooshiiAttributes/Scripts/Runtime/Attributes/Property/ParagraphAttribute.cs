using System;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draws a paragraph above a field.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ParagraphAttribute : PropertyAttribute
    {
        /// <summary>
        /// The text to draw.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The text anchor.
        /// </summary>
        public TextAnchor Anchor { get; }

        /// <summary>
        /// The text colour.
        /// </summary>
        public string TextColor { get; } = "#D2D2D2";

        /// <summary>
        /// The background colour.
        /// </summary>
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