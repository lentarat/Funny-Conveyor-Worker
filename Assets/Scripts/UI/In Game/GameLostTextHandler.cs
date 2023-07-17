using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLostTextHandler : MonoBehaviour
{
    [SerializeField] private GameObject _gameLostTextParent;

    private void Start()
    {
        GameManager.Instance.OnGameLost += ShowText;
    }

    private void ShowText()
    {
        _gameLostTextParent.gameObject.SetActive(true);
    }
}
