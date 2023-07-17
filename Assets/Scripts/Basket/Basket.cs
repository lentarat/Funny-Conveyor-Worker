using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private int _basketSize;

    public static event System.Action OnPickableObjectWasAddedToBasket;

    private List<PickableObject> _pickableObjects = new();

    private void Start()
    {
        _basketSize = GameManager.Instance.TaskGenerator.GetRequiredPickableObjectsNumber();
    }

    public void AddPickableObjectToBasket(PickableObject pickableObject)
    {
        var _requiredPickableObjectType = GameManager.Instance.TaskGenerator.GetRequiredPickableObjectType();
        if (pickableObject.ObjectType != _requiredPickableObjectType)
        {
            GameManager.Instance.GameLost();
        }

        _pickableObjects.Add(pickableObject);

        OnPickableObjectWasAddedToBasket?.Invoke();

        if (IsBasketFull())
        {
            GameManager.Instance.LevelPassed();
        }
    }

    private bool IsBasketFull()
    {
        if (_pickableObjects.Count >= _basketSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}