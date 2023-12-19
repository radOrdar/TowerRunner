using System;
using System.Collections.Generic;
using System.Linq;
using Obstacle;
using Services;
using Services.Generator;
using StaticData;
using Tower;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Gates gates;
    [SerializeField] private TowerBody towerBody;
    [SerializeField] private TowerMove towerMove;
    [SerializeField] private TowerCollision towerCollision;
    
    private ITowerGeneratorService _towerGeneratorService;

    private void Awake()
    {
        _towerGeneratorService = AllServices.Instance.Get<ITowerGeneratorService>();
        
        TowerPattern towerPattern = _towerGeneratorService.GeneratePattern(levelData.towerLevels, levelData.numLedge);
        towerMove.Init(towerPattern.towerProjections);
        towerBody.Init(towerPattern.matrix);
        towerCollision.Init(towerPattern.matrix);
        
        List<int[,]> gatePatterns = Enumerable.Range(0, levelData.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();
      
        // for (int i = 0; i < levelData.numOfGates; i++)
        // {
        //     Vector3 direction = Direction();
        //     patterns.Add(towerPattern.gatePatterns[direction]);
        // }

        gates.Init(gatePatterns, levelData.distanceBtwGates);

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