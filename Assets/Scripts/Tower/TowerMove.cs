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
        [SerializeField] private Gates _gates;
        [SerializeField] private CameraAnimations _cameraAnimations;

        [SerializeField] private float moveSpeed = 15;
        [SerializeField] private float hasteMoveSpeed = 60;
        [SerializeField] private float bounceBackSpeed = 3;
        [SerializeField] private float acceleration = 30;
        // [SerializeField] private float hasteaAcceleration = 40;
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
                    Vector3 forward = transform.forward;
                    if (_towerProjections.TryGetValue(transform.forward, out int[,] proj))
                    {
                        if (EqualityCheck(proj, _gates.NextGatePattern))
                        {
                            _targetSpeed = hasteMoveSpeed;
                            _cameraAnimations.SetHaste(true);
                        } else
                        {
                            _targetSpeed = moveSpeed;
                            _cameraAnimations.SetHaste(false);
                        }
                    } else
                    {
                        _targetSpeed = moveSpeed;
                        _cameraAnimations.SetHaste(false);
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
                transform.Rotate(Vector3.up, (mousePosition - _prevMousePos).x * manualRotationSpeed, Space.World);
                _prevMousePos = mousePosition;
            } else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, autoRotateSpeed * Time.deltaTime);
            }

            if (_inputService.GetMouseButtonUp(0))
            {
                Vector3 currentDir = transform.forward;
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
            yield return new WaitForSeconds(bounceAccelerationDelay);
            _currentAcceleration = acceleration;
            _slowedDown = false;
        }

        public void Init(Dictionary<Vector3, int[,]> towerProjections)
        {
            _towerProjections = towerProjections;
        }
    }
}