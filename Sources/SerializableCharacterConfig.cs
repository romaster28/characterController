using System;
using UnityEngine;

[Serializable]
public class SerializableCharacterConfig : ICharacterConfig
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _airControl;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _maxScopeAngle = 45;

    public float MoveSpeed => _moveSpeed;
    public float Acceleration => _acceleration;
    public float AirControl => _airControl;
    public float JumpHeight => _jumpHeight;
    public float Gravity => _gravity;
    public float GroundCheckRadius => _groundCheckRadius;
    public LayerMask Ground => _ground;
    public float MaxSlopeAngle => _maxScopeAngle;
}