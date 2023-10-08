using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace DotWeenExamples
{
    public class MoveTween : MonoBehaviour
    {
      
        [SerializeField] private float _maxSpeed;

        private Camera _camera;

        private Tweener _moveTweener;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetButtonDown("Fire1"))
            {
                return;
            } 
            
            
            Vector3 postion =  _camera.ScreenToWorldPoint(Input.mousePosition);
            postion.z = 0;
            float time = Vector2.Distance(transform.position, postion) / _maxSpeed;

            _moveTweener?.Kill();
            _moveTweener = transform.DOMove(postion, time).SetEase(Ease.InOutCubic);
        }
    }
}