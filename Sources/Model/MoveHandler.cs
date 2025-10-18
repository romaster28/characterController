using System;
using UnityEngine;

public class MoveHandler
{
    private readonly Func<bool> _checkGrounded;
    private readonly Config _config;
    private Vector3 _direction;

    public MoveHandler(Func<bool> checkGrounded, Config config)
    {
        _checkGrounded = checkGrounded ?? throw new ArgumentNullException(nameof(checkGrounded));
        _config = config;
    }

    public void UpdateDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public void Handle(ref Vector3 velocity)
    {
        Vector3 targetVelocity = _direction * _config.Speed;
        float controlFactor = _checkGrounded.Invoke() ? 1f : _config.AirControl;
        float timeFactor = _config.Acceleration * controlFactor * Time.deltaTime;
        
        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, timeFactor);
        
        if (_checkGrounded.Invoke())
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