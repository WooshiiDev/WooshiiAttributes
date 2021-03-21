using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ContainedGroupAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public ContainedGroupAttribute(string name)
        {
            Name = name;
        }
    }
}