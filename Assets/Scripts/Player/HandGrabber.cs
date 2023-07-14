using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    [Header("Hand Transform")]
    [SerializeField] private Transform _ikTargetTransform;
    [SerializeField] private Transform _ikHintTransform;

    [Header("Basket Transform")]
    [SerializeField] private Transform _basketTransform;

    public static event System.Action<PickableObject> TargetGotInHands;

    private PickableObject _target;

    private Vector3 _initialHandPosition;
    private Vector3 _lastTargetPosition;

    private HandState _currentHandState;
    private float _currentBlendValue;

    private enum HandState
    {
        Idle,
        HandToTarget,
        HandToBasket,
        HandToInitialPosition
    }

    private void Start()
    {
        _initialHandPosition = _ikTargetTransform.transform.position;
    }

    public void PutTargetToBasket(PickableObject target)
    {
        if (target != _target)
        {
            _target = target;
        }

        SetHandState(HandState.HandToTarget);

        StartCoroutine(PutTargetToBasketIEnumerator());
    }
    private void SetHandState(HandState handState)
    {
        _currentHandState = handState;
    }

    private IEnumerator PutTargetToBasketIEnumerator()
    {
        while (true)
        {
            Debug.Log(Time.time);
            switch (_currentHandState)
            {
                case HandState.HandToTarget:
                    {
                        PullHandToTarget();
                        break;
                    }
                case HandState.HandToBasket:
                    {
                        PullTargetToBasket();
                        break;
                    }
                case HandState.HandToInitialPosition:
                    {
                        PullHandToInitialPosition();
                        break;
                    }
                default:
                    break;
            }
            yield return null;
        }
    }

    private void PullHandToTarget()
    {
        LerpHandBetween(_initialHandPosition, _target.transform.position, _currentBlendValue);
        _currentBlendValue += Time.deltaTime;

        if (HasHandReachedDestination())
        {
            TargetGotInHands?.Invoke(_target);
            
            SetHandState(HandState.HandToBasket);

            _currentBlendValue = 0f;
            _lastTargetPosition = _ikTargetTransform.position;
        }
    }

    private void PullTargetToBasket()
    {
        LerpHandBetween(_lastTargetPosition, _basketTransform.position, _currentBlendValue);
        _currentBlendValue += Time.deltaTime;

        HoldTargetInHands();

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.HandToInitialPosition);
        }
    }

    private void PullHandToInitialPosition()
    {
        LerpHandBetween(_basketTransform.position, _initialHandPosition, _currentBlendValue);

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.Idle);

            StopCoroutine(PutTargetToBasketIEnumerator());
        }
    }

    private bool HasHandReachedDestination()
    {
        if (_currentBlendValue >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void LerpHandBetween(Vector3 start, Vector3 destination, float blendValue)
    {
        Vector3 lerpedPosition = Vector3.Lerp(start, destination, blendValue);
        
        _ikTargetTransform.transform.position = lerpedPosition;
        _ikHintTransform.transform.position = lerpedPosition;
    }

    private void HoldTargetInHands()
    {
        _target.transform.position = _ikTargetTransform.position; 
    }
}
