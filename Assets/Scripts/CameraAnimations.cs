using Services;
using Services.Event;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    [SerializeField] private float normalFov;
    [SerializeField] private float accelFov;
    [SerializeField] private float finishFov;
    [SerializeField] private float fovSpeed;
    [SerializeField] private float rotationSpeed;

    private EventService _eventService;
    private Camera _mainCamera;
    
    private float _targetFov;
    private bool _rotating;

    private void Start()
    {
        _mainCamera = Camera.main;
        _eventService = AllServices.Instance.Get<EventService>();
        _eventService.HasteSwitch += enable => _targetFov = enable ? accelFov : normalFov;;
        _eventService.FinishPassed += () =>
        {
            _rotating = true;
            _targetFov = finishFov;
        };
    }

    private void Update()
    {
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _targetFov, fovSpeed * Time.deltaTime);

        if (_rotating)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}