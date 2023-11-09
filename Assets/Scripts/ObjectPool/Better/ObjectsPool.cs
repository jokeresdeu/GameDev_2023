using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Better
{
    public class ObjectsPool : MonoBehaviour
    {
        private readonly List<IPoolable> _freeObjects = new();
        private readonly List<IPoolable> _objectsInUse = new();

        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _bulletsToCreate;

        private void Start()
        {
            for (int i = 0; i < _bulletsToCreate; i++)
                _freeObjects.Add(Instantiate(_prefab, transform).GetComponent<IPoolable>());
        }

        public IPoolable GetBullet()
        {
            IPoolable freeObject;
            if (_freeObjects.Count > 1)
            {
                freeObject = _freeObjects[0];
                _freeObjects.RemoveAt(0);
            }
            else
            {
                freeObject = Instantiate(_prefab, transform).GetComponent<IPoolable>();
            }

            freeObject.ReturnRequested += OnReturnRequested;
            _objectsInUse.Add(freeObject);
            return freeObject;
        }

        public void ReturnAllBullets()
        {
            foreach(var usedObject in _objectsInUse)
                ReturnBullet(usedObject);
            _objectsInUse.Clear();
        }

        private void OnReturnRequested(IPoolable returnedObject)
        {
            _objectsInUse.Remove(returnedObject);
            ReturnBullet(returnedObject);
        }

        private void ReturnBullet(IPoolable returnedObject)
        {
            returnedObject.ReturnRequested -= OnReturnRequested;
            returnedObject.GameObject.SetActive(false);
            returnedObject.ResetPoolable();
            _freeObjects.Add(returnedObject);
        }
    }
}