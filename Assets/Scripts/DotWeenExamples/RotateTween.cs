using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples
{
    public class RotateTween : MonoBehaviour
    {
        private void Start()
        {
            transform.DORotate(new Vector3(0,0,180), 2f).SetLoops(-1, LoopType.Incremental);
        }
    }
}