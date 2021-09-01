using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HeaderGroupAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public HeaderGroupAttribute(string name)
        {
            Name = name;
        }
    }
}