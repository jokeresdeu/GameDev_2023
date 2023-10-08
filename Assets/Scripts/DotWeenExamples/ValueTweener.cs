using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples
{
    public class ValueTweener : MonoBehaviour
    {
        private const float _maxValue = 20000;

        private  float _value = 1;

        private void Start()
        {
            DOTween.To(() => _value, value => _value = value, _maxValue, 10f).SetEase(Ease.OutQuart);
        }

        private void Update()
        {
            Debug.Log(_value);
        }
    }
}