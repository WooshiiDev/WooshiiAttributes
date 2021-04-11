using UnityEngine;

public class FloatClampAttribute : PropertyAttribute
{
    public float Min { get; }
    public float Max { get; }
    public bool ShowClamp { get; }

    public FloatClampAttribute(float min, float max, bool showClamp = false)
    {
        Min = min;
        Max = max;
        ShowClamp = showClamp;
    }
}