using System;
using UnityEngine;

namespace WooshiiAttributes
{
    public enum DisplayMode { EDITOR, PLAYING, BOTH }

    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
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