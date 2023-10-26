using System;
using Animation;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities.Bad
{
    public class Walker : MonoBehaviour
    {
        [SerializeField] private int _hp;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _targetLayer;
        
        [Header("Movement")]
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed;

        [Header("Animation")]
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField, SpineAnimation] private string _walkAnimation;
        [SerializeField, SpineAnimation] private string _attackAnimation;
        [SerializeField, SpineAnimation] private string _deathAnimation;
        [SerializeField, SpineEvent] private string _attackEvent;
        
        private Tweener _moveTweener;
        private Shooter _target;
      
        private AnimationType _currentAnimationType;

        private void Start()
        {
            _skeletonAnimation.AnimationState.Event += OnAnimationEvent;
            StartMoving();
        }

        private void FixedUpdate() => SetAnimationState(AnimationType.Action, TryGetAttackTarget(out _target), true);

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            if (_hp <= 0)
                Die();
        }

        private void Die()
        {
            _moveTweener?.Kill();
            _collider.enabled = false;
            SetAnimationState(AnimationType.Death, true, false, ()=> Destroy(gameObject));
        }

        #region Animation
        
        private string GetAnimationName(AnimationType animationType)
        {
            switch (animationType)
            {
                case  AnimationType.Walk:
                    return _walkAnimation;
                case AnimationType.Action:
                    return _attackAnimation;
                case AnimationType.Death:
                    return _deathAnimation;
            }
            return string.Empty;
        }
        
        private void SetAnimationState(AnimationType animationType, bool enable, bool loop = false, Action onComplete = null)
        {
            if (!enable)
            {
                if (_currentAnimationType == animationType)
                    StartMoving();
                return;
            }
            
            if(animationType <= _currentAnimationType)
                return;
            
            _moveTweener?.Kill();
            _currentAnimationType = animationType;
            PlayAnimation(GetAnimationName(animationType), loop, onComplete);
        }

        private void PlayAnimation(string animationName, bool loop, Action onComplete)
        {
            var te = _skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);

            if (loop || onComplete == null) 
                return;
            
            te.Complete += _ => onComplete.Invoke();
        }

        private void OnAnimationEvent(TrackEntry _, Event e)
        {
            if(e.Data.Name == _attackEvent)
                DealDamage();
        }
        
        #endregion
        
        private void StartMoving()
        {
            _currentAnimationType = AnimationType.Walk;
            _skeletonAnimation.AnimationState.SetAnimation(0, _walkAnimation, true);
            var time = Mathf.Abs(_endPoint.position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(_endPoint.position.x, time);
            _skeletonAnimation.AnimationState.SetAnimation(0, _walkAnimation, true);
        }

        private void DealDamage() => _target.TakeDamage(_damage);

        private bool TryGetAttackTarget(out Shooter shooter)
        {
            shooter = null;
            var col = Physics2D.OverlapPoint(_attackPoint.position, _targetLayer);
            if (col == null)
                return false;

            shooter = col.GetComponent<Shooter>();
            return true;
        }
    }
}
