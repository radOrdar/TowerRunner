using Tower.Components;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    [SerializeField] private float normalFov;
    [SerializeField] private float accelFov;
    [SerializeField] private float finishFov;
    [SerializeField] private float fovSpeed;
    [SerializeField] private float rotationSpeed;

    private float _targetFov;
    private Camera _mainCamera;
    private bool _rotating;

    private void Start()
    {
        _mainCamera = Camera.main;
        FindAnyObjectByType<TowerMove>().OnHasteSwitch += SetHaste;
        FindAnyObjectByType<TowerCollision>().OnFinishPassed += () =>
        {
            _rotating = true;
            _targetFov = finishFov;
        };
    }

    private void SetHaste(bool isEnabled) =>
        _targetFov = isEnabled ? accelFov : normalFov;

    private void Update()
    {
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _targetFov, fovSpeed * Time.deltaTime);

        if (_rotating)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}