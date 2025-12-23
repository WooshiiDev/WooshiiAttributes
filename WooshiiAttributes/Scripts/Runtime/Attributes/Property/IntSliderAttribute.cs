using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draw a slider for this field. Must be a supported value type.
    /// </summary>
    public class IntSliderAttribute : PropertyAttribute
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public int Max { get; }

        public IntSliderAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}