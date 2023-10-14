using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace DotWeenExamples
{
    public class RandomMoveTweener : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private SkeletonAnimation _skeleton;
        [SpineAnimation, SerializeField] private string _deathAnimation;

        private void Start()
        {
            float time = Mathf.Abs(transform.position.x -_endPoint.position.x) / Random.Range(_minSpeed, _maxSpeed);
            Tweener tweener = transform.DOMoveX(_endPoint.position.x, time).SetEase(Ease.Linear);
            tweener.SetLoops(5, LoopType.Yoyo);
            tweener.OnStepComplete(Rotate);
            tweener.OnComplete(Die);
        }

        private void Die()
        {
            _skeleton.AnimationState.SetAnimation(0, _deathAnimation, false);
        }

        private void Rotate()
        {
            transform.Rotate(0, 180, 0);
        }
    }
}