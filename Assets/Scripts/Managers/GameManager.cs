using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public TaskGenerator TaskGenerator;

    public event System.Action OnLevelPassed;
    public event System.Action OnGameLost;

    [HideInInspector] public GameState CurrentGameState;

    public enum GameState
    {
        Active,
        Win,
        Lost
    }

    private GameManager()
    {
    
    }

    private void Awake()
    {
        _instance = this;

        TaskGenerator = new TaskGenerator();

        CurrentGameState = GameState.Active;
    }

    public void GameLost()
    {
        CurrentGameState = GameState.Lost;
        OnGameLost?.Invoke();
    }

    public void LevelPassed()
    {
        OnLevelPassed?.Invoke();
        CurrentGameState = GameState.Win;
    }

    [ContextMenu ("Load")]
    public void LoadNextLevel()
    {
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        ScenesManager.LoadScene(ScenesManager.Scenes.Game);
    }
}
