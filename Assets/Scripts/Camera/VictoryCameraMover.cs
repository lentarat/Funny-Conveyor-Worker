using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCameraMover : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform _camera;

    [Header("Path Position")]
    [SerializeField] private Transform _cameraPathStartTransform;

    [Header("Player Position")]
    [SerializeField] private Transform _playerTransform;

    [Header("Common")]
    [SerializeField] private float _cameraFlyDuration;

    private float _timeWhenStartedFlying;
    private Transform _cameraPathEndTransform;

    private void Start()
    {
        GameManager.Instance.OnLevelPassed += MoveCamera;

        _cameraPathStartTransform.position = transform.position;
    }
    private void MoveCamera()
    {
        StartCoroutine(MoveCameraIEnumerator());
    }

    private IEnumerator MoveCameraIEnumerator()
    {
        while (_timeWhenStartedFlying + _cameraFlyDuration > Time.time)
        {
            LerpCamera();

            yield return null;
        }
    }

    private void LerpCamera()
    {
        float blendValue = (Time.time - _timeWhenStartedFlying) / _cameraFlyDuration;

        Vector3 calculatedPosition = Vector3.Lerp(_cameraPathStartTransform.position, _cameraPathEndTransform.position, blendValue);

        _camera.position = calculatedPosition;
    }

    private void WatchTarget(Vector3 position)
    {
        _camera.LookAt(_playerTransform.position);
    }
}
