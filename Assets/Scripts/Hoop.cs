using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hoop
{
    public GameObject hoopObj;

    public GameObject baseObj;

    public GameObject poseObj;

    public GameObject postBendObj;

    public bool randomizeHeight;

    [DualSlider(0.5f, 5f, 0.5f)]
    public Vector2 randomHeightSlider;

    [Tooltip("Height in meteres. Increments by 0.5m")]
    [RangeEx(0.5f, 5, 0.5f)]
    public float height = 1f;

    public bool canMove;

    public enum MovementType { None, BackAndForth, Circular }

    public MovementType movementType;

    public bool randomizeSpeed;

    [DualSlider(0.1f, 10f, 0.1f)]
    public Vector2 randomSpeedSlider;

    [RangeEx(0.1f, 10f, 0.1f)]
    public float movementSpeed = 1;

    public bool hasBase;

    public float GetHeight()
    {
        var heightVal = height;

        if (randomizeHeight)
        {
            heightVal = Random.Range(randomHeightSlider.x, randomHeightSlider.y);

            heightVal = Step(heightVal, 0.5f);
        }

        return heightVal;
    }

    public float GetSpeed()
    {
        if (!canMove)
            return 0;

        var speedVal = movementSpeed;

        if (randomizeHeight)
        {
            speedVal = Random.Range(randomHeightSlider.x, randomHeightSlider.y);

            speedVal = Step(speedVal, 0.1f);
        }

        return speedVal;
    }

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
}
