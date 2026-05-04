using NaughtyAttributes;
using UnityEngine;

public class BackstepNode : StateNode
{
    [SerializeField] private StateNode onTargetLost;
    [SerializeField] private StateNode onGone;

    [Header("Options")]
    [SerializeField] private float goneDistance = 3f;
    [SerializeField] private float backstepModifier = 0.9f;

    [Foldout("Animations")] [SerializeField] private string BackstepId = "ReverseWalk";
    [Foldout("Animations")] [SerializeField] private string ArmsCombatIdleId = "CombatIdle";

    private float _defaultModifier;
    private Entity _target;

    public override void Enter()
    {
        _target = Controller.Detector.CurrentTarget;

        if (_target == null)
        {
            TransitionTo(onTargetLost);
            return;
        }

        _target.Dead += OnTargetDead;

        _defaultModifier = Controller.Movement.SpeedModifier;
        Controller.Movement.SetSpeedModifier(backstepModifier);
    }

    public override void UpdateState()
    {
        float distance = Vector2.Distance(transform.position, _target.transform.position);

        if (distance < goneDistance)
        {
            float direction = Mathf.Sign(transform.position.x - _target.transform.position.x);

            Controller.Movement.SetDirection(direction);
            Controller.Movement.HandleFlip(Mathf.Sign(_target.transform.position.x - transform.position.x));

            Controller.Animator.BodyAnimator.Play(BackstepId);
            Controller.Animator.ArmsAnimator.Play(ArmsCombatIdleId);
        }
        else
            TransitionTo(onTargetLost);
    }

    public override void Exit()
    {
        Controller.Movement.SetSpeedModifier(_defaultModifier);
        Controller.Movement.SetDirection(0);

        _target.Dead -= OnTargetDead;
    }

    private void OnTargetDead()
    {
        TransitionTo(onTargetLost);
    }
}