using UnityEngine;

public class IntClampAttribute : PropertyAttribute
{
    public int Min { get; }
    public int Max { get; }
    public bool ShowClamp { get; }

    public IntClampAttribute(int min, int max, bool showClamp = false)
    {
        Min = min;
        Max = max;
        ShowClamp = showClamp;
    }
}