using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public TaskGenerator TaskGenerator = new TaskGenerator();

    public event System.Action OnLevelPassed;

    public GameState CurrentGameState;

    public enum GameState
    {
        Active,
        Win
    }

    private GameManager()
    {
    
    }

    private void Awake()
    {
        _instance = this;

        CurrentGameState = GameState.Active;
    }

    public void GameLost()
    {
        Time.timeScale = 0f;
    }

    public void LevelPassed()
    {
        OnLevelPassed?.Invoke();
        CurrentGameState = GameState.Win;
    }
}
