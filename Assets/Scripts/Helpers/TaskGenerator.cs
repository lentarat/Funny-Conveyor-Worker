using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGenerator
{
    private int _randomPickableObjectsNumber;
    private string _randomPickableObjectTypeString;
    private PickableObject.ObjectsType _randomPickableObjectType;

    private int _minPickableObjectsRequired = 1;
    private int _maxPickableObjectsRequired = 7;

    public TaskGenerator()
    {
        Generate();
    }

    private void Generate()
    {
        System.Random random = new System.Random();

        _randomPickableObjectsNumber = random.Next(_minPickableObjectsRequired, _maxPickableObjectsRequired);

        int pickableObjectTypesLength = (int)PickableObject.ObjectsType.NumberOfTypes;
        _randomPickableObjectType = (PickableObject.ObjectsType)random.Next(0, pickableObjectTypesLength);
        _randomPickableObjectTypeString = _randomPickableObjectType.ToString();
    }

    public string GetTask()
    {
        return "Collect " + _randomPickableObjectsNumber + " " + _randomPickableObjectTypeString + "s";
    }

    public int GetRequiredPickableObjectsNumber()
    {
        return _randomPickableObjectsNumber;
    }

    public PickableObject.ObjectsType GetRequiredPickableObjectType()
    {
        return _randomPickableObjectType;
    }
}
