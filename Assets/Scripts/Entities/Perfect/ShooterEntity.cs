using Animation;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Perfect
{
    public class ShooterEntity : BaseEntity
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private LayerMask _target;
        [SerializeField] private float _range;
        
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
            Bullet bullet =  Instantiate(_bulletPrefab, transform);
            bullet.transform.position = ActionPoint.position;
            bullet.MoveTo(_endReach);
        }

        private bool EnemyInReach() => Physics2D.OverlapArea(ActionPoint.position, _endReach, _target);
    }
}