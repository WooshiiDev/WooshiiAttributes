using System;
using UnityEngine;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Vector3ClampAttribute : PropertyAttribute
    {
        public readonly float m_min;
        public readonly float m_max;

        public Vector3 value;

        /// <summary>
        /// Limit the values of a Vector3
        /// </summary>
        /// <param name="_min">Minimum x, y and z value</param>
        /// <param name="_max">Maximum x, y and z value</param>
        public Vector3ClampAttribute(float _min, float _max)
        {
            this.m_min = _min;
            this.m_max = _max;
        }
    }

    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Vector2ClampAttribute : PropertyAttribute
    {
        public readonly float m_min;
        public readonly float m_max;

        public Vector2 value;

        /// <summary>
        /// Limit the values of a Vector2
        /// </summary>
        /// <param name="_min">Minimum x and y value</param>
        /// <param name="_max">Maximum x and y value</param>
        public Vector2ClampAttribute(float _min, float _max)
        {
            this.m_min = _min;
            this.m_max = _max;
        }
    }
}