using System;

namespace WooshiiAttributes
{
    /// <summary>
    /// Add a button control for this method.
    /// </summary>
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MethodButtonAttribute : GUIElementAttribute
    {
        /// <summary>
        /// The display name of the method.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The default argument values.
        /// </summary>
        public object[] Arguments { get; private set; }

        public MethodButtonAttribute()
        {

        }

        public MethodButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public MethodButtonAttribute(string methodName, params object[] args)
        {
            MethodName = methodName;
            Arguments = args;
        }
    }
}