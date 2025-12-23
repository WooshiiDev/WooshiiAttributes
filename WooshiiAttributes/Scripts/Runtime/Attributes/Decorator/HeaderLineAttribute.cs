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
        public readonly string Text;

        //Constructor
        public HeaderLineAttribute(string text)
        {
            this.Text = text;
        }
    }
}