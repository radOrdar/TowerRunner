using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Loading
{
    public class LoadingScreenProvider
    {
        public async Awaitable Load(ILoadingOperation loadingOperation)
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(loadingOperation);
            await Load(operations);
        }

        public async Awaitable Load(Queue<ILoadingOperation> loadingOperations)
        {
            ResourceRequest resourceRequest = ProjectContext.I.AssetProvider.Load<LoadingScreen>(Constants.AssetPaths.LOADING_SCREEN);
            
            while (!resourceRequest.isDone)
            {
                await Awaitable.NextFrameAsync();
            }
            LoadingScreen loadingScreen = (LoadingScreen)resourceRequest.asset;
            SceneManager.MoveGameObjectToScene(loadingScreen.gameObject, SceneManager.GetSceneByName(Constants.Scenes.STARTUP));
            // await loadingScreen.Load(loadingOperations);
        }
    }
}
