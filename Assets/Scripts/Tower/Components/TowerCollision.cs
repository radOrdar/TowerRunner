﻿using System;
using System.Collections.Generic;
using Obstacle;
using UnityEngine;

namespace Tower.Components
{
    public class TowerCollision : MonoBehaviour
    {
        public event Action OnGatePassed;
        public event Action OnGateCollided;
        
        private List<Collider> _colliders = new();

        private float _lastGatePassTime;

        public void Init(int[][,] matrix)
        {
            GenerateBoxColliders(matrix);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GatePass _))
            {
                if (Time.time - _lastGatePassTime > 0.5f)
                {
                    _lastGatePassTime = Time.time;
                    OnGatePassed?.Invoke();
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.TryGetComponent(out ObstacleBlock obstacleBlock))
            {
                obstacleBlock.OnCollided(Vector3.forward * 8 + Vector3.right * 3);
                foreach (var col in _colliders)
                {
                    Physics.IgnoreCollision(col, other.collider);
                }

                OnGateCollided?.Invoke();
            }
        }

        private void GenerateBoxColliders(int[][,] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix[i][j, k] == 1)
                        {
                            var boxCollider = gameObject.AddComponent<BoxCollider>();
                            boxCollider.center = new Vector3(j - 2, i + 0.5f, k - 2);
                            _colliders.Add(boxCollider);
                        }
                    }
                }
            }
        }

        private void GenerateMeshCollider(int[][,] towerPattern)
        {
            List<CombineInstance> combineInstances = new();

            for (int i = 0; i < towerPattern.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (towerPattern[i][j, k] == 1)
                        {
                            var matrix4X4 = new Matrix4x4(new Vector4(1, 0, 0, 0),
                                new Vector4(0, 1, 0, 0),
                                new Vector4(0, 0, 1, 0),
                                new Vector4(j - 2, i, k - 2, 1));

                            var combineInstance = new CombineInstance
                            {
                                mesh = CreateCubeMesh(),
                                transform = matrix4X4
                            };

                            combineInstances.Add(combineInstance);
                        }
                    }
                }
            }

            var towerMesh = new Mesh();
            towerMesh.CombineMeshes(combineInstances.ToArray());
            var meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = towerMesh;
            // meshCollider.convex = true;
            // meshCollider.isTrigger = true;
        }

        private Mesh CreateCubeMesh() =>
            new()
            {
                vertices = new Vector3[]
                {
                    new(0.5f, 0, -0.5f),
                    new(-0.5f, 0, -0.5f),
                    new(-0.5f, 0, 0.5f),
                    new(0.5f, 0, 0.5f),
                    new(0.5f, 1, -0.5f),
                    new(-0.5f, 1, -0.5f),
                    new(-0.5f, 1, 0.5f),
                    new(0.5f, 1, 0.5f),
                },
                triangles = new []
                {
                    0, 2, 1,
                    0, 3, 2,
                    0, 1, 5,
                    0, 5, 4,
                    2, 5, 1,
                    2, 6, 5,
                    3, 6, 2,
                    3, 7, 6,
                    0, 4, 7,
                    0, 7, 3,
                    4, 5, 6,
                    4, 6, 7
                }
            };
    }
}