using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class DualSlider: PropertyAttribute
{
    
    public float sliderMinValue;

    public float sliderMaxValue;

    //public float minValue;

    //public float maxValue;

    public float step;

    public DualSlider(float _minValue, float _maxValue, float _step)
    {
        sliderMinValue = _minValue;

        sliderMaxValue = _maxValue;

        step = _step;
    }
}
