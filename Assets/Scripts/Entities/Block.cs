using UnityEngine;

namespace Entities
{
    public class Block : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _hp;
        
        public void TakeDamage(int damage)
        {
            _hp -= damage;
            if (_hp <= 0)
                Destroy(gameObject);
        }
    }
}