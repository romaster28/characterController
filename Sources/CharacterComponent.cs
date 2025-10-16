using System;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    [SerializeField] private SerializableConfig _config;

    private CharacterModel _model;
    private TransformModel _transformModel;
    
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Physics.CheckSphere(transform.position, _config.GroundCheckRadius, _config.Ground) ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, _config.GroundCheckRadius);
    }

    private void TransformModelOnPositionUpdated(Vector3 position)
    {
        transform.localPosition = position;
    }
}
