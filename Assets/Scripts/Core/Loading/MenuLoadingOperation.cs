using System;
using Core.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoadingOperation : ILoadingOperation
{
    public string Description => "Main menu loading...";
    public async Awaitable Load(Action<float> onProgress)
    {
        onProgress?.Invoke(0.5f);
        await SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
        onProgress?.Invoke(1f);
    }
}
