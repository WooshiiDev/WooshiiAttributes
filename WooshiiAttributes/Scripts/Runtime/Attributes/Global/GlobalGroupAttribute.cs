using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GlobalGroupAttribute : GlobalAttribute
    {
        public string Name { get; private set; }

        public bool Uppercase   { get; private set; }
        public bool Underline   { get; private set; }
        public bool Foldout     { get; private set; }
        public bool Contained   { get; private set; }

        public GlobalGroupAttribute(string name, bool uppercase = false, bool underline = false, bool foldout = false, bool contained = false)
        {
            Name = name;

            Uppercase = uppercase;
            Underline = underline;
            Foldout = foldout;
            Contained = contained;
        }

        public override string GetValidator()
        {
            return Name;
        }
    }
}