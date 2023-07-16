using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    [Header("Hand Transform")]
    [SerializeField] private Transform _ikTargetTransform;

    [Header("Basket Transform")]
    [SerializeField] private Transform _basketTransform;

    [Header("Trigger")]
    [SerializeField] private LayerMask _pickableObjectLayerMask;

    public static event System.Action<PickableObject> TargetGotInHands;

    private PickableObject _target;

    private Vector3 _initialHandPosition;
    private Vector3 _realInitialHandPosition;
    private Vector3 _lastTargetPosition;

    private HandState _currentHandState;
    private float _currentBlendValue;

    private Coroutine _putTargetToBasketCoroutine;
    private bool _putTargetToBasketCoroutineIsRunning;

    private bool _isTouchingTarget;

    private enum HandState
    {
        Idle,
        HandToTarget,
        HandToBasket,
        HandToInitialPosition
    }

    private void Start()
    {
        _realInitialHandPosition = _ikTargetTransform.transform.position;
        _initialHandPosition = _realInitialHandPosition;

        TriggerChecker.TriggerHitsSmth += TriggerTouchesTarget;
    }

    public void PutTargetToBasket(PickableObject target)
    {
        if (_currentHandState == HandState.HandToBasket)
        {
            return;
        }
        else if (_currentHandState == HandState.HandToTarget || _currentHandState == HandState.HandToInitialPosition)
        {
            _initialHandPosition = _ikTargetTransform.transform.position;

            _currentBlendValue = 0f;
        }

        _target = target;        

        SetHandState(HandState.HandToTarget);

        if (_putTargetToBasketCoroutineIsRunning == false)
        {
            _putTargetToBasketCoroutine = StartCoroutine(PutTargetToBasketIEnumerator());
            _putTargetToBasketCoroutineIsRunning = true;
        }
    }
    private void SetHandState(HandState handState)
    {
        _currentHandState = handState;
    }

    private IEnumerator PutTargetToBasketIEnumerator()
    {
        while (true)
        {
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

        if (HasHandReachedDestination() && _isTouchingTarget)
        {
            _isTouchingTarget = false;

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

            _currentBlendValue = 0f;

            _initialHandPosition = _realInitialHandPosition;
        }
    }

    private void PullHandToInitialPosition()
    {
        LerpHandBetween(_basketTransform.position, _initialHandPosition, _currentBlendValue);
        _currentBlendValue += Time.deltaTime;

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.Idle);

            _currentBlendValue = 0f;

            StopCoroutine(_putTargetToBasketCoroutine);
            _putTargetToBasketCoroutineIsRunning = false;
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
    }

    private void HoldTargetInHands()
    {
        _target.transform.position = _ikTargetTransform.position; 
    }

    private void TriggerTouchesTarget()
    {
        _isTouchingTarget = true;
    }
}
