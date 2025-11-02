using System;
using UnityEngine;

public class MoveHandler
{
    private readonly Config _config;
    private readonly TransformModel _transform;
    private readonly CollisionHandler _collisionHandler;
    
    private Vector3 _direction;

    public MoveHandler(CollisionHandler collisionHandler, TransformModel transform, Config config)
    {
        _collisionHandler = collisionHandler ?? throw new ArgumentNullException(nameof(collisionHandler));
        _transform = transform ?? throw new ArgumentNullException(nameof(transform));
        _config = config;
    }

    public void UpdateDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public void Handle(ref Vector3 velocity)
    {
        Vector3 convertedDirection = _transform.TranslateDirection(_direction);
        convertedDirection = Vector3.ProjectOnPlane(convertedDirection, _collisionHandler.GetNormal(convertedDirection));

        bool isGrounded = _collisionHandler.CheckGrounded();
        Vector3 targetVelocity = _direction * _config.Speed;
        float controlFactor = isGrounded ? 1f : _config.AirControl;
        float timeFactor = _config.Acceleration * controlFactor * Time.deltaTime;
        
        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, timeFactor);
        
        if (isGrounded)
            velocity.y = Mathf.Lerp(velocity.y, targetVelocity.y, timeFactor);
        
        velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, timeFactor);
    }

    public class Config
    {
        public float Acceleration { get; }
        public float Speed { get; }
        public float AirControl { get; }

        public Config(float acceleration, float speed, float airControl)
        {
            if (airControl < 0)
                throw new ArgumentOutOfRangeException(nameof(airControl));

            if (acceleration < 0)
                throw new ArgumentOutOfRangeException(nameof(acceleration));

            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed));
            
            Acceleration = acceleration;
            Speed = speed;
            AirControl = airControl;
        }
    }
}