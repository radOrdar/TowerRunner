using System;
using Infrastructure;
using UnityEngine;

namespace Core.Loading
{
    public class LoadProgressOperation : ILoadingOperation
    {
        public string Description => "Loading progress..";
        public async Awaitable Load(Action<float> onProgress)
        {
            onProgress(0.15f);
            await Awaitable.WaitForSecondsAsync(0.5f);
            UserContainer userContainer = ProjectContext.I.UserContainer;
            userContainer.IsSubscriber = PlayerPrefs.GetInt("IsSubscriber") == 1;
            userContainer.Level = PlayerPrefs.GetInt("Level");
            onProgress(1f);
        }
    }
}
