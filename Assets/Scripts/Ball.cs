using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int pointValue;

    private bool m_canMiss = true;

    public bool CanMiss { get { return m_canMiss; } set { m_canMiss = value; } }

    public static EventWrapper<Ball> onGroundHit = new EventWrapper<Ball>();    

    public int BallValue { get { return pointValue; } }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGroundHit.InvokeEvent(this);
            return;
        }
    }
}
