using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsWithBasketHandler : MonoBehaviour
{
    [Header("ik Transforms")]
    [SerializeField] private Transform _ikTargetTransform;

    [Header("Basket Transforms")]
    [SerializeField] private Transform _basketLiftDestination;
    [SerializeField] private Transform _basketTransform;

    [Header("Common")]
    [SerializeField] private float _evaluationSpeed;

    private float _blendValue;

    private Vector3 _basketStartPosition;

    private Coroutine _pullBasketCoroutine;
    private bool _isPullBasketCoroutineRunning;

    private bool _isPullingUp;

    private void Start()
    {
        _basketStartPosition = _ikTargetTransform.position;
    }

    [ContextMenu("Up")]
    public void LiftBasket()
    {
        _isPullingUp = true;

        if (_isPullBasketCoroutineRunning == false)
        {
            _isPullBasketCoroutineRunning = true;
            _pullBasketCoroutine = StartCoroutine(PullBasket());
        }
    }

    [ContextMenu("Down")]
    public void LowerBasket()
    {
        _isPullingUp = false;

        if (_isPullBasketCoroutineRunning == false)
        {
            _isPullBasketCoroutineRunning = true;
            _pullBasketCoroutine = StartCoroutine(PullBasket());
        }
    }

    private IEnumerator PullBasket()
    {
        while (true)
        {
            Debug.Log("PullBasket " + Time.time);

            if (_isPullingUp)
            {
                _blendValue += _evaluationSpeed * Time.deltaTime;

                LerpHandBetween(_basketStartPosition, _basketLiftDestination.position, _blendValue);

                HoldBasketWithHands();

                if (HasHandGottenToPosition(_basketLiftDestination.position))
                {
                    StopCoroutine(_pullBasketCoroutine);

                    _isPullBasketCoroutineRunning = false;
                }
            }
            else
            {
                _blendValue -= _evaluationSpeed * Time.deltaTime;

                LerpHandBetween(_basketStartPosition, _basketLiftDestination.position, _blendValue);

                HoldBasketWithHands();

                if (HasHandGottenToPosition(_basketStartPosition))
                {
                    StopCoroutine(_pullBasketCoroutine);

                    _isPullBasketCoroutineRunning = false;
                }
            }

            yield return null;
        }
    }

    private bool HasHandGottenToPosition(Vector3 where)
    {
        if (_ikTargetTransform.position == where)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void LerpHandBetween(Vector3 pos1, Vector3 pos2, float blendValue)
    {
        Vector3 calculatedPosition = Vector3.Lerp(pos1, pos2, blendValue);

        _ikTargetTransform.position = calculatedPosition;
    }

    private void HoldBasketWithHands()
    {
        _basketTransform.position = _ikTargetTransform.position;
    }
}
