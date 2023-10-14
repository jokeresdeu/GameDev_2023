using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class PunchTween : MonoBehaviour
    {
        private void Start()
        {
            transform.DOPunchPosition(Vector3.up, 2, 2).SetLoops(-1, LoopType.Restart);
        }
    }
}