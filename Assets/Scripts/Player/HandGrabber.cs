using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    [Header("Hand Transform")]
    [SerializeField] private Transform _ikTarget;
    [SerializeField] private Transform _ikHint;

    [Header("Basket Transform")]
    [SerializeField] private Transform _basketTransform;

    private Vector3 _initialHandPosition;

    private enum HandState
    {
        HandToTarget,
        TargetToBasket,
        HandToInitialPosition
    }

    private void Start()
    {
        _initialHandPosition = _ikTarget.position;
    }

    public void PutPickableObjectToBasket(PickableObject pickableObject)
    {
        StartCoroutine(PutPickableObjectToBasketCoroutineIEnumerator(pickableObject));
    }
    private IEnumerator PutPickableObjectToBasketCoroutineIEnumerator(PickableObject pickableObject)
    {
        PullHandToTarget(pickableObject.transform);
        
        yield return null;
    }

    private void PullHandToTarget(Transform targetTransform)
    {   

    }

    private void PullTargetToBasket()
    {
        
    }

    private void PullHandToInitialPosition()
    {
    
    }
}
