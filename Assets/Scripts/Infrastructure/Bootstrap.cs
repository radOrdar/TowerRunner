using System;
using Services;
using Services.Audio;
using Services.Generator;
using Services.Input;
using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
   public class Bootstrap : MonoBehaviour
   {
      [SerializeField] private SoundsData soundsData;
      
      private void Awake()
      {
         DontDestroyOnLoad(this);
         RegisterServices();
         LoadGame();
      }

      private void RegisterServices()
      {
         AllServices.Instance.Register<ITowerGeneratorService>(new TowerGeneratorService());
         AllServices.Instance.Register<IInputService>(new OldInputService());
         
         var audioService = new GameObject("AudioService").AddComponent<AudioService>();
         audioService.Init(soundsData);
         DontDestroyOnLoad(audioService);
         AllServices.Instance.Register<IAudioService>(audioService);
      }

      private void LoadGame()
      {
         SceneManager.LoadScene("Game");
      }

      private void OnDestroy()
      {
         AllServices.Instance.Dispose();
      }
   }
}
