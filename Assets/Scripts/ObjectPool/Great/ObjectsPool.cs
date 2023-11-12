using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Great
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
            {
                var createdObject = Instantiate(_prefab, transform).GetComponent<IPoolable>();
                createdObject.GameObject.SetActive(false);
                _freeObjects.Add(createdObject);
            }
        }

        private void OnValidate()
        {
            if (_prefab.GetComponent<IPoolable>() == null)
                _prefab = null;
        }

        public IPoolable GetObject()
        {
            IPoolable freeObject;
            if (_freeObjects.Count > 0)
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

        public void ReturnAllObject()
        {
            foreach(var usedObject in _objectsInUse)
                ReturnObject(usedObject);
            _objectsInUse.Clear();
        }

        private void OnReturnRequested(IPoolable returnedObject)
        {
            _objectsInUse.Remove(returnedObject);
            ReturnObject(returnedObject);
        }

        private void ReturnObject(IPoolable returnedObject)
        {
            returnedObject.ReturnRequested -= OnReturnRequested;
            returnedObject.GameObject.SetActive(false);
            returnedObject.ResetPoolable();
            _freeObjects.Add(returnedObject);
        }
    }
}