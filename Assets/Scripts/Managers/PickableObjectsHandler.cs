using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickableObjectsHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _spawnedPickableObjectsParent;
    [SerializeField] private PickableObject[] _pickableObjectsToSpawn;
    private List<PickableObject> _pickableObjects = new();

    [Header("Common")]
    [SerializeField, Min(0.05f)] private float _spawnTime;
    public float SpawnTime { set => _spawnTime = value; }

    private float _lastTimeObjectWasSpawned;

    private void Update()
    {
        if (HasSpawnTimePassed())
        {
            SpawnPickableObjects();
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

    private void SpawnPickableObjects()
    {
        int randomObjectNumber = Random.Range(0, _pickableObjectsToSpawn.Length);
        Transform instatiatedPickableObjectTransform = Instantiate(_pickableObjectsToSpawn[randomObjectNumber].transform, _spawnPoint.position, Quaternion.identity, _spawnedPickableObjectsParent);
        PickableObject instatiatedPickableObject = instatiatedPickableObjectTransform.GetComponent<PickableObject>();
        _pickableObjects.Add(instatiatedPickableObject);
    }

    public void RemovePickableObject(PickableObject pickableObject)
    {
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
}




//public class PickableObjectsHandler : MonoBehaviour
//{
//    [Header("Objects")]
//    [SerializeField] private Transform _spawnPoint;
//    [SerializeField] private Transform _spawnedObjectsParent;
//    [SerializeField] private Transform[] _objectsToSpawn;
//    private List<Transform> _spawnedObjects = new();

//    [Header("Common")]
//    [SerializeField, Min(0.05f)] private float _spawnTime;
//    public float SpawnTime { set => _spawnTime = value; }

//    private float _lastTimeObjectWasSpawned;

//    private void Update()
//    {
//        if (HasSpawnTimePassed())
//        {
//            SpawnPickableObjects();
//            _lastTimeObjectWasSpawned = Time.time;
//        }
//    }

//    private bool HasSpawnTimePassed()
//    {
//        if (Time.time > _lastTimeObjectWasSpawned + _spawnTime)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    private void SpawnPickableObjects()
//    {
//        int randomObjectNumber = Random.Range(0, _objectsToSpawn.Length);
//        Transform instatiatedObject = Instantiate(_objectsToSpawn[randomObjectNumber], _spawnPoint.position, Quaternion.identity, _spawnedObjectsParent);
//        _spawnedObjects.Add(instatiatedObject);
//    }

//    public void RemovePickableObject(Transform obj)
//    {
//        _spawnedObjects.Remove(obj);
//    }

//    public Transform GetPickableObject(int iter)
//    {
//        return _spawnedObjects[iter];
//    }

//    public int GetPickableObjectsNumber()
//    {
//        return _spawnedObjects.Count;
//    }
//}
