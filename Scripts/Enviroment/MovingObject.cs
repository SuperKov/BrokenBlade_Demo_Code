using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Option")]
    [SerializeField] private bool startMoving;

    [Header("Moving")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private Vector3 direction = Vector3.right;

    private float _currentSpeed;
    private float _targetSpeed;

    private void Start()
    {
        if (startMoving)
        {
            _targetSpeed = maxSpeed;
            _currentSpeed = maxSpeed;
        }
    }

    private void Update()
    {
        Move();
    }

    public void StartMove()
    {
        _targetSpeed = maxSpeed;
    }

    public void StopMove()
    {
        _targetSpeed = 0;
    }

    private void Move()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * acceleration);
        target.position += direction * (_currentSpeed / 100);
    }
}