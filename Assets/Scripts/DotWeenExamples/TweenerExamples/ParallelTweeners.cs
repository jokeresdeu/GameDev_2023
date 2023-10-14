using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class ParallelTweeners : MonoBehaviour
    {
        [SerializeField] private Transform _pointToMove;
        private void Start()
        {
            transform.DOMove(_pointToMove.position, 1f);
            transform.DORotate(new Vector3(0,0,180), 1f);
            transform.DOScale(2f, 1f);
        }
    }
}