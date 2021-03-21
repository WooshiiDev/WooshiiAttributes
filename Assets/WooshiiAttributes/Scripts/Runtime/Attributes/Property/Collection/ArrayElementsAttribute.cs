using System;
using UnityEngine;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ArrayElementsAttribute : PropertyAttribute
    {
        public ArrayElementsAttribute()
        {
        }
    }
}