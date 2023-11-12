﻿using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Perfect
{
    public class ObjectsPool 
    {
        private static ObjectsPool _instance;
        public static ObjectsPool Instance => _instance ??= new ObjectsPool();
        
        private readonly Dictionary<GameObject, PoolTask> _activePoolTasks;
        private readonly Transform _objectPoolTransform;

        private ObjectsPool()
        {
            _activePoolTasks = new Dictionary<GameObject, PoolTask>();
            _objectPoolTransform = new GameObject().transform;
        }
        
        public T GetObject<T>(T prefab) where T : MonoBehaviour, IPoolable 
        {
            if (!_activePoolTasks.TryGetValue(prefab.GameObject, out var poolTask))
            {
                AddTaskToPool(prefab, out poolTask);
            }

            return poolTask.GetFreeObject(prefab);
        }

        private void AddTaskToPool<T>(T prefab, out PoolTask poolTask) where T : MonoBehaviour, IPoolable
        {
            GameObject container = new GameObject
            {
                name = $"{prefab.name}s_pool"
            };
            container.transform.SetParent((_objectPoolTransform));
            poolTask = new PoolTask(container.transform);
            _activePoolTasks.Add(prefab.GameObject, poolTask);
        }
        
        public void DisposeTask()
        {
            foreach (var poolTask in _activePoolTasks.Values)
            {
                poolTask.ClearPool();
            }
        }
    }
}