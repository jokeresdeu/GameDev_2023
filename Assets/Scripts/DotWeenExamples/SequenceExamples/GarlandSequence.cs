using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DotWeenExamples.SequenceExamples
{
    public class  GarlandSequence : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _elements;
        [SerializeField] private List<Color> _colors;
        [SerializeField] private float _elementTime;

        private void Start() => PlayGarlandSequence();

        private void PlayGarlandSequence()
        {
            OneWayGarlandSequence().onComplete += PlayGarlandSequence;
        }

        private Sequence OneWayGarlandSequence()
        {
            var color = _colors[Random.Range(0, _colors.Count)];
            var sequence = DOTween.Sequence();
            foreach (var element in _elements)
                sequence.Append(GetColorChangeTween(element, color));
            return sequence;
        }

        private Tweener GetColorChangeTween(SpriteRenderer render, Color color) => render.DOColor(color, _elementTime);
    }
}