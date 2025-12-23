using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MethodButtonAttribute : GUIElementAttribute
    {
        public string MethodName { get; set; }
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