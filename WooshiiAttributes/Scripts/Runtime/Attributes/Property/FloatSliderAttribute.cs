using UnityEngine;

public class FloatSliderAttribute : PropertyAttribute
{
    public float Min { get; }
    public float Max { get; }

    public FloatSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}