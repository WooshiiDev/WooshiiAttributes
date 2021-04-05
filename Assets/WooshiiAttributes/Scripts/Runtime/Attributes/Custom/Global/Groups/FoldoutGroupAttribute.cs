using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FoldoutGroupAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public FoldoutGroupAttribute(string name)
        {
            Name = name;
        }
    }
}