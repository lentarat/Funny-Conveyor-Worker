using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private Transform _ikTargetTransform;
    [SerializeField] private HandsWithBasketHandler _handsWithBasketHandler;

    [Header("Basket Transform")]
    [SerializeField] private Transform _basketTransform;

    [Header("Trigger")]
    [SerializeField] private LayerMask _pickableObjectLayerMask;

    [Header("Common")]
    [SerializeField] private float _handSpeed;

    public static event System.Action<PickableObject> TargetGotInBasket;

    private PickableObject _currentTarget;

    private Vector3 _initialHandPosition;
    private Vector3 _realInitialHandPosition;
    private Vector3 _lastTargetPosition;
    private Vector3 _lastGrabbedDisappearedTargetPosition;

    private HandState _currentHandState;
    public HandState CurrentHandState => _currentHandState;

    private float _currentBlendValue;

    private Coroutine _putTargetToBasketCoroutine;
    private bool _putTargetToBasketCoroutineIsRunning;

    private bool _isTouchingTarget;
    
    public enum HandState
    {
        Idle,
        HandToTarget,
        HandToBasket,
        HandToInitialPosition,
        HandToInitialPositionFromDisappearedTarget
    }

    private void Start()
    {
        _realInitialHandPosition = _ikTargetTransform.transform.position;
        _initialHandPosition = _realInitialHandPosition;

        TriggerChecker.TriggerHitsSmth += TriggerTouchesTarget;
    }

    public void LoseCurrentTarget(PickableObject pickableObjectToLose)
    {
        if (_currentTarget != pickableObjectToLose)
            return;

        _lastGrabbedDisappearedTargetPosition = _currentTarget.transform.position;
        
        _currentTarget = null;

        _currentBlendValue = 0f;

        SetHandState(HandState.HandToInitialPositionFromDisappearedTarget);
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

        _currentTarget = target;        

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

            if (GameManager.Instance.CurrentGameState != GameManager.GameState.Active)
            {
                yield return null;
            }

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
                case HandState.HandToInitialPositionFromDisappearedTarget:
                    {
                        PullHandToInitialPosition(_lastGrabbedDisappearedTargetPosition);
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
        LerpHandBetween(_initialHandPosition, _currentTarget.transform.position, _currentBlendValue);
        AddBlendValue(_handSpeed * Time.deltaTime);

        if (HasHandReachedDestination() && _isTouchingTarget)
        {
            _isTouchingTarget = false;

            SetHandState(HandState.HandToBasket);

            _currentBlendValue = 0f;
            _lastTargetPosition = _ikTargetTransform.position;

            _handsWithBasketHandler.LiftBasket();
        }
    }

    private void PullTargetToBasket()
    {
        LerpHandBetween(_lastTargetPosition, _basketTransform.position, _currentBlendValue);
        AddBlendValue(_handSpeed * Time.deltaTime);

        HoldTargetInHands();

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.HandToInitialPosition);

            _currentBlendValue = 0f;
            
            _initialHandPosition = _realInitialHandPosition;

            _handsWithBasketHandler.LowerBasket();

            TargetGotInBasket?.Invoke(_currentTarget);
        }
    }

    private void PullHandToInitialPosition()
    {
        LerpHandBetween(_basketTransform.position, _initialHandPosition, _currentBlendValue);
        AddBlendValue(_handSpeed * Time.deltaTime);

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.Idle);

            _currentBlendValue = 0f;

            StopCoroutine(_putTargetToBasketCoroutine);
            _putTargetToBasketCoroutineIsRunning = false;
        }
    }

    private void PullHandToInitialPosition(Vector3 from)
    {
        LerpHandBetween(from, _initialHandPosition, _currentBlendValue);
        AddBlendValue(_handSpeed * Time.deltaTime);

        if (HasHandReachedDestination())
        {
            SetHandState(HandState.Idle);

            _currentBlendValue = 0f;

            StopCoroutine(_putTargetToBasketCoroutine);
            _putTargetToBasketCoroutineIsRunning = false;
        }
    }

    private void LerpHandBetween(Vector3 start, Vector3 destination, float blendValue)
    {
        Vector3 lerpedPosition = Vector3.Lerp(start, destination, blendValue);
        
        _ikTargetTransform.transform.position = lerpedPosition;
    }

    private void AddBlendValue(float value)
    {
        _currentBlendValue += value;
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

    private void HoldTargetInHands()
    {
        _currentTarget.transform.position = _ikTargetTransform.position; 
    }

    private void TriggerTouchesTarget()
    {
        _isTouchingTarget = true;
    }
}


