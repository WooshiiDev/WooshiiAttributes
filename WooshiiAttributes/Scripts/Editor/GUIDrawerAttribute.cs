using System;

namespace WooshiiAttributes
{
    /// <summary>
    /// The base attribute representing any custom drawers.
    /// </summary>
    public class GUIDrawerAttribute : Attribute
    {
        /// <summary>
        /// The target GUIElement.
        /// </summary>
        public Type Element { get; }

        public GUIDrawerAttribute(Type element)
        {
            Element = element;
        }
    }
}