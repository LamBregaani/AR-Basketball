using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal : MonoBehaviour
{
    [SerializeField] private float m_pointMultiplier;

    public static EventWrapper<Ball, Goal> onGoalHitGlobal = new EventWrapper<Ball, Goal>();

    public EventWrapper onGoalHit = new EventWrapper();

    public float PointMultiplier { get { return m_pointMultiplier; } private set { m_pointMultiplier = value; } }


    private void OnTriggerEnter(Collider other)
    {

        if (CheckForComponent.CheckComponent<Ball>(other.gameObject, out Ball ball))
        {
            if(CheckCollision.CheckDirection(ball.transform.position, transform.position, transform.up))
            {
                ball.CanMiss = false;

                onGoalHit?.InvokeEvent();

                onGoalHitGlobal?.InvokeEvent(ball, this);
            }

        }

    }
}
