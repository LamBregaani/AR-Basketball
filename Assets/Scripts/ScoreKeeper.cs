using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private float m_score;

    public static EventWrapper<float> onScoreUpdated = new EventWrapper<float>();


    public void OnEnable()
    {
        Goal.onGoalHitGlobal.AddListener(AddScore);
    }

    public void AddScore(Ball _ball, Goal _goal)
    {
        var addedScore = _ball.BallValue * _goal.PointMultiplier;

        m_score += addedScore;

        onScoreUpdated?.InvokeEvent(m_score);
    }
}
