using System;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Represents when a field can be displayed.
    /// </summary>
    public enum DisplayMode { EDITOR, PLAYING, BOTH }

    /// <summary>
    /// Set a field to readonly.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        /// <summary>
        /// When to display this field.
        /// </summary>
        public readonly DisplayMode _displayMode;

        /// <summary>
        /// Display a readonly value in the inspector
        /// </summary>
        public ReadOnlyAttribute()
        {
            _displayMode = DisplayMode.BOTH;
        }

        /// <summary>
        /// Display a readonly value in the inspector
        /// </summary>
        /// <param name="_displayMode">When is the value displayed</param>
        public ReadOnlyAttribute(DisplayMode _displayMode)
        {
            this._displayMode = _displayMode;
        }
    }
}