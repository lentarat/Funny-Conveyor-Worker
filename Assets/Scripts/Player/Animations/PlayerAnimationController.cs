using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        GameManager.Instance.OnLevelPassed += Dance;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelPassed -= Dance;
    }

    private void Dance()
    {
        _animator.Play("Dance");
    }
}
