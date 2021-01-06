using System;
using UnityEngine;

namespace WooshiiAttributes
    {
    /// <summary>
    /// Contain a class in a little padded toggle view box
    /// </summary>
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ContainedClassAttribute : PropertyAttribute
        {
        /// <summary>
        /// Contain a class in a small padded box
        /// </summary>
        public ContainedClassAttribute()
            {
            
            }
        }
    }
