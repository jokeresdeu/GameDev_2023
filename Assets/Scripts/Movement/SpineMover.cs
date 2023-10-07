using Animation;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Movement
{
    public class SpineMover : MonoBehaviour
    {
        [Header("HorizontalMovement")] 
        [SerializeField] private float _speed;
        [SerializeField] private bool _faceRight;
        
        [Header("Animations")]
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SpineAnimation, SerializeField] private string _runAnimation;
        [SpineAnimation, SerializeField] private string _attackAnimation;

        [Header("AnimationEvents")]
        [SpineEvent, SerializeField] private string _attackEvent;

        [Header("Skins")]
        [SpineSkin, SerializeField] private string _defaultSkin;
        [SpineSkin, SerializeField] private string _woodenBucketSkin;
        [SpineSkin, SerializeField] private string _ironBucketSkin;
        
        private Rigidbody2D _rigidbody2D;
        private float _direction;
        
        private AnimationType _currentAnimationType;
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _skeletonAnimation.AnimationState.Event += ActionRequired;
        }

        private void ActionRequired(TrackEntry trackentry, Event e)
        {
            if(e.Data.Name == _attackEvent)
                Debug.Log("Attack");
        }

        private void Update()
        {
            _direction = Input.GetAxisRaw("Horizontal");
            PlayAnimation(AnimationType.Run, _direction != 0, true);
            
            if(Input.GetButtonDown("Fire1"))
                PlayAnimation(AnimationType.Attack, true);
            
            if(Input.GetKeyDown(KeyCode.Alpha0))
                _skeletonAnimation.Skeleton.SetSkin(_defaultSkin);
            else if(Input.GetKeyDown(KeyCode.Alpha1))
                _skeletonAnimation.Skeleton.SetSkin(_woodenBucketSkin);
            else if(Input.GetKeyDown(KeyCode.Alpha2))
                _skeletonAnimation.Skeleton.SetSkin(_ironBucketSkin);
        }
        
        private void FixedUpdate()
        {
            _rigidbody2D.velocity = new Vector2(_speed * _direction * _speed, _rigidbody2D.velocity.y);
            SetDirection();
        }

        private void PlayAnimation(AnimationType animationType, bool enable, bool loop = false)
        {
            if (!enable)
            {
                if (_currentAnimationType == animationType)
                    PlayIdle();
                return;
            }
            
            if(animationType <= _currentAnimationType)
                return;
            
            _currentAnimationType = animationType;
            _skeletonAnimation.timeScale = 1;
            PlayAnimation(GetAnimationName(animationType), loop);
        }

        private void PlayAnimation(string animationName, bool loop)
        {
            var te = _skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
            
            if(!loop)
                te.Complete += _ => PlayIdle();
        }

        private string GetAnimationName(AnimationType animationType)
        {
            switch (animationType)
            {
                case  AnimationType.Run:
                    return _runAnimation;
                case AnimationType.Attack:
                    return _attackAnimation;
            }
            Debug.LogError($"Animation {animationType} is not supported");
            return string.Empty;
        }

        private void PlayIdle()
        {
            _skeletonAnimation.timeScale = 0;
            _currentAnimationType = AnimationType.Idle;
        }
        
        
        
        #region Direction

        private void SetDirection()
        {
            if (_faceRight && _direction < 0)
                Flip();
            else if (!_faceRight && _direction > 0)
                Flip();
        }

        private void Flip()
        {
            _faceRight = !_faceRight;
            transform.Rotate(0, 180, 0);
        }

        #endregion
    }
}