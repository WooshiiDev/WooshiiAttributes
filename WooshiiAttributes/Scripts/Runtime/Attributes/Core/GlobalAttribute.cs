using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class GlobalAttribute : Attribute
    {
        public abstract string GetIdenifier();
    }
}