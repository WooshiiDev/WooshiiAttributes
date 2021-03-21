using System;

namespace WooshiiAttributes
    {
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HeaderLineGroupAttribute : GlobalAttribute
        {
        public string Name { get; private set; }

        public HeaderLineGroupAttribute(string name)
            {
            Name = name;
            }
        }
    }
