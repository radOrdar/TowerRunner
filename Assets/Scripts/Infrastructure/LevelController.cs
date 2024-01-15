﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Audio;
using Obstacle;
using StaticData;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI scoreText;

        private AudioProvider _audioProvider;
        private EventsProvider _eventsProvider;

        private void Awake()
        {
            _audioProvider = ProjectContext.I.AudioProvider;
            _eventsProvider = ProjectContext.I.EventsProvider;
            _audioProvider.PlayMusic();
            
            scoreText.SetText($"Level {ProjectContext.I.UserContainer.Level + 1}");

            TowerPattern towerPattern = new TowerGenerator().GeneratePattern(levelData.towerLevels, levelData.numLedge);
            FindAnyObjectByType<TowerMove>().Init(towerPattern.towerProjections);
            FindAnyObjectByType<TowerBody>().Init(towerPattern.matrix);
            TowerCollision towerCollision = FindAnyObjectByType<TowerCollision>();
            towerCollision.Init(towerPattern.matrix);
            _eventsProvider.GateCollided += () => _audioProvider.PlayBump();
            _eventsProvider.GatePassed += () => _audioProvider.PlayDing();
            _eventsProvider.FinishPassed += () => _audioProvider.PlayFinish();

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