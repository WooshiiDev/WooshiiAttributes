using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Clamp the field to the given float range. Must be a supported value type.
    /// </summary>
    public class FloatSliderAttribute : PropertyAttribute
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public float Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public float Max { get; }

        public FloatSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}