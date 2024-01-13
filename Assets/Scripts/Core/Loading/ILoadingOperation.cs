using System;
using UnityEngine;

namespace Core.Loading
{
    public interface ILoadingOperation
    {
        string Description { get; }
        
        Awaitable Load(Action<float> onProgress);
    }
}