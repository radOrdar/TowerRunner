using System.Collections;
using System.Collections.Generic;
using Obstacle;
using Services;
using Services.Generator;
using Services.Input;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

namespace Tower
{
    public class TowerMove : MonoBehaviour
    {
        [SerializeField] private TowerEffects _towerEffects;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Gates _gates;
        [SerializeField] private TowerCameraAnimations _cameraAnimations;

        [SerializeField] private float moveSpeed = 15;
        [SerializeField] private float hasteMoveSpeed = 60;
        [SerializeField] private float bounceBackSpeed = 3;
        [SerializeField] private float acceleration = 30;
        [SerializeField] private float bounceAcceleration = 3;
        [SerializeField] private float bounceAccelerationDelay = 1.5f;
        [SerializeField] private float manualRotationSpeed = 3;
        [SerializeField] private float autoRotateSpeed = 90;

        private Vector3[] _directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back, };
        private Dictionary<Vector3, int[,]> _towerProjections;

        private IInputService _inputService;

        private Vector3 _prevMousePos;
        private Quaternion _targetRotation;
        private float _targetSpeed;
        private float _currentSpeed;
        private float _currentAcceleration;
        private bool _slowedDown;

        public void Init(Dictionary<Vector3, int[,]> towerProjections)
        {
            _towerProjections = towerProjections;
        }

        private void Start()
        {
            _inputService = AllServices.Instance.Get<IInputService>();
            _targetSpeed = moveSpeed;
            _currentAcceleration = acceleration;
            StartCoroutine(CheckGateForm());
        }

        private IEnumerator CheckGateForm()
        {
            while (true)
            {
                if (!_slowedDown)
                {
                    if (_towerProjections.TryGetValue(_bodyTransform.forward, out int[,] proj) && EqualityCheck(proj, _gates.NextGatePattern))
                    {
                        _targetSpeed = hasteMoveSpeed;
                        _cameraAnimations.SetHaste(true);
                        _towerEffects.SetEnabledSpeedFx(true);
                    } else
                    {
                        _targetSpeed = moveSpeed;
                        _cameraAnimations.SetHaste(false);
                        _towerEffects.SetEnabledSpeedFx(false);
                    }
                }

                yield return new WaitForSeconds(.1f);
            }
        }

        private bool EqualityCheck(int[,] m1, int[,] m2)
        {
            if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
                return false;

            int rows = m1.GetLength(0);
            int cols = m2.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (m1[i, j] != m2[i, j])
                        return false;
                }
            }

            return true;
        }

        // Update is called once per frame
        void Update()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _currentAcceleration * Time.deltaTime);
            transform.position += Vector3.forward * (_currentSpeed * Time.deltaTime);

            if (_inputService.GetMouseButtonDown(0))
            {
                _prevMousePos = _inputService.MousePosition;
            }

            if (_inputService.GetMouseButton(0))
            {
                Vector3 mousePosition = _inputService.MousePosition;
                _bodyTransform.Rotate(Vector3.up, (mousePosition - _prevMousePos).x * manualRotationSpeed, Space.World);
                _prevMousePos = mousePosition;
            } else
            {
                _bodyTransform.rotation = Quaternion.RotateTowards(_bodyTransform.rotation, _targetRotation, autoRotateSpeed * Time.deltaTime);
            }

            if (_inputService.GetMouseButtonUp(0))
            {
                Vector3 currentDir = _bodyTransform.forward;
                Vector3 max = _directions[0];
                float maxDot = Vector3.Dot(currentDir, max);
                for (int i = 1; i < _directions.Length; i++)
                {
                    float dot = Vector3.Dot(currentDir, _directions[i]);
                    if (dot > maxDot)
                    {
                        maxDot = dot;
                        max = _directions[i];
                    }
                }

                _targetRotation = Quaternion.LookRotation(max, Vector3.up);
            }
        }

        public void BounceBack()
        {
            _currentSpeed = -bounceBackSpeed;
            StartCoroutine(BounceAccelerationRoutine());
        }

        private IEnumerator BounceAccelerationRoutine()
        {
            _currentAcceleration = bounceAcceleration;
            _slowedDown = true;
            _cameraAnimations.SetHaste(false);
            _towerEffects.SetEnabledSpeedFx(false);
            yield return new WaitForSeconds(bounceAccelerationDelay);
            _currentAcceleration = acceleration;
            _slowedDown = false;
        }
    }
}