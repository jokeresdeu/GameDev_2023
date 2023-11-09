using System;
using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
        event Action<IPoolable> ReturnRequested;
        void ResetPoolable();
    }
}