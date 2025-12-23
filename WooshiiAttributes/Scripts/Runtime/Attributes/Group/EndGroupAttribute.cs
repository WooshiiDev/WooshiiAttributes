using System;

namespace WooshiiAttributes
{
    /// <summary>
    /// End a group (if one currently exists).
    /// </summary>
    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EndGroupAttribute : GroupAttribute
    {
        public EndGroupAttribute()
        {
        }
    }
}