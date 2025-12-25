using System;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draw a comment above the field.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class CommentAttribute : PropertyAttribute
    {
        /// <summary>
        /// Represents various message types.
        /// </summary>
        public enum MessageType { NONE, WARNING, INFO, ERROR }

        /// <summary>
        /// The comment text.
        /// </summary>
        public readonly string Text;

        /// <summary>
        /// The type of message.
        /// </summary>
        public readonly MessageType messageType = MessageType.INFO;

        /// <summary>
        /// Display a comment.
        /// </summary>
        /// <param name="text">The text to show.</param>
        public CommentAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Display a comment.
        /// </summary>
        /// <param name="text">The text to show.</param>
        /// <param name="messageType">The message display type.</param>
        public CommentAttribute(string text, MessageType messageType)
        {
            Text = text;
            this.messageType = messageType;
        }
    }
}