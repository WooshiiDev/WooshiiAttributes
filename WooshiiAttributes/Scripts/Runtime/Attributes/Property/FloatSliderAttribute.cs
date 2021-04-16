using UnityEngine;

public class FloatSliderAttribute : PropertyAttribute
{
    public float Min { get; }
    public float Max { get; }

    public FloatSliderAttribute(float _min, float _max)
    {
        Min = _min;
        Max = _max;
    }
}