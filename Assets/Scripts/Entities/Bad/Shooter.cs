using System;
using Animation;
using Entities.Perfect;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Bad
{
    public class Shooter : MonoBehaviour, IDamageable
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private LayerMask _target;
        [SerializeField] private float _range;
        [SerializeField] private int _hp;

        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField, SpineAnimation] private string _attackAnimation;
        [SerializeField, SpineAnimation] private string _idleAnimation;
        [SerializeField, SpineEvent] private string _attackEvent;

        private Vector2 _endReach;
        private TrackEntry _currentTrack;
        
        private AnimationType _currentAnimationType;

        private void Start()
        {
            _endReach = new Vector2(_shootPoint.position.x + _range, _shootPoint.position.y);
            _skeletonAnimation.AnimationState.Event += OnAnimationEvent;
        }

        private void Update() => SetAnimationState(AnimationType.Action, EnemyInReach());
        
        #region Animation
        
        private string GetAnimationName(AnimationType animationType)
        {
            switch (animationType)
            {
                case  AnimationType.Idle:
                    return _idleAnimation;
                case AnimationType.Action:
                    return _attackAnimation;
            }
            return string.Empty;
        }
        
        private void SetAnimationState(AnimationType animationType, bool enable, bool loop = false, Action onComplete = null)
        {
            if (!enable)
            {
                if (_currentAnimationType == animationType)
                    PlayIdle();
                return;
            }
            
            if(animationType <= _currentAnimationType)
                return;
            
            _currentAnimationType = animationType;
            PlayAnimation(GetAnimationName(animationType), loop, onComplete);
        }

        private void PlayAnimation(string animationName, bool loop, Action onComplete)
        {
            var te = _skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);

            if (loop) 
                return;
            if (onComplete == null)
                te.Complete += _ => PlayIdle();
            else
                te.Complete += _ => onComplete?.Invoke();
        }
        
        private void PlayIdle()
        {
            _currentAnimationType = AnimationType.Idle;
            _skeletonAnimation.AnimationState.SetAnimation(0, _idleAnimation, true);
        }
        
        private void OnAnimationEvent(TrackEntry _, Event e)
        {
            if(e.Data.Name == _attackEvent)
                Shoot();
        }
        
        #endregion

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            if(_hp <= 0)
                Destroy(gameObject);
        }

        private void Shoot()
        {
            Bullet bullet =  Instantiate(_bulletPrefab, transform);
            bullet.transform.position = _shootPoint.position;
            bullet.MoveTo(_endReach);
        }

        private bool EnemyInReach() => Physics2D.OverlapArea(_shootPoint.position, _endReach, _target);
        
    }
}