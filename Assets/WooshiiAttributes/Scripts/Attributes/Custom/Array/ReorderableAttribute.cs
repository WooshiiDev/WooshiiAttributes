using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReorderableAttribute : ArrayAttribute
    {
    }
}