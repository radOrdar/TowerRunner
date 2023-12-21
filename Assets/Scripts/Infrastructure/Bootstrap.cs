using System;
using Services;
using Services.Generator;
using Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
   public class Bootstrap : MonoBehaviour
   {
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
