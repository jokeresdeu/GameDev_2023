using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.SequenceExamples
{
    public class BallBounceSequence : MonoBehaviour
    {
        [SerializeField] private Transform _ball;
        [SerializeField] private Transform _moveToPoint;
        [SerializeField] private float _moveTime;
        [SerializeField] private Vector2 _sizeVector;
        [SerializeField] private float _scaleTime;

        private Vector2 _platformPosition;
        
        private void Start()
        {
            var originalSize = _ball.transform.localScale;
            Sequence sequence = BounceSequence();
        }

        private Sequence BounceSequence()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_ball.DOMove(_moveToPoint.transform.position, _moveTime).SetEase(Ease.InQuad));
            sequence.Append(_ball.DOScale(_sizeVector, _scaleTime).SetEase(Ease.OutQuart));
            sequence.SetLoops(-1, LoopType.Yoyo);
            return sequence;
        }
    }
}
