using System;
using System.Collections;
using System.Collections.Generic;
using Animation;
using ObjectPool;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities
{
    public abstract class BaseEntity : MonoBehaviour, IDamageable, IPoolable
    {
        [SerializeField] private int _hp;

        [Header("Animation")]
        [SerializeField] private SkeletonAnimation _skeleton;
        [SerializeField] private List<AnimationTypeName> _animationStates;
        [SerializeField, SpineAnimation] private string _idleAnimation;

        private MeshRenderer _meshRenderer;
        private AnimationType _currentAnimationType;
        private int _currentHp;
        
        private Coroutine _actionCoroutine;
        
        [field: SerializeField] protected Transform ActionPoint { get; set; }
        
        public GameObject GameObject => gameObject;
        public event Action<IPoolable> ReturnRequested;
        
        public virtual void Initialize()
        {
            _meshRenderer = _skeleton.GetComponent<MeshRenderer>();
            _skeleton.AnimationState.Event += OnAnimationEvent;
            _actionCoroutine = StartCoroutine(ActionCoroutine());
            _currentHp = _hp;
            PlayIdle();
        }

        public void SetSortingOrder(int sortingOrder) => 
            _meshRenderer.sortingOrder = sortingOrder;

        public virtual void TakeDamage(int damage)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
                Death();
        }

        public virtual void ResetPoolable()
        {
            StopCoroutine(_actionCoroutine);
            _skeleton.AnimationState.Event -= OnAnimationEvent;
        }

        public void Remove() => ReturnRequested?.Invoke(this);

        protected abstract IEnumerator ActionCoroutine();

        protected virtual void Death() => Remove();

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