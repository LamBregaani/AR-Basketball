using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LoseOnMisses GameRule", menuName = "Game Rules/Lose On Miss")]
public class MissLimit : GameRule
{
    [SerializeField] private int m_maxMisses;

    private int m_missesRemaining;

    private GameModeController m_controller;


    public event Action onMiss = delegate { };

    ~MissLimit()
    {
        onMiss -= m_controller.EndGame;
    }

    public override void Init(GameModeController controller)
    {
        m_controller = controller;

        onMiss += controller.EndGame;

        Ball.onGroundHit.AddListener(BallMissed);

        m_missesRemaining = m_maxMisses;
    }

    public override void ResetRule()
    {
        m_missesRemaining = m_maxMisses;
    }

    public void BallMissed(Ball ball)
    {
        if (!ball.CanMiss)
            return;


        if(m_missesRemaining == 0)
        {
            onMiss?.Invoke();
            return;
        }

        m_missesRemaining--;
    }
}
