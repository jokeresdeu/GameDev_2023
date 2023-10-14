using Spine.Unity;
using UnityEngine;

namespace DotWeenExamples.TweenerExamples
{
    public class MoveTween : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed;
        [SerializeField] private SkeletonAnimation _skeleton;
        [SpineAnimation, SerializeField] private string _deathAnimation;
        
        private void Start()
        {
            
        }

        private void Die()
        {
            _skeleton.AnimationState.SetAnimation(0, _deathAnimation, false);
        }
    }
}