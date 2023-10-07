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

        private Vector2 _platformPosition;
        
        private void Start()
        {
            var originalPos = _ball.position;
            var originalSize = _ball.localScale;
            _platformPosition = _platform.transform.position;
            var sequence = DOTween.Sequence();
            sequence.Join(_ball.DOMove(_moveToPoint.transform.position, _moveTime).SetEase(Ease.InQuad));
            sequence.Append(_ball.DOScale(_sizeVector, _scaleTime).SetEase(Ease.OutQuart));
            var scaleTween = _ball.DOScale(originalSize, _scaleTime).SetEase(Ease.OutQuart);
            scaleTween.onComplete += ActivatePlatform;
            sequence.Append(scaleTween);
            var moveUpTween = _ball.DOMove(originalPos, _moveTime).SetEase(Ease.OutQuad);
            moveUpTween.onComplete += ResetPlatform;
            sequence.Append(moveUpTween);
            sequence.SetLoops(-1);
        }

        private void ActivatePlatform()
        {
            _platform.simulated = true;
        }

        private void ResetPlatform()
        {
            _platform.simulated = false;
            _platform.transform.position = _platformPosition;
        }
    }
}
