public class PlayerRecoveryState : PlayerBaseState
{
    public PlayerRecoveryState(PlayerController context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        context.Movement.SetDirection(0);
        context.Movement.BlockMoving = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        context.View.PlayTakeDamage();
    }

    public override void CheckSwitchStates()
    {
        if (!context.Stats.IsRecovering)
        {
            if (context.IsFocusing)
                SwitchState(stateFactory.Focus);
            else
                SwitchState(stateFactory.Move);
        }    
    }

    public override void ExitState()
    {
        context.Movement.BlockMoving = false;
    }
}