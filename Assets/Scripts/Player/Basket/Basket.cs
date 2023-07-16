using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private int _basketSize;

    public event System.Action PickableObjectWasAddedToBasket;
    public event System.Action BasketIsFull;

    private List<PickableObject> _pickableObjects = new();

    public void AddPickableObjectToBasket(PickableObject pickableObject)
    {
        _pickableObjects.Add(pickableObject);
        PickableObjectWasAddedToBasket?.Invoke();

        if (IsBasketFull())
        {
            BasketIsFull?.Invoke();   
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