using NaughtyAttributes;
using UnityEngine;

public class ChaseNode : StateNode
{
    [SerializeField] private StateNode onGetTarget;
    [SerializeField] private StateNode onTargetLost;

    [Header("Chasing")]
    [SerializeField] private float stopChasingDistance = 15f;
    [SerializeField] private float startRunningDistance = 5f;
    [SerializeField] private float stopDistance = 3f;

    [Header("Modifiers")]
    [SerializeField] private float runModifier = 1.2f;

    [Foldout("Animations")] [SerializeField] private string RunId = "Run";
    [Foldout("Animations")] [SerializeField] private string WalkId = "Walk";

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
    }

    public override void UpdateState()
    {
        float distance = Vector2.Distance(transform.position, _target.transform.position);

        if (distance > stopChasingDistance)
        {
            TransitionTo(onTargetLost);
            Controller.Detector.ResetTarget();
            return;
        }
        else if (distance < stopDistance)
        {
            TransitionTo(onGetTarget);
            return;
        }
        else if (distance > startRunningDistance)
        {
            Controller.Movement.SetSpeedModifier(runModifier);

            if (distance > startRunningDistance + 0.1f)
                Controller.Animator.Play(RunId);
        }
        else
        {

            Controller.Movement.SetSpeedModifier(1);
            Controller.Animator.Play(WalkId);
        }

        float direction = Mathf.Sign(_target.transform.position.x - transform.position.x);
        Controller.Movement.SetDirection(direction);
        Controller.Movement.HandleFlip(direction);
    }

    public override void Exit()
    {
        Controller.Movement.SetSpeedModifier(1);
        Controller.Movement.SetDirection(0);
        _target.Dead -= OnTargetDead;
    }

    private void OnTargetDead()
    {
        TransitionTo(onTargetLost);
    }
}