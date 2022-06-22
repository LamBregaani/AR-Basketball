using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;


    public void OnEnable()
    {
        ScoreKeeper.onScoreUpdated.AddListener(UpdateScore);
    }

    public void OnDisable()
    {
        ScoreKeeper.onScoreUpdated.RemoveListener(UpdateScore);
    }

    public void UpdateScore(float val)
    {
        m_text.text = $"Score: {val}";
    }
}
