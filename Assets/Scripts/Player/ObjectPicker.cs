using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPicker : MonoBehaviour
{
    [Header("Custom Classes")]
    [SerializeField] private Basket _basket;
    [SerializeField] private PickableObjectsHandler _pickableObjectsHandler;

    [Header("Other")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _layerMask;

    private PlayerInputActions _playerInputActions;

    private Vector2 _mousePosition;
    private Ray _rayToTouchPoint;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        EnableActions(true);
    }

    private void OnDisable()
    {
        EnableActions(false);
    }

    private void EnableActions(bool state)
    {
        if (state == true)
        {
            _playerInputActions.Player.TouchPosition.performed += DoTouchPosition;
            _playerInputActions.Player.TouchPosition.Enable();
        }
        else
        {
            _playerInputActions.Player.TouchPosition.Disable();
            _playerInputActions.Player.TouchPosition.performed += DoTouchPosition;
        }
    }

    private void DoTouchPosition(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
        _rayToTouchPoint = _camera.ScreenPointToRay(_mousePosition);

        PickableObject pickableObject = GetPickableObject();

        if (pickableObject != null)
        {
            _basket.AddPickableObjectToBasket(pickableObject);
            _pickableObjectsHandler.RemovePickableObject(pickableObject);

            _basket.TeleportPickableObjectToBasket(pickableObject.transform);
        }
    }

    private PickableObject GetPickableObject()
    {
        RaycastHit raycastHit;
        if(Physics.Raycast(_rayToTouchPoint, out raycastHit, Mathf.Infinity, _layerMask))
        {
            return raycastHit.collider.GetComponent<PickableObject>();
        }

        return null;
    }    
}
