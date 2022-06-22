using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Rules/Ball Limit", fileName = "New Ball Limit")]
public class BallLimit : GameRule
{
    [SerializeField] private int m_maxBalls;

    private int m_ballsRemaining;

    public event Action onRunOut = delegate { };

    public override void Init(GameModeController controller)
    {
        onRunOut += controller.EndGame;

        ThrowBall.onBallthrownGlobal += SetBallAmount;

        m_ballsRemaining = m_maxBalls;
    }

    public void SetBallAmount()
    {
        m_ballsRemaining--;

        Debug.Log(m_ballsRemaining);

        if(m_ballsRemaining <= 0)
        {
            onRunOut?.Invoke();
        }
    }

    public override void ResetRule()
    {
        m_ballsRemaining = m_maxBalls;
    }


}
