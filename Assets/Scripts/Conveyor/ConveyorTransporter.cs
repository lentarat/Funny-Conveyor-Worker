using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTransporter : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private PickableObjectsHandler _pickableObjectsHandler;

    [Header("Path")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private float _destinationCatchRadius;

    [Header("Conveyor Properties")]
    [SerializeField] private float _speed;
 
    private bool _isWorking = true;

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
                DestroyPickableObject(pickableObjectToTransport);
            }
        }
    }

    private void DestroyPickableObject(PickableObject pickableObjectToDestroy)
    {
        _pickableObjectsHandler.RemovePickableObject(pickableObjectToDestroy);
        Destroy(pickableObjectToDestroy.gameObject);
    }
}