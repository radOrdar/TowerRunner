using System;
using Core;
using Core.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadingOperation : ILoadingOperation
{
    public string Description => "Loading Game..";
    public async UniTask Load(Action<float> onProgress)
    {
        onProgress(0.1f);

        await SceneManager.UnloadSceneAsync(Constants.Scenes.MAIN_MENU);
        onProgress(0.7f);
        await SceneManager.LoadSceneAsync(Constants.Scenes.GAME, LoadSceneMode.Additive);
    }
}
