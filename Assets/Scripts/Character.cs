using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private const string HorizontalAxisKey = "Horizontal";
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private ObstacleChecker _groundChecker;
    [SerializeField] private ObstacleChecker _ceilChecker;
    [SerializeField] private ObstacleChecker _leftWallChecker;
    [SerializeField] private ObstacleChecker _rightWallChecker;
    [SerializeField] private float _jumpForceWallX;
    [SerializeField] private float _jumpForceWallY;
    
    [SerializeField] private float _wallJumpDelay = 0.3f;
    [SerializeField] private float _yVelocity;
    [SerializeField] private float _gravity;
    private float _wallSlideSpeed = 1.5f;
    private Vector2 _velocity;
    private bool _jumpPressed;
    private bool _isWallJumping;

    private readonly Quaternion TurnRight = Quaternion.identity;
    private readonly Quaternion TurnLeft = Quaternion.Euler(0, 180, 0);

    public Vector2 Velocity => _rigidbody.velocity;
    public bool IsGrounded => _groundChecker.IsTouches();

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw(HorizontalAxisKey);

        _jumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (!_isWallJumping)
        {
            float horizontalVelocity = _moveSpeed * horizontalInput;

            _velocity = new Vector2(horizontalVelocity, _velocity.y);
        }
            


        HandleGravity();
        HandleSlideWall();
        HandleJump();
        JumpOffTheWall();
        HandleCeil();

        _rigidbody.velocity = _velocity;

        transform.rotation = RotationTo(_velocity);
    }


    private void HandleCeil()
    {
        if (_ceilChecker.IsTouches())
            _velocity.y = MathF.Min(0, _velocity.y);
    }
    private void HandleGravity()
    {
        if (_groundChecker.IsTouches() && _velocity.y <= 0)
            _velocity.y = 0;
        else   
            _velocity.y -= _gravity * Time.deltaTime;
    }
            
    private void HandleJump()
    {
        if (_jumpPressed && _groundChecker.IsTouches())
            _velocity.y = _yVelocity;
    }

    private void HandleSlideWall()
    {
        if (_isWallJumping)
            return;

        if (IsWallTouches && !IsGrounded && _velocity.y < 0)
        {
            _velocity.y = Math.Max(_velocity.y, -_wallSlideSpeed);
        }
    }

    private void JumpOffTheWall()
    {
        if (!_jumpPressed || IsGrounded || _isWallJumping)
            return;

        if (_leftWallChecker.IsTouches())
            _velocity = new Vector2(_jumpForceWallX, _jumpForceWallY);
        else if (_rightWallChecker.IsTouches())
            _velocity = new Vector2(- _jumpForceWallX, _jumpForceWallY);
        else
            return;

        StartCoroutine(WallJumpProcess());
    }

    private bool IsWallTouches => _leftWallChecker.IsTouches() || _rightWallChecker.IsTouches();
    private Quaternion RotationTo(Vector2 velocity)
    {
        if (velocity.x >0)
            return TurnRight;

        if (velocity.x < 0)
            return TurnLeft;

        return transform.rotation;
    }

    private IEnumerator WallJumpProcess()
    {
        _isWallJumping = true;

        yield return new WaitForSeconds(_wallJumpDelay);

        _isWallJumping = false;
    }
}

