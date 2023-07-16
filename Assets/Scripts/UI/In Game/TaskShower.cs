using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TaskShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private void Start()
    {
        string generatedTaskText = GameManager.Instance.TaskGenerator.GetTask();

        _textMeshProUGUI.text = generatedTaskText;
    }
}
