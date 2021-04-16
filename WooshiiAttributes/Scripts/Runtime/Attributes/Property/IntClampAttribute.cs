using UnityEngine;

public class IntClampAttribute : PropertyAttribute
{
    public int Min { get; }
    public int Max { get; }
    public bool ShowClamp { get; }

    public IntClampAttribute(int _min, int _max, bool _showClamp = false)
    {
        Min = _min;
        Max = _max;
        ShowClamp = _showClamp;
    }
}