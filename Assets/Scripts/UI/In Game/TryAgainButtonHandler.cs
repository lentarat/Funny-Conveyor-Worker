using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TryAgainButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButton;

    private void Start()
    {
        _tryAgainButton.onClick.AddListener(LoadNextLevel);

        GameManager.Instance.OnGameLost += SetButtonActive;
    }

    private void LoadNextLevel()
    {
        GameManager.Instance.ReloadLevel();
    }

    private void SetButtonActive()
    {
        _tryAgainButton.gameObject.SetActive(true);
    }
}
