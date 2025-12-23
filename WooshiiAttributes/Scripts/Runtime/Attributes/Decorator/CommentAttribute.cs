using System;
using UnityEngine;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class CommentAttribute : PropertyAttribute
    {
        public readonly string Text;

        public enum MessageType { NONE, WARNING, INFO, ERROR }

        public MessageType messageType = MessageType.INFO;

        /// <summary>
        /// Display a comment
        /// </summary>
        /// <param name="text">Text to show</param>
        public CommentAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Display a comment
        /// </summary>
        /// <param name="text">Text to show</param>
        /// <param name="messageType">Message display type</param>
        public CommentAttribute(string text, MessageType messageType)
        {
            Text = text;
            this.messageType = messageType;
        }
    }
}