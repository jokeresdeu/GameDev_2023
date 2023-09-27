using UnityEngine;

namespace Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _crouchAnimation;
        [SerializeField] private string _castAnimation;

        private void Update()
        {
            _animator.SetBool(_crouchAnimation, Input.GetAxis("Vertical") < 0);
            
            if(Input.GetButtonDown("Fire1"))
                _animator.SetBool(_castAnimation, true);
        }

        private void EndCast()
        {
            Debug.Log("Cast");
            _animator.SetBool(_castAnimation, false);
        }

        private void ResetAnimationParams()
        {
            _animator.SetBool(_castAnimation, false);
        }
    }
}