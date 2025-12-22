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

        public MethodButtonAttribute(string _methodName)
        {
            MethodName = _methodName;
        }

        public MethodButtonAttribute(string _methodName, params object[] _args)
        {
            MethodName = _methodName;
            Arguments = _args;
        }
    }
}