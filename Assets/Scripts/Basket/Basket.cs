using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private int _basketSize;

    public static event System.Action OnPickableObjectWasAddedToBasket;
    //public static event System.Action BasketIsFull;

    private List<PickableObject> _pickableObjects = new();

    private PickableObject.ObjectsType _requiredPickableObjectType;

    private void Start()
    {
        _basketSize = GameManager.Instance.TaskGenerator.GetRequiredPickableObjectsNumber();
        _requiredPickableObjectType = GameManager.Instance.TaskGenerator.GetRequiredPickableObjectType();
    }

    public void AddPickableObjectToBasket(PickableObject pickableObject)
    {
        if (pickableObject.ObjectType != _requiredPickableObjectType)
        {
            GameManager.Instance.GameLost();
        }

        _pickableObjects.Add(pickableObject);

        OnPickableObjectWasAddedToBasket?.Invoke();

        if (IsBasketFull())
        {
            //BasketIsFull?.Invoke();
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