using System.Collections.Generic;
using Core;
using Core.Loading;
using UnityEngine;

namespace Infrastructure
{
    public class AppStartup : MonoBehaviour
    {
        private void Start()
        {
            ProjectContext.I.Initialize();

            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(ProjectContext.I.AssetProvider);
            loadingOperations.Enqueue(new LoadProgressOperation());

            ProjectContext.I.LoadingScreenProvider.Load(loadingOperations);
        }
    }
}
