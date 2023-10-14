using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class FadeTween : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            var tween = _spriteRenderer.DOFade(0.2f, 1f);
            tween.SetLoops(-1, LoopType.Yoyo);
        }
    }
}