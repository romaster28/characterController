using System;
using UnityEngine;

public class CharacterModel
{
    private readonly TransformModel _transform;
    
    private readonly CollisionHandler _collisionHandler;
    private readonly MoveHandler _moveHandler;
    private readonly GravityHandler _gravityHandler;
    
    private Vector3 _velocity;

    public CharacterModel(ICharacterConfig config, Vector3 startPosition)
    {
        _transform = new TransformModel(startPosition);
        _collisionHandler = new CollisionHandler(config.Ground, config.GroundCheckRadius);
        _moveHandler = new MoveHandler(_collisionHandler, _transform, new MoveHandler.Config(config.Acceleration, config.MoveSpeed, config.AirControl));
        _gravityHandler = new GravityHandler(_collisionHandler.CheckGrounded, config.Gravity, config.JumpHeight);
    }

    public IReadOnlyTransform Transform => _transform;

    public void RotateToAngle(float angle)
    {
        _transform.Rotate(angle);
    }
    
    public void MoveByDirection(Vector3 direction)
    {
        _moveHandler.UpdateDirection(direction); 
    }

    public bool TryJump()
    {
        if (!_gravityHandler.CanJump())
            return false;

        _gravityHandler.MakeJump(ref _velocity);
        return true;
    }

    public void TickSimulate()
    {
        _collisionHandler.UpdatePoint(Transform.Position);
        _gravityHandler.Handle(ref _velocity);
        _moveHandler.Handle(ref _velocity);
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        _transform.Position += _velocity * Time.fixedDeltaTime;
    }
}