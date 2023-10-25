using DG.Tweening;
using Entities.Good;
using UnityEngine;

namespace Entities
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private int _damage;

        private Tweener _moveTweener;

        public void MoveTo(Vector2 position)
        {
            var time = Mathf.Abs(position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(position.x, time);
            _moveTweener.OnComplete(() => Destroy(gameObject));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            col.GetComponent<WalkerEntity>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}