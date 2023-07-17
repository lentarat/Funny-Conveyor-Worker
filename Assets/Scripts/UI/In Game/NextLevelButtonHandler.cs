using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _nextLevelButton;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(LoadNextLevel);

        GameManager.Instance.OnLevelPassed += SetButtonActive;
    }

    private void LoadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    private void SetButtonActive()
    {
        _nextLevelButton.gameObject.SetActive(true);
    }
}
