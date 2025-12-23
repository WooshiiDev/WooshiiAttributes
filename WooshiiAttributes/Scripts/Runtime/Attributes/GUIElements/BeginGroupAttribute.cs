using System;

namespace WooshiiAttributes
{
    /// <summary>
    /// Begin a group.
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginGroupAttribute : GroupAttribute
    {
        public BeginGroupAttribute(string title, bool groupedTitle = false, bool upperTitle = false, bool underlineTitle = false) 
            : base (title, groupedTitle, upperTitle, underlineTitle)
        {
            
        }

    }
}