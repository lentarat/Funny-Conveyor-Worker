using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCameraMover : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform _camera;

    [Header("Path Position")]
    [SerializeField] private Transform _cameraPathEndTransform;

    [Header("Player Position")]
    [SerializeField] private Transform _playerTransform;

    [Header("Common")]
    [SerializeField] private float _cameraFlyDuration;

    private float _timeWhenStartedFlying;
    private Vector3 _cameraPathStartPosition;

    private void Start()
    {
        GameManager.Instance.OnLevelPassed += MoveCamera;

        _cameraPathStartPosition = transform.position;
    }
    [ContextMenu("go")]
    private void MoveCamera()
    {
        _timeWhenStartedFlying = Time.time;

        StartCoroutine(MoveCameraIEnumerator());
    }

    private IEnumerator MoveCameraIEnumerator()
    {
        while (_timeWhenStartedFlying + _cameraFlyDuration > Time.time)
        {
            LerpCamera();

            WatchPlayer();

            yield return null;
        }
    }

    private void LerpCamera()
    {
        float blendValue = (Time.time - _timeWhenStartedFlying) / _cameraFlyDuration;

        Vector3 calculatedPosition = Vector3.Lerp(_cameraPathStartPosition, _cameraPathEndTransform.position, blendValue);

        Debug.Log(blendValue + " " + calculatedPosition);

        _camera.position = calculatedPosition;
    }

    private void WatchPlayer()
    {
        _camera.LookAt(_playerTransform.position);
    }
}
