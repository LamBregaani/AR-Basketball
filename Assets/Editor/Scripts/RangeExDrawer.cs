using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RangeExAttribute))]
public class RangeExDrawer : PropertyDrawer
{
    private float value;


    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        var rangeAttribute = (RangeExAttribute)base.attribute;

        if (_property.propertyType == SerializedPropertyType.Float || _property.propertyType == SerializedPropertyType.Integer)
        {
            value = _property.floatValue;

            value = EditorGUI.Slider(_position, _label, value, rangeAttribute.min, rangeAttribute.max);

            value = Step(value, rangeAttribute);

            value = Mathf.Clamp(value, rangeAttribute.min, rangeAttribute.max);

            _property.floatValue = value;
        }
        else
        {
            EditorGUI.LabelField(_position, _label.text, "Use Range with float or int.");
        }
    }

    //Round the value based on the set step incriment value
    private float Step(float _value, RangeExAttribute _rangeAttribute)
    {
        //Make sure the value is not 0
        if(_rangeAttribute.step == 0)
            return _value;

        //Divide the inital value by the step value, then store it as an int
        //this is the amount of "steps" the value has taken
        int amount = (int)(_value / _rangeAttribute.step);

        //Multiply the amount of steps by the rangeattribute step to get the new value
        _value = amount * _rangeAttribute.step;

        return _value;
    }
}
