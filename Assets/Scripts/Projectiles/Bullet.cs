using System;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Projectiles
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;

        private Tweener _moveTweener;

        public event Action<Bullet> ReturnRequested;

        public void MoveTo(Vector2 position)
        {
            var time = Mathf.Abs(position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(position.x, time);
            _moveTweener.OnComplete(() => ReturnRequested?.Invoke(this));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            col.GetComponent<IDamageable>().TakeDamage(_damage);
            ReturnRequested?.Invoke(this);
        }

        public void ResetBullet() => _moveTweener?.Kill();
    }
}