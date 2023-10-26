using DG.Tweening;
using Entities.Bad;
using UnityEngine;

namespace Entities
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;

        private Tweener _moveTweener;

        public void MoveTo(Vector2 position)
        {
            var time = Mathf.Abs(position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(position.x, time);
            _moveTweener.OnComplete(() => Destroy(gameObject));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out Walker walker))
                walker.TakeDamage(_damage);
            
            Destroy(gameObject);
        }
    }
}