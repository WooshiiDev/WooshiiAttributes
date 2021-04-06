using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HeaderLineGroupAttributeAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public HeaderLineGroupAttributeAttribute(string name)
        {
            Name = name;
        }
    }
}