using System;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    [SerializeField] private SerializableCharacterConfig _config;
    [Range(0, 360f)] [SerializeField] private float _angle;

    private CharacterModel _model;
    private IReadOnlyTransform _transformModel;

    public void UpdateAngle(float newAngle)
    {
        _angle = newAngle;
    }
    
    private void Awake()
    {
        _model = new CharacterModel(_config, transform.position);
        _transformModel = _model.Transform;
    }

    private void OnEnable()
    {
        _transformModel.PositionUpdated += TransformModelOnPositionUpdated;
    }

    private void OnDisable()
    {
        _transformModel.PositionUpdated -= TransformModelOnPositionUpdated;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _model.MoveByDirection(new Vector3(horizontal, 0, vertical));
        
        if (Input.GetKeyDown(KeyCode.Space))
            _model.TryJump();
    }

    private void FixedUpdate()
    {
        _model.TickSimulate();
        _model.RotateToAngle(_angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Physics.CheckSphere(transform.position, _config.GroundCheckRadius, _config.Ground) ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, _config.GroundCheckRadius);
        
        Gizmos.color = Color.blue;

        Vector3 position = _transformModel?.Position ?? transform.position;
        Func<Vector3, Vector3> calculateDirection = _transformModel == null ? transform.TransformDirection : _transformModel.TranslateDirection;
        Vector3 forward = _transformModel?.Forward ?? transform.forward;
        
        // Forward
        Vector3 toPoint = position + forward * 1;
        Gizmos.DrawLine(position, toPoint);
        Gizmos.DrawLine(toPoint, toPoint + calculateDirection(new Vector3(-1, 0, -1) * .5f));
        Gizmos.DrawLine(toPoint, toPoint + calculateDirection(new Vector3(1, 0, -1) * .5f));
    }

    private void TransformModelOnPositionUpdated(Vector3 position)
    {
        transform.localPosition = position;
    }
}
