using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : MonoBehaviour
{
    [SerializeField] private GamemodeRules gamemodeRules;

    private void Awake()
    {
        StartGamemode();
    }

    public void StartGamemode()
    {
        foreach (var _rules in gamemodeRules.GameRules)
        {
            _rules.Init(this);
        }
    }

    public void ChangeGamemode(GamemodeRules _rules)
    {
        gamemodeRules = _rules;
    }

    public void CleanUpGamemode()
    {

    }

    public void RestartGamemode()
    {
        CleanUpGamemode();

        StartGamemode();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended!");
    }


    // Update is called once per frame
    void Update()
    {
        foreach (var _rules in gamemodeRules.GameRules)
        {
            _rules.Update();
        }
    }
}
