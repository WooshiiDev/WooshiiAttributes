using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Clamp the field to the given float range. Must be a supported value type.
    /// </summary>
    public class FloatClampAttribute : PropertyAttribute
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public float Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public float Max { get; }

        /// <summary>
        /// Should the clamp be shown.
        /// </summary>
        public bool ShowClamp { get; }

        public FloatClampAttribute(float min, float max, bool showClamp = false)
        {
            Min = min;
            Max = max;
            ShowClamp = showClamp;
        }
    }
}