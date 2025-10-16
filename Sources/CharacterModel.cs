using System;
using UnityEngine;

public class CharacterModel
{
    private readonly ICharacterConfig _config;

    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private bool _grounded;
    private float _coyoteTimeCounter = .2f;
    
    public CharacterModel(ICharacterConfig config, Vector3 startPosition)
    {
        _config = config ?? throw new ArgumentOutOfRangeException(nameof(config));
        Transform = new TransformModel(startPosition);
    }

    public TransformModel Transform { get; }

    public void MoveByDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    public bool TryJump()
    {
        if (!CanJump())
            return false;

        Jump();
        
        return true;
    }

    public bool CanJump()
    {
        return _coyoteTimeCounter > 0 && _grounded;
    }
    
    public void TickSimulate()
    {
        CheckGrounded();
        HandleGravity();
        HandleMovement();
        
        ApplyVelocity();
    }

    private void CheckGrounded()
    {
        _grounded = Physics.CheckSphere(Transform.Position, _config.GroundCheckRadius, _config.Ground);
    }

    private void HandleMovement()
    {
        Vector3 targetVelocity = _moveDirection * _config.MoveSpeed;
        float controlFactor = _grounded ? 1f : _config.AirControl;
        
        _velocity.x = Mathf.Lerp(_velocity.x, targetVelocity.x, _config.Acceleration * controlFactor * Time.deltaTime);
        _velocity.z = Mathf.Lerp(_velocity.z, targetVelocity.z, _config.Acceleration * controlFactor * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (!_grounded)
        {
            _velocity.y += _config.Gravity * Time.deltaTime;
        }
        else if (_velocity.y < 0)
        {
            _velocity.y = 0;
        }
        
        _velocity.y = Mathf.Max(_velocity.y, _config.Gravity * 2f);
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_config.JumpHeight * -2f * _config.Gravity);
    }

    private void ApplyVelocity()
    {
        Transform.Position += _velocity * Time.deltaTime;
    }
}