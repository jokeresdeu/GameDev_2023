using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class MoveTween : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed;
        [SerializeField] private SkeletonAnimation _skeleton;
        [SpineAnimation, SerializeField] private string _deathAnimation;
        
        private void Start()
        {
            float time = Mathf.Abs(transform.position.x - _endPoint.position.x) / _speed;
            Tweener tweener = transform.DOMoveX(_endPoint.position.x, time).SetEase(Ease.Linear);
            tweener.OnComplete(Die);
        }

        private void Die()
        {
            _skeleton.AnimationState.SetAnimation(0, _deathAnimation, false);
        }
    }
}