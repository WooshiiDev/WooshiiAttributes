using System;

namespace WooshiiAttributes
{
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