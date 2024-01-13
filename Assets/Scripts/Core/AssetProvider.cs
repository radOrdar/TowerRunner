using System;
using Core.Loading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class AssetProvider : ILoadingOperation
    {
        private bool _isReady;
        
        public ResourceRequest Load<T>(string assetPath) where T : Object
        {
            return Resources.LoadAsync<T>(assetPath);
        }

        public string Description => "Assets Initialization...";
        public async Awaitable Load(Action<float> onProgress)
        {
            //Initialize addressables?
            onProgress(0.15f);
            await Awaitable.WaitForSecondsAsync(1f);
            _isReady = true;
        }
    }
}
