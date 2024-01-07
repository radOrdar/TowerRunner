using System;
using System.Collections.Generic;
using System.Linq;
using Obstacle;
using Services;
using Services.Audio;
using Services.Event;
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
        [SerializeField] private Finish finishPf;

        private IAudioService _audioService;
        private EventService _eventService;
        
        private void Awake()
        {
            _audioService = AllServices.Instance.Get<IAudioService>();
            _eventService = AllServices.Instance.Get<EventService>();
            _audioService.PlayMusic();
            
            TowerPattern towerPattern = AllServices.Instance.Get<ITowerGeneratorService>().GeneratePattern(levelData.towerLevels, levelData.numLedge);
            FindAnyObjectByType<TowerMove>().Init(towerPattern.towerProjections);
            FindAnyObjectByType<TowerBody>().Init(towerPattern.matrix);
            TowerCollision towerCollision = FindAnyObjectByType<TowerCollision>();
            towerCollision.Init(towerPattern.matrix);
            _eventService.GateCollided += () => _audioService.PlayBump();
            _eventService.GatePassed += () => _audioService.PlayDing();
            _eventService.FinishPassed += () => _audioService.PlayFinish();
            
            List<int[,]> gatePatterns = Enumerable.Range(0, levelData.numOfGates).Select(_ => towerPattern.towerProjections[RandomDirection()]).ToList();

            Instantiate(finishPf, new Vector3(0, 0.1f, levelData.numOfGates * levelData.distanceBtwGates + 40), Quaternion.identity);
            FindAnyObjectByType<AllGates>().Init(gatePatterns, levelData.distanceBtwGates);
        }

        private Vector3 RandomDirection()
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