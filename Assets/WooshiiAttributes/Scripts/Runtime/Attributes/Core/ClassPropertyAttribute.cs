using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClassPropertyAttribute : Attribute
    {
        public ClassPropertyAttribute()
        {

        }
    }
}