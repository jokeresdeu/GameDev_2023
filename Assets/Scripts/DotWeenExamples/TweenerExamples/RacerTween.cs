using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class RacerTween : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
     
        private void Start()
        {
            float distance = Mathf.Abs(transform.position.x - _endPoint.position.x);
            float time = distance / Random.Range(_minSpeed, _maxSpeed);
            var tweener = transform.DOMoveX(_endPoint.position.x, time).SetEase(Ease.Linear);
            tweener.OnStepComplete(Rotate);
            tweener.SetLoops(-1, LoopType.Yoyo);
        }

        private void Rotate()
        {
            transform.Rotate(0, 180, 0);
        }
    }
}