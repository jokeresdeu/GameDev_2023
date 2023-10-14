using System;
using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class BallBounceTweens : MonoBehaviour
    {
        [SerializeField] private Transform _ball;
        [SerializeField] private Transform _moveToPoint;
        [SerializeField] private float _moveTime;
        [SerializeField] private Vector2 _sizeVector;
        [SerializeField] private float _scaleTime;

        private Vector2 _originalPosition;
        
        private void Start()
        {
            _originalPosition = _ball.position;
            MoveDown();
        }
        
        private void MoveDown()
        {
            _ball.DOMove(_moveToPoint.transform.position, _moveTime).SetEase(Ease.InQuad).OnComplete(Squeeze);
        }

        private void Squeeze()
        {
            _ball.DOScale(_sizeVector, _scaleTime).SetEase(Ease.OutQuart).OnComplete(UnSqueeze);
        }
        
        private void UnSqueeze()
        {
            _ball.DOScale(Vector3.one, _scaleTime).SetEase(Ease.OutQuart).OnComplete(MoveUp);
        }
        
        private void MoveUp()
        {
            _ball.DOMove(_originalPosition, _moveTime).SetEase(Ease.OutQuad).OnComplete(MoveDown);
        }
    }
}