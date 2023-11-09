using System.Collections.Generic;
using Animation;
using Projectiles;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Good
{
    public class ShooterEntity : BaseEntity
    {
        private readonly List<Bullet> _bulletsPool = new();
        
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
            Bullet bullet;
            if (_bulletsPool.Count > 0)
            {
                bullet = _bulletsPool[0];
                _bulletsPool.RemoveAt(0);
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Instantiate(_bulletPrefab, transform);
            }

            bullet.ReturnRequested += ReturnBullet;
            bullet.transform.position = ActionPoint.position;
            bullet.MoveTo(_endReach);
        }

        private void ReturnBullet(Bullet bullet)
        {
            _bulletsPool.Add(bullet);
            bullet.gameObject.SetActive(false);
            bullet.ReturnRequested -= ReturnBullet;
            bullet.ResetBullet();
        }

        private bool EnemyInReach() => Physics2D.OverlapArea(ActionPoint.position, _endReach, _target);
    }
}