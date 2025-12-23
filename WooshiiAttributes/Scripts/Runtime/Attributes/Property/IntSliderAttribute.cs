using UnityEngine;

public class IntSliderAttribute : PropertyAttribute
{
    public int Min { get; }
    public int Max { get; }

    public IntSliderAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}