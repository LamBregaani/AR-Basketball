using System;
using UnityEngine;

/// <summary>
/// Extended range attribute with custom step incriments
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class RangeExAttribute : PropertyAttribute
{

    //The minimum value of the range
    public readonly float min;
    //The maximum value of the range
    public readonly float max;
    //The amount to incriment by
    public readonly float step;

    public RangeExAttribute(float min, float max, float step)
    {
        this.min = min;
        this.max = max;
        this.step = step;
    }
}
