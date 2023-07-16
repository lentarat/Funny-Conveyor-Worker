using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickableObjectsHandler : MonoBehaviour
{
    [Header("Storage")]
    [SerializeField] private Basket _basket;

    [Header("Pickable Objects")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _spawnedPickableObjectsConveyorParent;
    [SerializeField] private Transform _spawnedPickableObjectsBasketParent;
    [SerializeField] private Transform[] _pickableObjectsToSpawn;
    private List<PickableObject> _pickableObjects = new();

    [Header("Common")]
    [SerializeField, Min(0.05f)] private float _spawnTime;
    public float SpawnTime { set => _spawnTime = value; }

    private float _lastTimeObjectWasSpawned;


    private void Awake()
    {
        HandGrabber.TargetGotInBasket += ManageLists;
    }

    private void Start()
    {
        SpawnPickableObject();
    }

    private void Update()
    {
        if (HasSpawnTimePassed())
        {
            SpawnPickableObject();
            _lastTimeObjectWasSpawned = Time.time;
        }
    }

    private bool HasSpawnTimePassed()
    {
        if (Time.time > _lastTimeObjectWasSpawned + _spawnTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpawnPickableObject()
    {
        int randomObjectNumber = Random.Range(0, _pickableObjectsToSpawn.Length);

        Transform instatiatedPickableObjectTransform =
            Instantiate(_pickableObjectsToSpawn[randomObjectNumber].transform, _spawnPoint.transform.position, Quaternion.identity/*, _spawnedPickableObjectsParent*/);

        instatiatedPickableObjectTransform.parent = _spawnedPickableObjectsConveyorParent;

        PickableObject instatiatedPickableObject = instatiatedPickableObjectTransform.GetComponent<PickableObject>();

        _pickableObjects.Add(instatiatedPickableObject);
    }

    public void RemovePickableObject(PickableObject pickableObject)
    {
        pickableObject.gameObject.transform.parent = _spawnedPickableObjectsBasketParent;

        _pickableObjects.Remove(pickableObject);
    }

    public PickableObject GetPickableObject(int iter)
    {
        return _pickableObjects[iter];
    }

    public int GetPickableObjectsNumber()
    {
        return _pickableObjects.Count;
    }

    private void ManageLists(PickableObject pickableObject)
    {
        _basket.AddPickableObjectToBasket(pickableObject);
        RemovePickableObject(pickableObject);
    }
}