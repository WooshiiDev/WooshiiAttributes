using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginGroupAttribute : GroupAttribute
    {
        public BeginGroupAttribute(string title, bool groupedTitle = false, bool upperTitle = false, bool underlineTitle = false) 
            : base (title, groupedTitle, upperTitle, underlineTitle)
        {
            
        }

    }
}