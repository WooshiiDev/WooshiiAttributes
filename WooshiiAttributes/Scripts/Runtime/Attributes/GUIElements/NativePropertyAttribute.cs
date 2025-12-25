using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NativePropertyAttribute : GUIElementAttribute
    {
        public NativePropertyAttribute()
        {

        }
    }
}