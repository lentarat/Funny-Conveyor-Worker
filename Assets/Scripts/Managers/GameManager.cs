using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public TaskGenerator TaskGenerator = new TaskGenerator();

    private GameManager()
    {
    
    }

    private void Awake()
    {
        _instance = this;
    }

    public void GameLost()
    {
        Time.timeScale = 0f;
    }
}
