using UnityEngine;
using System;

namespace WooshiiAttributes
    {
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class CommentAttribute : PropertyAttribute
        {
        public readonly string text;
        public enum MessageType { NONE, WARNING, INFO, ERROR }
        public MessageType messageType = MessageType.INFO;

        /// <summary>
        /// Display a comment
        /// </summary>
        /// <param name="text">Text to show</param>
        public CommentAttribute(string text)
            {
            this.text = text;
            }

        /// <summary>
        /// Display a comment
        /// </summary>
        /// <param name="text">Text to show</param>
        /// <param name="messageType">Message display type</param>
        public CommentAttribute(string text, MessageType messageType)
            {
            this.text = text;
            this.messageType = messageType;
            }
        }

    }
