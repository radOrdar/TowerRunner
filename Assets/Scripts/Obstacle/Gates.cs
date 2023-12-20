using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class Gates : MonoBehaviour
    {
        [SerializeField] private ObstacleBlock obstaclePf;
        [SerializeField] private ObstacleFrame frameObstaclePf;

        public int[,] NextGatePattern => _gatePatterns[_currentGateIndex];
    
        private List<int[,]> _gatePatterns;
        private int _currentGateIndex;
        private float _timeLastCollided;

        public void Init(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            _gatePatterns = gatePatterns;
            SpawnGates(gatePatterns, distanceBtwObstacles);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time - _timeLastCollided < 0.5f) 
                return;
        
            if (_currentGateIndex < _gatePatterns.Count - 1)
                _currentGateIndex++;
            _timeLastCollided = Time.time;
        }

        private void SpawnGates(List<int[,]> gatePatterns, int distanceBtwObstacles)
        {
            int height = gatePatterns[0].GetLength(1);
            for (int nom = 0; nom < gatePatterns.Count; nom++)
            {
                int nextGateDistance = (nom + 1) * distanceBtwObstacles;
                BoxCollider gateCollider = gameObject.AddComponent<BoxCollider>();
                gateCollider.center = new Vector3(0, 1, nextGateDistance + 5);
                gateCollider.isTrigger = true;

                var gateParent = GenerateGateFrames(nextGateDistance, height);
                gateParent.parent = transform;

                int[,] matrix = gatePatterns[nom];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            Transform spawnObs = SpawnObs(gateParent);
                            spawnObs.localPosition = new Vector3(i, j, 0);
                        }
                    }
                }
            }

            Transform SpawnObs(Transform obstacleParent)
            {
                Transform obstacleBlock = Instantiate(obstaclePf).transform;
                obstacleBlock.parent = obstacleParent;
                return obstacleBlock;
            }
        }

        private Transform GenerateGateFrames(float distance, int height)
        {
            Transform obstacleParent = new GameObject("Obstacle").transform;
            obstacleParent.position = new Vector3(-2, 0, distance);
            var obstacleFrameLeft = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameLeft.localPosition = new Vector3(-1, 0, 0);
            obstacleFrameLeft.localScale = new Vector3(1, height, 1);
            var obstacleFrameRight = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameRight.localPosition = new Vector3(5, 0, 0);
            obstacleFrameRight.localScale = new Vector3(1, height, 1);
            var obstacleFrameTop = Instantiate(frameObstaclePf, obstacleParent).transform;
            obstacleFrameTop.up = obstacleParent.TransformDirection(Vector3.right);
            obstacleFrameTop.localPosition = new Vector3(-1.5f, height + .5f, 0);
            obstacleFrameTop.localScale = new Vector3(1, 7, 1);
            return obstacleParent;
        }
    }
}