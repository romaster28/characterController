using UnityEngine;

public interface ICharacterConfig
{
    float MoveSpeed { get; }
    float Acceleration { get; }
    float AirControl { get; }
    float JumpHeight { get; }
    float Gravity { get; }
    float GroundCheckRadius { get; }
    LayerMask Ground { get; }
}