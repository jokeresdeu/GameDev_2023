using System.Collections;
using Animation;
using ObjectPool;
using Projectiles;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities
{
    public class ShooterEntity : BaseEntity
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private LayerMask _target;
        
        [SerializeField, SpineEvent] private string _attackEvent;

        private Vector2 _endReach;
        
        public void Initialize(Vector2 endReach)
        {
            _endReach = new Vector2(endReach.x, ActionPoint.position.y);
            Initialize();
        }

        protected override IEnumerator ActionCoroutine()
        {
            while (true)
            {
                SetAnimationState(AnimationType.Action, EnemyInReach(), true);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        protected override void OnAnimationEvent(TrackEntry _, Event e)
        {
            if(e.Data.Name == _attackEvent)
                Shoot();
        }
        
        private void Shoot()
        {
            Bullet bullet = ObjectsPool.Instance.GetObject(_bulletPrefab);
            bullet.transform.position = ActionPoint.position;
            bullet.gameObject.SetActive(true);
            bullet.MoveTo(_endReach);
        }
        
        private bool EnemyInReach() => 
            Physics2D.OverlapArea(ActionPoint.position, _endReach, _target);
    }
}