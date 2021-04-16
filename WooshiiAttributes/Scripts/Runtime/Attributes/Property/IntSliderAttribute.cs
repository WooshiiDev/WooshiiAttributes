using UnityEngine;

public class IntSliderAttribute : PropertyAttribute
{
    public int Min { get; }
    public int Max { get; }

    public IntSliderAttribute(int _min, int _max)
    {
        Min = _min;
        Max = _max;
    }
}