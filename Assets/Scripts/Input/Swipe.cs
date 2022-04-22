using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Swipe
{
    public Vector2 startPosition;

    public Vector2 currentPosition;

    public Vector2 endPosition;

    public float distance;

    public Vector2 direction2D;

    public Swipe(Vector2 _pos)
    {
        startPosition = _pos;

        currentPosition = new Vector2();

        endPosition = new Vector2();

        distance = 0;

        direction2D = Vector2.zero;
    }

    public void CalculateDistance(Vector2 _pos)
    {
        currentPosition = _pos;

        direction2D = _pos - startPosition;

        var touchDistance = startPosition.y - _pos.y;

        distance = Mathf.Clamp(touchDistance, 1f, Mathf.Infinity);
    }

    public void SwipeEnded(Vector2 _pos)
    {
        endPosition = _pos;

        CalculateDistance(_pos);

    }
}
