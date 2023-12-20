using System;
using System.Collections.Generic;
using System.Linq;
using Obstacle;
using Services;
using Services.Generator;
using StaticData;
using Tower.Components;
using Tower.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        private Gates _gates;
        private TowerBody _towerBody;
        private TowerMove _towerMove;
        private TowerCollision _towerCollision;
    
        private ITowerGeneratorService _towerGeneratorService;

        private void Awake()
        {
            _gates = FindAnyObjectByType<Gates>();
            _towerBody = FindAnyObjectByType<TowerBody>();
            _towerMove = FindAnyObjectByType<TowerMove>();
            _towerCollision = FindAnyObjectByType<TowerCollision>();
            _towerGeneratorService = AllServices.Instance.Get<ITowerGeneratorService>();
        
            TowerPattern towerPattern = _towerGeneratorService.GeneratePattern(levelData.towerLevels, levelData.numLedge);
            _towerMove.Init(towerPattern.towerProjections);
            _towerBody.Init(towerPattern.matrix);
            _towerCollision.Init(towerPattern.matrix);
        
            List<int[,]> gatePatterns = Enumerable.Range(0, levelData.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();

            _gates.Init(gatePatterns, levelData.distanceBtwGates);

            Vector3 RandomDirection()
            {
                return Random.Range(0, 4) switch
                {
                    0 => Vector3.forward,
                    1 => Vector3.back,
                    2 => Vector3.right,
                    3 => Vector3.left,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    
    }
}