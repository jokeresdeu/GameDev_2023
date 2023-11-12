using Animation;
using ObjectPool.Better;
using Projectiles;
using Projectiles.Good;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Better
{
    public class ShooterEntity : BaseEntity
    {
        [SerializeField] private LayerMask _target;
        [SerializeField] private float _range;
        [SerializeField] private BulletsObjectPool _bulletsObjectPool;
        
        [SerializeField, SpineEvent] private string _attackEvent;

        private Vector2 _endReach;
        protected override void Start()
        {
            _endReach = new Vector2(ActionPoint.position.x + _range, ActionPoint.position.y);
            base.Start();
        }

        private void FixedUpdate() => SetAnimationState(AnimationType.Action, EnemyInReach(), true);
        
        protected override void Death() => Destroy(gameObject);

        protected override void OnAnimationEvent(TrackEntry _, Event e)
        {
            if(e.Data.Name == _attackEvent)
                Shoot();
        }
        
        private void Shoot()
        {
            Bullet bullet = _bulletsObjectPool.GetBullet();
            bullet.transform.position = ActionPoint.position;
            bullet.gameObject.SetActive(true);
            bullet.MoveTo(_endReach);
        }
        
        private bool EnemyInReach() => Physics2D.OverlapArea(ActionPoint.position, _endReach, _target);
    }
}