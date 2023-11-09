using System;
using DG.Tweening;
using Entities;
using ObjectPool;
using UnityEngine;

namespace Projectiles.Prefect
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;

        private Tweener _moveTweener;

        public GameObject GameObject => gameObject;
        
        public event Action<IPoolable> ReturnRequested;

        public void MoveTo(Vector2 position)
        {
            var time = Mathf.Abs(position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(position.x, time);
            _moveTweener.OnComplete(() => ReturnRequested?.Invoke(this));
        }
        
        public void ResetPoolable() => _moveTweener?.Kill();

        private void OnTriggerEnter2D(Collider2D col)
        {
            col.GetComponent<IDamageable>().TakeDamage(_damage);
            ReturnRequested?.Invoke(this);
        }
    }
}