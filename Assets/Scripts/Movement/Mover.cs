using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {
        [Header("HorizontalMovement")] [SerializeField]
        private float _speed;

        [SerializeField] private bool _faceRight;
        [SerializeField] private bool _airMove;
        [Range(0, 1)] [SerializeField] private float _airMoveModificator;

        [Header("Jump")] [SerializeField] private float _jumpPower;
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private LayerMask _whatIsGround;

        [Header("Crouch")] [SerializeField] private BoxCollider2D _boxCollider2D;
        [SerializeField] private Transform _cellChecker;
        [Range(0, 1)] [SerializeField] private float _crouchModificator;

        [Header("Animation")] 
        [SerializeField] private Animator _animator;
        private AnimationState _currentState;
        private string _animatorParameterName;

        private Rigidbody2D _rigidbody2D;
        private float _direction;
        private bool _jump;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animatorParameterName = _animator.GetParameter(0).name;
        }

        private void Update()
        {
            _direction = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump"))
                _jump = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckRadius);
        }

        private void FixedUpdate()
        {
            bool isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckRadius, _whatIsGround);
            bool canStand = !Physics2D.OverlapBox(_cellChecker.position, _boxCollider2D.size, 0, _whatIsGround);

            if (_jump && isGrounded)
                _rigidbody2D.AddForce(Vector2.up * _jumpPower);
            _jump = false;

            if (Input.GetAxis("Vertical") < 0 || !canStand)
                _boxCollider2D.enabled = false;
            else
                _boxCollider2D.enabled = true;

            if (isGrounded || _airMove)
            {
                float speedModificator = 1;
                if (!canStand)
                    speedModificator = _crouchModificator;
                else if (!isGrounded)
                    speedModificator = _airMoveModificator;

                _rigidbody2D.velocity = new Vector2(_speed * _direction * speedModificator, _rigidbody2D.velocity.y);
                SetDirection();
            }

            PlayAnimation(AnimationState.Jump, !isGrounded && _rigidbody2D.velocity.y > 0);
            PlayAnimation(AnimationState.Fall, !isGrounded && _rigidbody2D.velocity.y < 0);
            PlayAnimation(AnimationState.Crouch, !_boxCollider2D.enabled);
        }
        
        private void PlayAnimation(AnimationState animationState, bool active)
        {
            if (animationState < _currentState)
                return;

            if (!active)
            {
                if (animationState == _currentState)
                {
                    _animator.SetInteger( _animatorParameterName, (int)AnimationState.Idle);
                    _currentState = AnimationState.Idle;
                }
                
                return;
            }

            _animator.SetInteger( _animatorParameterName, (int)animationState);
            _currentState = animationState;
        }

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

        private enum AnimationState
        {
            Idle = 0,
            Run = 1,
            Jump = 2,
            Fall = 3,
            Crouch = 4,
        }
    }
}