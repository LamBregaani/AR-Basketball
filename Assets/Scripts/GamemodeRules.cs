using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// A collection of game rules for a gamemode
/// </summary>
[CreateAssetMenu(fileName = "New Game Mode", menuName = "Game Mode")]
public class GamemodeRules : ScriptableObject
{
    [SerializeField] private string m_gamemodeName;

    public string GamemodeName { get { return m_gamemodeName; } }   

    [SerializeField] private List<GameRule> m_gameRules = new List<GameRule>();

    public List<GameRule> GameRules { get { return m_gameRules; } }

}
