using System.Collections;
using UnityEngine;

public class CharacterJumper
{
    private readonly float _jumpVelocity;
    private readonly float _wallSlideSpeed;
    private readonly float _jumpForceWallX;
    private readonly float _jumpForceWallY;
    private readonly float _jumpWallDelay;
    private readonly MonoBehaviour _coroutineRunner;
    public bool isWallJumping {get; private set;}

    public CharacterJumper(float jumpVelocity, float wallSlideSpeed, float jumpForceWallX, float jumpForceWallY, float jumpWallDelay, MonoBehaviour coroutineRunner)
    {
        _jumpVelocity = jumpVelocity;
        _wallSlideSpeed = wallSlideSpeed;
        _coroutineRunner = coroutineRunner;
        _jumpForceWallX = jumpForceWallX;
        _jumpForceWallY = jumpForceWallY;
        _jumpWallDelay = jumpWallDelay;
    }
    public Vector2 HandleJump (Vector2 velocity, bool isGrounded, bool jumpPressed)
    {
        if (jumpPressed && isGrounded)
            velocity.y = _jumpVelocity;
        
        return velocity;
    }

    public Vector2 HandleSlideWall(Vector2 velocity, bool IsWallTouches, bool isGrounded)
    {
        if (isWallJumping)
            return velocity;

        if (IsWallTouches && !isGrounded && velocity.y < 0)
            velocity.y = Mathf.Max(velocity.y, - _wallSlideSpeed);

        return velocity;
    }

    public Vector2 JumpOffTheWall(Vector2 velocity, bool isJumpPressed, bool isGrounded, bool isLeftWallTouches, bool isRightWallTouches)
    {
        if (!isJumpPressed || isGrounded || isWallJumping)
            return velocity;

        if (isLeftWallTouches)
            velocity = new Vector2(_jumpForceWallX, _jumpForceWallY);
        else if (isRightWallTouches)
            velocity = new Vector2(- _jumpForceWallX, _jumpForceWallY);
        else 
            return velocity;

        isWallJumping = true;

        _coroutineRunner.StartCoroutine(WallJumpProcess());

        return velocity;
    }

    private IEnumerator WallJumpProcess()
    {
        yield return new WaitForSeconds(_jumpWallDelay);

        isWallJumping = false;
    }
        
}
