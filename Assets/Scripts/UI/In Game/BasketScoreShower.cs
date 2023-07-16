using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasketScoreShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _plusOneText;

    [Header("Basket Position")]
    [SerializeField] private Transform _basketTransform;

    [Header("Common")]
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _raiseSpeed;

    private Vector3 _initialPlusOneTextPosition;
    private float _timeAtStart;

    private void Awake()
    {
        Basket.PickableObjectWasAddedToBasket += ShowPlusOnePoint;

        _plusOneText.transform.position = _basketTransform.position;
        _initialPlusOneTextPosition = _plusOneText.transform.position;
    }

    private void ShowPlusOnePoint()
    {
        Reset();

        StartCoroutine(RaiseAndFadeEffect());
    }

    private void Reset()
    {
        _plusOneText.alpha = 1f;
        _plusOneText.transform.position = _initialPlusOneTextPosition;
    }

    private IEnumerator RaiseAndFadeEffect()
    {
        _plusOneText.gameObject.SetActive(true);

        _timeAtStart = Time.time;

        while (_timeAtStart + _fadeTime > Time.time)
        {
            Raise();
            Fade();

            yield return null;
        }

        _plusOneText.gameObject.SetActive(false);

        yield return null;
    }

    private void Raise()
    {
        _plusOneText.transform.position += Vector3.up * _raiseSpeed * Time.time;
    }

    private void Fade()
    {
        float blendValue = (Time.time - _timeAtStart) / _fadeTime;
        _plusOneText.alpha = 1 - blendValue;
    }
}
