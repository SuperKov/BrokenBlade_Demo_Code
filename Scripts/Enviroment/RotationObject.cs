using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool enableOnStart = true;

    [Header("Rotation")]
    [SerializeField] private float startSpeed = 10f;
    [SerializeField] private float startAcceleration = 5f;
    [SerializeField] private Vector3 direction = Vector3.back;

    private float _targetSpeed = 0;
    private float _currentSpeed = 0;
    private float _acceleration = 0;

    private void Start()
    {
        _acceleration = startAcceleration;
        _targetSpeed = startSpeed;

        if (enableOnStart)
            _currentSpeed = startSpeed;
    }

    private void Update()
    {
        Rotate();
    }

    public void StartRotaion(float targretSpeed)
    {
        _targetSpeed = targretSpeed;
    }

    public void StopRotaion()
    {
        _targetSpeed = 0;
    }

    public void FastStopRotaion()
    {
        _targetSpeed = 0;
        _currentSpeed = 0;
    }

    public void SetAcceleration(float acceleration)
    {
        _acceleration = acceleration;
    }

    public void ResetAcceleration()
    {
        _acceleration = startAcceleration;
    }

    private void Rotate()
    {
        target.transform.localEulerAngles += direction * _currentSpeed;
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * _acceleration);
    }
}