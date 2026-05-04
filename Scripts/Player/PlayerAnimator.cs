using NaughtyAttributes;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private FrameAnimator BodyAnimator;
    [SerializeField] private FrameAnimator ArmsAnimator;

    [Header("Controllers")]
    [SerializeField] private MoveController _moveController;

    [Header("Walk")]
    [SerializeField] private float _minVelocityToWalk = 1f;

    [Header("Idle")]
    [Foldout("Keys")] [SerializeField] private string IdleAnimationId = "Idle";

    [Header("Walk")]
    [Foldout("Keys")] [SerializeField] private string WalkAnimationId = "Walk";
    [Foldout("Keys")] [SerializeField] private string ReverseWalkAnimationId = "ReverseWalk";

    [Header("Punch")]
    [Foldout("Keys")] [SerializeField] private string IdlePunchAnimationId = "IdlePunch";
    [Foldout("Keys")] [SerializeField] private string WalkPunchAnimationId = "WalkPunch";
    [Foldout("Keys")] [SerializeField] private string PunchAnimationId = "Punch";

    [Header("Run")]
    [Foldout("Keys")] [SerializeField] private string RunAnimationId = "Run";

    [Header("TakeDamage")]
    [Foldout("Keys")][SerializeField] private string TakeDamageId = "TakeDamage";

    [Header("Block")]
    [Foldout("Keys")][SerializeField] private string BlockId = "Block";

    [Foldout("Events")][SerializeField] private string _stepEventId = "Step";

    private void OnEnable()
    {
        BodyAnimator.SpecialEvent += OnAnimationEvent;
        ArmsAnimator.SpecialEvent += OnAnimationEvent;
    }

    private void OnDisable()
    {
        BodyAnimator.SpecialEvent -= OnAnimationEvent;
        ArmsAnimator.SpecialEvent -= OnAnimationEvent;
    }

    public void Stop()
    {
        BodyAnimator.Stop();
        ArmsAnimator.Stop();
    }

    public void PlayMove(float velocity, bool isRunning, bool isFocusing, bool movingBack)
    {
        bool isMoving = Mathf.Abs(velocity) > _minVelocityToWalk;

        if (!isMoving)
        {
            BodyAnimator.Play(IdleAnimationId);
            ArmsAnimator.Play(isFocusing ? IdlePunchAnimationId : IdleAnimationId);
            return;
        }

        if (isRunning && !isFocusing)
        {
            BodyAnimator.Play(RunAnimationId);
            ArmsAnimator.Play(RunAnimationId);
        }
        else if (movingBack)
        {
            BodyAnimator.Play(ReverseWalkAnimationId);
            ArmsAnimator.Play(isFocusing ? WalkPunchAnimationId : WalkAnimationId);
        }
        else
        {
            BodyAnimator.Play(WalkAnimationId);
            ArmsAnimator.Play(isFocusing ? WalkPunchAnimationId : WalkAnimationId);
        }
    }

    public void PlayAttack(float rate, bool ignoreRepeat = false)
    {
        float speed = 1f / rate;
        BodyAnimator.Play(PunchAnimationId, speed, ignoreRepeat);
        ArmsAnimator.Play(PunchAnimationId, speed, ignoreRepeat);
    }

    public void PlayBlock()
    {
        BodyAnimator.Play(BlockId);
        ArmsAnimator.Play(BlockId);
    }

    public void PlayTakeDamage()
    {
        BodyAnimator.Play(TakeDamageId);
        ArmsAnimator.Play(TakeDamageId);
    }

    public void PlayGuardBreak()
    {
        // GuardBreak
    }

    private void OnAnimationEvent(string id)
    {
        if (id == _stepEventId)
            _moveController.CallStep();
    }
}