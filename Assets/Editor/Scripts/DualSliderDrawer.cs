using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DualSlider))]
public class DualSliderDrawer : PropertyDrawer
{

    //The rect postion used for displaying the properties
    private Rect m_currentRect;

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        //Get the attribute values
        var duelSliderAttribute = (DualSlider)base.attribute;

        //Make sure the field is a Vector2
        if (_property.propertyType != SerializedPropertyType.Vector2)
        {
            EditorGUI.LabelField(_position, _label.text, "Use Dual Slider with Vector2.");
            return;
        }

        EditorGUI.BeginProperty(_position, _label, _property);

        m_currentRect = new Rect(_position.x, _position.y - 18, _position.width, 16);

        //Get the minimum, maximum and step values of the slider and store them as floats
        var sliderMinValue = duelSliderAttribute.sliderMinValue;

        var sliderMaxValue = duelSliderAttribute.sliderMaxValue;

        var stepValue = duelSliderAttribute.step;

        //Store the properties for the set slider values on each slider
        var minValueProp = _property.FindPropertyRelative(nameof(Vector2.x));

        var maxValueProp = _property.FindPropertyRelative(nameof(Vector2.y));


        //
        //Minimum slider
        //

        //Get the current value stoed
        float value = minValueProp.floatValue;



        //Create a slider
        value = EditorGUI.Slider(GetNextRect(), "Minimum", value, sliderMinValue, sliderMaxValue);

        //Set the value of the slider to be a multiple of the step value
        value = Step(value, stepValue);

        //Clamp the value of the slider based on the minimum and maximum values
        //The minimum value cannot be equal to the maximum value, so it's true maximum is the slider's max value minus the step value
        value = Mathf.Clamp(value, sliderMinValue, sliderMaxValue - stepValue);

        //Set the stored value based on the slider's value
        minValueProp.floatValue = value;

        //Move the other slider so that the maximum is always higher than the minimum
        if(minValueProp.floatValue >= maxValueProp.floatValue)
        {
            maxValueProp.floatValue = minValueProp.floatValue + stepValue;
        }



        //
        //Maximum slider
        //

        //Roughly the same process as before with minor differences in clamping
        value = maxValueProp.floatValue;

        value = EditorGUI.Slider(GetNextRect(), "Maximum", value, sliderMinValue, sliderMaxValue);

        value = Step(value, stepValue);

        //The maximum value cannot be equal to the minimum value, so it's true minimum is the slider's minimum value plus the step value
        value = Mathf.Clamp(value, sliderMinValue + stepValue, sliderMaxValue + stepValue);

        maxValueProp.floatValue = value;

        //Move the slider so that the minimum is always lower than the maximum
        if (maxValueProp.floatValue <= minValueProp.floatValue)
        {
            minValueProp.floatValue = maxValueProp.floatValue - stepValue;
        }

        EditorGUI.EndProperty();

    }

    //Round the value based on the set step incriment value
    private float Step(float _value, float _step)
    {
        //Make sure the value is not 0
        if (_step == 0)
            return _value;

        //Divide the inital value by the step value, then store it as an int
        //this is the amount of "steps" the value has taken
        int amount = (int)(_value / _step);

        //Multiply the amount of steps by the rangeattribute step to get the new value
        _value = amount * _step;

        return _value;
    }

    /// <summary>
    /// Get the next postion that a field should be placed
    /// </summary>
    /// <returns></returns>
    private Rect GetNextRect()
    {
        m_currentRect = new Rect(m_currentRect.x, m_currentRect.y + 18, m_currentRect.width, 16);

        return m_currentRect;
    }

}
