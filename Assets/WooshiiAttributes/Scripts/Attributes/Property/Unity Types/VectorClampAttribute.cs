using System;
using UnityEngine;

namespace WooshiiAttributes
    {
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Vector3ClampAttribute : PropertyAttribute
        {
        public readonly float min;
        public readonly float max;

        public Vector3 value;


        /// <summary>
        /// Limit the values of a Vector3 
        /// </summary>
        /// <param name="min">Minimum x, y and z value</param>
        /// <param name="max">Maximum x, y and z value</param>
        public Vector3ClampAttribute(float min, float max)
            {
            this.min = min;
            this.max = max;
            }
        }

    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Vector2ClampAttribute : PropertyAttribute
        {
        public readonly float min;
        public readonly float max;

        public Vector2 value;

        /// <summary>
        /// Limit the values of a Vector2 
        /// </summary>
        /// <param name="min">Minimum x and y value</param>
        /// <param name="max">Maximum x and y value</param>
        public Vector2ClampAttribute(float min, float max)
            {
            this.min = min;
            this.max = max;
            }
        }
    }
