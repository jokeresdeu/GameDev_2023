using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples
{
    public class BallTweener : MonoBehaviour
    {
        [SerializeField] private Transform _ball;
        [SerializeField] private Transform _moveToPoint;
        [SerializeField] private float _moveTime;
        [SerializeField] private Vector2 _sizeVector;
        [SerializeField] private float _scaleTime;
        [SerializeField] private Rigidbody2D _platform;

        private void Start()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_ball.DOMove(_moveToPoint.transform.position, _moveTime).SetEase(Ease.InQuad));
            sequence.Append(_ball.DOScale(_sizeVector, _scaleTime).SetEase(Ease.OutQuart));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }
    }
}
