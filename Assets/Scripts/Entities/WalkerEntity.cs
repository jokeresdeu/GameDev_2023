using System;
using System.Collections;
using Animation;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Entities
{
    public class WalkerEntity : BaseEntity
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        
        [SerializeField] private Transform _endPoint;
        [SerializeField] private LayerMask _playerLayerMask;
        [SerializeField] private Collider2D _collider;
        
        [SerializeField, SpineEvent] private string _attackEvent;
        
        private IDamageable _target;
        private Tweener _moveTweener;

        private void Start() => Initialize();

        public override void ResetPoolable()
        {
            base.ResetPoolable();
            _collider.enabled = true;
        }

        protected override IEnumerator ActionCoroutine()
        {
            while (true)
            {
                var action = TryGetAttackTarget(out _target);
                SetAnimationState(AnimationType.Action, action, true);
                if(action)
                    _moveTweener?.Kill();
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        private bool TryGetAttackTarget(out IDamageable target)
        {
            target = null;
            var col = Physics2D.OverlapPoint(ActionPoint.position, _playerLayerMask);
            if(col == null)
                return false;

            target = col.GetComponent<IDamageable>();
            return true;
        }

        protected override void PlayIdle()
        {
            base.PlayIdle();
            var time = Mathf.Abs(_endPoint.position.x - transform.position.x) / _speed;
            _moveTweener = transform.DOMoveX(_endPoint.position.x, time);
        }

        protected override void OnAnimationEvent(TrackEntry _, Event e)
        {
            if(e.Data.Name == _attackEvent)
                DealDamage();
        }
      
        private void DealDamage() => _target.TakeDamage(_damage);

        protected override void Death()
        {
            SetAnimationState(AnimationType.Death, true, false, base.Death);
            _collider.enabled = false;
            _moveTweener?.Kill();
        }
    }
}