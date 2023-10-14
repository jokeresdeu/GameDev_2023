using DG.Tweening;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class MouseMoverTween : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if(!Input.GetButtonDown("Fire1"))
                return;

            var position = _camera.ScreenToWorldPoint(Input.mousePosition);
            var time = Vector2.Distance(position, transform.position) / _speed;
            position.z = 0;
            transform.DOMove(position, time);
        }
    }
}