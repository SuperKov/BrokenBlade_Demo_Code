using MainProject.Stats;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour, IMove
{
    public event System.Action StepTriggered;

    public bool BlockMoving;

    [Header("Rotation")]
    [SerializeField] protected Transform RotationTarget;
    [SerializeField] protected float RotaionDuration = 0.2f;
    [SerializeField] protected Ease FlipEase = Ease.OutBack;

    protected float _moveDirection;
    protected float _lookDirection;
    protected Rigidbody2D _rb;

    private Tweener _currentRotation;
    private MovementData _data;

    public float VelocityX => _rb.linearVelocityX;
    public float SpeedModifier { get; private set; } = 1f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lookDirection = RotationTarget.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    public bool IsMovingBack()
    {
        if (VelocityX == 0)
            return false;
        return Mathf.Sign(VelocityX) != Mathf.Sign(_lookDirection);
    }

    public void SetData(MovementData data) => _data = data;

    public void SetDirection(float direction) => _moveDirection = direction;

    public void SetSpeedModifier(float multiplier) => SpeedModifier = multiplier;

    public virtual void HandleMove()
    {
        if (_data == null) return;

        float targetSpeed;

        if (BlockMoving)
            targetSpeed = 0;
        else
            targetSpeed = _moveDirection * _data.Speed * SpeedModifier;

        _rb.linearVelocityX = Mathf.MoveTowards(
            _rb.linearVelocityX,
            targetSpeed,
            _data.Acceleration * SpeedModifier * Time.fixedDeltaTime
        );

        if (Mathf.Abs(_rb.linearVelocityX) < 0.01f)
        {
            _rb.linearVelocityX = 0;
        }

        /*
         float speed = dir * Stats.Movement.Speed;

        _currentSpeed = Mathf.MoveTowards(_currentSpeed,
            speed,
            Time.fixedDeltaTime * Stats.Movement.Acceleration);

        if (Mathf.Abs(_currentSpeed) < 0.01f)
            _currentSpeed = 0f;
        //_currentSpeed = Mathf.Clamp(_currentSpeed, -speed, speed);

        _rb.linearVelocity = new Vector2(_currentSpeed, _rb.linearVelocity.y);
         */
    }

    public void HandleFlip(float moveX)
    {
        float targetScaleX = moveX > 0 ? 1f : -1f;
        if (moveX == 0)
            targetScaleX = _lookDirection;

        if (targetScaleX != _lookDirection)
        {
            _lookDirection = targetScaleX;
            AnimateRotate(targetScaleX);
        }
    }

    public void CallStep()
    {
        StepTriggered?.Invoke();
    }

    private void AnimateRotate(float targetScaleX)
    {
        if (RotationTarget == null) return;

        if (_currentRotation != null)
            _currentRotation.Kill();

        _currentRotation = RotationTarget.DOScaleX(targetScaleX, RotaionDuration).SetEase(FlipEase);
    }
}