using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EndGroupAttribute : GroupAttribute
    {
        public EndGroupAttribute()
        {
        }
    }
}