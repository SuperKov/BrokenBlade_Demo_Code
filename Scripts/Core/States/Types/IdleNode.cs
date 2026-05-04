using NaughtyAttributes;
using UnityEngine;

public class IdleNode : StateNode
{
    [SerializeField] private StateNode onTargetFound;

    [Header("Optional")]
    [SerializeField] private bool ReturnToDefaultPosition = false;

    [Foldout("Animations")] [SerializeField] private string IdleId = "Idle";
    [Foldout("Animations")]
    [ShowIf(nameof(ReturnToDefaultPosition))] [SerializeField] private string WaklkId = "Walk";

    private Vector2 _defaultPos;

    private void Start()
    {
        if (ReturnToDefaultPosition)
            _defaultPos = transform.position;
    }

    public override void Enter()
    {
        if (Controller.Detector.CurrentTarget != null)
            TransitionTo(onTargetFound);
    }

    public override void UpdateState()
    {
        if (Controller.Detector.CurrentTarget != null)
        {
            TransitionTo(onTargetFound);
            return;
        }
        
        if (ReturnToDefaultPosition)
        {
            float distance = _defaultPos.x - transform.position.x;

            if (Mathf.Abs(distance) > 0.2f)
            {
                Controller.Animator.Play(WaklkId);

                Controller.Movement.SetDirection(Mathf.Sign(distance));
                Controller.Movement.HandleFlip(distance);
                return;
            }
            else
                Controller.Movement.SetDirection(0);
        }

        Controller.Animator.Play(IdleId);
    }

    public override void Exit()
    {
        if (ReturnToDefaultPosition)
            Controller.Movement.SetDirection(0);
    }
}