using System;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Display a header with an underline
    /// </summary>
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HeaderLineAttribute : PropertyAttribute
    {
        /// <summary>
        /// The title for this header.
        /// </summary>
        public readonly string Text;

        public HeaderLineAttribute(string text)
        {
            this.Text = text;
        }
    }
}