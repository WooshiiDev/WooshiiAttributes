using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SelectableArrayAttribute : ArrayAttribute
        {

        }
    }
