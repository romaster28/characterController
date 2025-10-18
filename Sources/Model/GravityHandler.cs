using System;
using UnityEngine;

public class GravityHandler
{
    private readonly Func<bool> _checkGrounded;
    private readonly float _gravity;
    private readonly float _jumpHeight;
    
    private const float CoyoteTimeCounter = .2f;

    public GravityHandler(Func<bool> checkGrounded, float gravity, float jumpHeight)
    {
        if (jumpHeight < 0)
            throw new ArgumentOutOfRangeException(nameof(jumpHeight));
        
        _checkGrounded = checkGrounded;
        _gravity = gravity;
        _jumpHeight = jumpHeight;
    }

    public void Handle(ref Vector3 velocity)
    {
        if (!_checkGrounded.Invoke())
        {
            velocity.y += _gravity * Time.fixedDeltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = 0;
        }
    }

    public bool CanJump()
    {
        return CoyoteTimeCounter > 0 && _checkGrounded.Invoke();
    }

    public void MakeJump(ref Vector3 velocity)
    {
        velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }
}