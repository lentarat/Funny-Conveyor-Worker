using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTransporter : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] private PickableObjectsHandler _pickableObjectsHandler;
    [SerializeField] private HandGrabber _handGrabber;

    [Header("Path")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private float _destinationCatchRadius;

    [Header("Conveyor Properties")]
    [SerializeField] private float _speed;
    [SerializeField, Range(0f, 1f)] private float _deceleration;
 
    private bool _isWorking = true;

    private void Start()
    {
        GameManager.Instance.OnLevelPassed += DecelerateConveyor;
        GameManager.Instance.OnGameLost += DecelerateConveyor;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelPassed -= DecelerateConveyor;
        GameManager.Instance.OnGameLost -= DecelerateConveyor;
    }

    private void Update()
    {
        if (_isWorking)
        {
            TransportObjects();
        }
    }

    private void TransportObjects()
    {
        for(int i = 0; i < _pickableObjectsHandler.GetPickableObjectsNumber(); i++)
        {
            PickableObject pickableObjectToTransport = _pickableObjectsHandler.GetPickableObject(i);

            if (Vector3.Distance(pickableObjectToTransport.transform.position, _destinationPoint.position) > _destinationCatchRadius)
            {
                Vector3 directionToDestination = (_destinationPoint.position - pickableObjectToTransport.transform.position).normalized;
                pickableObjectToTransport.transform.Translate(directionToDestination * _speed * Time.deltaTime);
            }
            else
            {
                if (HasLostRequiredPickableObject(pickableObjectToTransport))
                {
                    GameManager.Instance.GameLost();
                }
                _handGrabber.LoseCurrentTarget(pickableObjectToTransport);
                DestroyPickableObject(pickableObjectToTransport);
            }
        }
    }

    private bool HasLostRequiredPickableObject(PickableObject pickableObjectToDestroy)
    {
        PickableObject.ObjectsType requiredObjectType = GameManager.Instance.TaskGenerator.GetRequiredPickableObjectType();

        if (pickableObjectToDestroy.ObjectType == requiredObjectType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DestroyPickableObject(PickableObject pickableObjectToDestroy)
    {
        _pickableObjectsHandler.RemovePickableObject(pickableObjectToDestroy);
        Destroy(pickableObjectToDestroy.gameObject);
    }

    private void DecelerateConveyor()
    {
        StartCoroutine(DecelerateConveyorIEnumerator());
    }

    private IEnumerator DecelerateConveyorIEnumerator()
    {
        while(_speed > 0.02f)
        {
            _speed *= _deceleration * (1 - Time.deltaTime);
            
            yield return null;
        }
        _speed = 0f;

        yield return null;
    }
}   