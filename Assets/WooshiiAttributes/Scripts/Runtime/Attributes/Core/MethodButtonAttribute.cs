﻿using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MethodButtonAttribute : Attribute
    {
        public string MethodName { get; set; } = null;

        public MethodButtonAttribute()
        {
 
        }

        public MethodButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}