using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesManager
{
    public enum Scenes
    {
        Game
    }

    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
