using NaughtyAttributes;
using UnityEngine;

public class RecoveringNode : StateNode
{
    [SerializeField] private StateNode onStunEnd;

    [Foldout("Animations")] [SerializeField] private string RecoveringId = "TakeDamage";

    public override void Enter()
    {
        Controller.Animator.Play(RecoveringId);
        Controller.RecoveringStoped += OnRecoveringStoped;

        Controller.Movement.BlockMoving = true;
        // Controller.Animator?.Play("Hurt");
    }

    public override void UpdateState() { }

    public override void Exit()
    {
        Controller.RecoveringStoped -= OnRecoveringStoped;

        Controller.Movement.BlockMoving = false;
    }

    private void OnRecoveringStoped()
    {
        TransitionTo(onStunEnd);
    }
}