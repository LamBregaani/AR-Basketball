using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Rules/Time Limit", fileName = "New Time Limit GameRule")]
public class TimeLimit : GameRule
{
    [SerializeField] private int m_timeLimit;

    private GameModeController m_controller;

    private float m_timeRemaining;


    public event Action onTimeRunOut = delegate { };

    public event Action<float> onTimeUpdated = delegate { };

    ~TimeLimit()
    {
        onTimeRunOut -= m_controller.EndGame;
    }

    public override void Init(GameModeController controller)
    {
        m_controller = controller;

        onTimeRunOut += controller.EndGame;

        m_timeRemaining = m_timeLimit;
    }

    public override void Update()
    {

        m_timeRemaining-= Time.deltaTime;
        
        m_timeRemaining = Mathf.Clamp(m_timeRemaining, 0, Mathf.Infinity);

        onTimeUpdated?.Invoke(m_timeRemaining);

        Debug.Log($"Time Left: {m_timeRemaining}");

        if (m_timeRemaining == 0)
            onTimeRunOut?.Invoke();
    }

    public override void ResetRule()
    {
        m_timeRemaining = m_timeLimit;
    }
}
