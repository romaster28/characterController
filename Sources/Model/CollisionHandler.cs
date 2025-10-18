using System;
using UnityEngine;

public class CollisionHandler
{
    private readonly LayerMask _ground;
    private readonly float _groundCheckDistance;
    private readonly float _forwardSurfaceDistance = .5f;
    private Vector3 _point;

    public CollisionHandler(LayerMask ground, float groundCheckDistance)
    {
        if (groundCheckDistance < 0)
            throw new ArgumentOutOfRangeException(nameof(groundCheckDistance));

        _ground = ground;
        _groundCheckDistance = groundCheckDistance;
    }

    public void UpdatePoint(Vector3 point)
    {
        _point = point;
    }

    public bool CheckGrounded()
    {
        return Physics.CheckSphere(_point, _groundCheckDistance, _ground);
    }

    public Vector3 GetNormal(Vector3 forward)
    {
        Vector3 resultNormal = Vector3.up;
        forward = forward.normalized;

        if (Physics.Raycast(_point, Vector3.down, out RaycastHit groundHit, _groundCheckDistance))
            resultNormal = groundHit.normal;

        if (Physics.Raycast(_point, forward, out RaycastHit forwardHit, _forwardSurfaceDistance))
            resultNormal = forwardHit.normal;

        return resultNormal;
    }
}