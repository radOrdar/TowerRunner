using System;
using System.Collections.Generic;
using System.Linq;
using Obstacle;
using Services;
using Services.Audio;
using Services.Generator;
using StaticData;
using Tower.Components;
using Tower.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;

        private IAudioService _audioService;
        private void Awake()
        {
            _audioService = AllServices.Instance.Get<IAudioService>();
            _audioService.PlayMusic();
            
            TowerPattern towerPattern = AllServices.Instance.Get<ITowerGeneratorService>().GeneratePattern(levelData.towerLevels, levelData.numLedge);
            FindAnyObjectByType<TowerMove>().Init(towerPattern.towerProjections);
            FindAnyObjectByType<TowerBody>().Init(towerPattern.matrix);
            TowerCollision towerCollision = FindAnyObjectByType<TowerCollision>();
            towerCollision.Init(towerPattern.matrix);
            towerCollision.OnGateCollided += () => _audioService.PlayBump();
            towerCollision.OnGatePassed += () => _audioService.PlayDing();

            
            List<int[,]> gatePatterns = Enumerable.Range(0, levelData.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();

            FindAnyObjectByType<AllGates>().Init(gatePatterns, levelData.distanceBtwGates);
            
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