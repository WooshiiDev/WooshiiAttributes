using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ArrayElementsAttribute : Attribute
    {
        public ArrayElementsAttribute()
        {
        }
    }
}