using UnityEngine;
using System;

namespace WooshiiAttributes
    {
    /// <summary>
    /// Display a header with an underline
    /// </summary>
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HeaderLineAttribute : PropertyAttribute
        {
        public readonly string text;

        //Constructor
        public HeaderLineAttribute(string text)
            {
            this.text = text;
            }
        }

    }
