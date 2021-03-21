using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginGroupHeaderLineAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public BeginGroupHeaderLineAttribute(string name)
        {
            Name = name;
        }
    }
}