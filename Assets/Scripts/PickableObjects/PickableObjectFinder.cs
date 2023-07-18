using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickableObjectFinder : MonoBehaviour
{
    [Header("Custom Classes")]
    [SerializeField] private HandGrabber _handGrabber;

    [Header("Other")]
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _pickableObjectsLayerMask;

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

        PickableObject pickableObject = GetPickableObjectWithRayCast();

        if (pickableObject != null)
        {
            HandlePuttingPickableObjectToBasket(pickableObject);
        }
    }

    private PickableObject GetPickableObjectWithRayCast()
    {
        RaycastHit raycastHit;
        if(Physics.Raycast(_rayToTouchPoint, out raycastHit, Mathf.Infinity, _pickableObjectsLayerMask))
        {
            return raycastHit.collider.GetComponent<PickableObject>();
        }

        return null;
    }

    private void HandlePuttingPickableObjectToBasket(PickableObject pickableObject)
    {
        if (_handGrabber.CurrentHandState != HandGrabber.HandState.HandToBasket)
        {
            _handGrabber.PutTargetToBasket(pickableObject);
        }
    }

}
