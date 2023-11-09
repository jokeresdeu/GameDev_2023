using System;
using System.Collections.Generic;
using Animation;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities
{
    public abstract class BaseEntity : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _hp;

        [Header("Animation")]
        [SerializeField] private SkeletonAnimation _skeleton;
        [SerializeField] private List<AnimationTypeName> _animationStates;
        [SerializeField, SpineAnimation] private string _idleAnimation;

        [field: SerializeField] protected Transform ActionPoint { get; set; }

        private AnimationType _currentAnimationType;
        protected virtual void Start()
        {
            _skeleton.AnimationState.Event += OnAnimationEvent;
            PlayIdle();
        }

        public virtual void TakeDamage(int damage)
        {
            _hp -= damage;
            if (_hp <= 0)
                Death();
        }

        protected abstract void Death();

        #region Animation

        protected void SetAnimationState(AnimationType animationType, bool enable, bool loop = false, Action onComplete = null)
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
            var animationName = _animationStates.Find(element => element.Type == animationType).Name;
            PlayAnimation(animationName, loop, onComplete);
        }

        private void PlayAnimation(string animationName, bool loop, Action onComplete)
        {
            var te = _skeleton.AnimationState.SetAnimation(0, animationName, loop);

            if (loop || onComplete == null) 
                return;

            te.Complete += _ => onComplete.Invoke();
        }
        
        protected virtual void PlayIdle()
        {
            _currentAnimationType = AnimationType.Idle;
            _skeleton.AnimationState.SetAnimation(0, _idleAnimation, true);
        }

        protected abstract void OnAnimationEvent(TrackEntry _, Event e);

        #endregion
    }
}