using System;
using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
        event Action<IPoolable> ReturnRequested;
        void ResetPoolable();
    }
}