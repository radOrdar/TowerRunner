using UnityEngine;

public class TowerCameraAnimations : MonoBehaviour
{
    [SerializeField] private float normalFov;
    [SerializeField] private float accelFov;
    [SerializeField] private float fovSpeed;

    private float _targetFov;
    private Camera _mainCamera;

    private void Start() => 
        _mainCamera = Camera.main;

    public void SetHaste(bool isEnabled) =>
        _targetFov = isEnabled ? accelFov : normalFov;

    private void Update() => 
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _targetFov, fovSpeed * Time.deltaTime);
}