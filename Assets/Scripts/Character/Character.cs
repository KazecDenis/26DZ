using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private ObstacleChecker _groundChecker;
    [SerializeField] private ObstacleChecker _ceilChecker;
    [SerializeField] private ObstacleChecker _leftWallChecker;
    [SerializeField] private ObstacleChecker _rightWallChecker;
    [SerializeField] private float _jumpForceWallX;
    [SerializeField] private float _jumpForceWallY;
    [SerializeField] private float _wallJumpDelay = 0.3f;
    [SerializeField] private float _yVelocity;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _deadJumpForce;
    [SerializeField] private float _deadGravity;
    [SerializeField] private float _restartDelay;
    private bool _isDead;
    private float _wallSlideSpeed = 1.5f;
    private Vector2 _velocity;
    private readonly Quaternion TurnRight = Quaternion.identity;
    private readonly Quaternion TurnLeft = Quaternion.Euler(0, 180, 0);
    private PlayerInput _playerInput;
    private CharacterMover _characterMover;
    private CharacterJumper _characterJumper;
    private float _horizontalInput;
    private bool _isJumpPressed;
    private bool IsWallTouches => _leftWallChecker.IsTouches() || _rightWallChecker.IsTouches();
    public bool IsGrounded => _groundChecker.IsTouches();
    public Vector2 Velocity => _rigidbody.velocity;



    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterMover = new CharacterMover(_moveSpeed, _gravity);
        _characterJumper = new CharacterJumper(_yVelocity, _wallSlideSpeed, _jumpForceWallX, _jumpForceWallY, _wallJumpDelay, this);
    }

    private void Update()
    {
        if (_isDead)
            return;

        _horizontalInput = _playerInput.Horizontal;

        if (_playerInput.JumpPressed)
            _isJumpPressed = true;
    }

    private void FixedUpdate()
    {
        if (_isDead)
            return;

        if (!_characterJumper.isWallJumping)
            _velocity = _characterMover.Move(_velocity, _horizontalInput);
        
        _velocity = _characterMover.HandleGravity(_velocity, IsGrounded, Time.fixedDeltaTime);
        _velocity = _characterJumper.HandleSlideWall(_velocity, IsWallTouches, IsGrounded);
        _velocity = _characterJumper.HandleJump(_velocity, IsGrounded, _isJumpPressed);
        _velocity = _characterJumper.JumpOffTheWall(_velocity, _isJumpPressed, IsGrounded, _leftWallChecker.IsTouches(), _rightWallChecker.IsTouches());
        _velocity = _characterMover.HandleCeil(_velocity, _ceilChecker.IsTouches());

        _rigidbody.velocity = _velocity;

        transform.rotation = RotationTo(_velocity);

        _isJumpPressed = false;
    }

    private Quaternion RotationTo(Vector2 velocity)
    {
        if (velocity.x >0)
            return TurnRight;

        if (velocity.x < 0)
            return TurnLeft;

        return transform.rotation;
    }

    public void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        _collider.enabled = false;

        _rigidbody.velocity = Vector2.zero;
        
        _rigidbody.gravityScale = _deadGravity;

        _rigidbody.AddForce(Vector2.up * _deadJumpForce, ForceMode2D.Impulse);

        StartCoroutine(RestartProcess());

    }
    public void DieFromFall()
    {
        if (_isDead)
            return;

        StartCoroutine(RestartProcess());
    }
    private IEnumerator RestartProcess()
    {
        yield return new WaitForSeconds(_restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

   

