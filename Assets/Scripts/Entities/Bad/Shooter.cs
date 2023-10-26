using System;
using Animation;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Bad
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Bullet _bullet;
        [SerializeField] private LayerMask _target;
        [SerializeField] private float _range;
        [SerializeField] private int _hp;
        
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField, SpineAnimation] private string _attackAnimation;
        [SerializeField, SpineAnimation] private string _idleAnimation;
        [SerializeField, SpineEvent] private string _attackEvent;
        
        private TrackEntry _currentTrack;
        
        private AnimationType _currentAnimationType;

        private void Start()
        {
            _skeletonAnimation.AnimationState.Event += OnAnimationEvent;
        }

        private void FixedUpdate() => SetAnimationState(AnimationType.Action, EnemyInRange(), true);

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_shootPoint.position, _shootPoint.position + new Vector3(_range, 0, 0));
        }
        
        public void TakeDamage(int damage)
        {
            _hp -= damage;
            if (_hp <= 0)
                Destroy(gameObject);
        }

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
            if (e.Data.Name == _attackEvent)
                Shoot();
        }
        
        #endregion

        private void Shoot()
        {
            Bullet bullet = Instantiate(_bullet, transform);
            bullet.transform.position = _shootPoint.position;
            bullet.MoveTo(_shootPoint.position + Vector3.right * _range);
        }

        private bool EnemyInRange() => 
            Physics2D.Raycast(_shootPoint.position, Vector2.right, _range, _target);
    }
}