using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace DotWeenExamples
{
    public class MoveTween : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private SkeletonAnimation _skeleton;
        [SpineAnimation, SerializeField] private string _deathAnimation;
        
        private void Start()
        {
            float distance = Mathf.Abs(transform.position.x - _endPoint.position.x);
            float time = distance / Random.Range(_minSpeed, _maxSpeed);
            var tweener = transform.DOMoveX(_endPoint.position.x, time).SetEase(Ease.Linear);
            tweener.OnStepComplete(Rotate);
            tweener.SetLoops(-1, LoopType.Yoyo);
            
            /*var endPos = new Vector2(_endPoint.position.x, transform.position.y);
            var distance = Vector2.Distance(transform.position, endPos);
            var time = distance / Random.Range(_minSpeed, _maxSpeed);
            transform.DOMove(endPos, time).SetEase(Ease.Linear).OnComplete(Die);*/
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