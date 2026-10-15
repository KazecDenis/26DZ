using UnityEngine;

public class CharacterMover
{
    private readonly float _moveSpeed;
    private readonly float _gravity;

    public CharacterMover (float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public Vector2 Move(Vector2 velocity, float horizontalInput)
    {
        velocity.x = horizontalInput * _moveSpeed;

        return velocity;
    }

    public Vector2 ApplyGravity(Vector2 velocity, bool isGrounded, float deltaTime)
    {
        if (isGrounded && velocity.y <= 0)
            velocity.y = 0;
        else
            velocity.y -= _gravity * deltaTime;

        return velocity;
    }

    public Vector2 HandleCeil(Vector2 velocity, bool isTouchingCeil)
    {
        if (isTouchingCeil)
            velocity.y = Mathf.Min(0, velocity.y);

        return velocity;
    }
}
