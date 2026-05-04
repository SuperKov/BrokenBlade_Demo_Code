public class PlayerAttackState : PlayerBaseState
{
    private bool _nextAttack;

    public PlayerAttackState(PlayerController context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        _nextAttack = false;
        context.Movement.BlockMoving = true;
        context.Combat.StartDelayedAttack();

        context.View.PlayAttack(context.Stats.Stats.Combat.AttackRate, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        context.View.PlayAttack(context.Stats.Stats.Combat.AttackRate);

        if (InputManager.Instance.Actions.Player.Attack.WasPressedThisFrame())
            _nextAttack = true;
    }

    public override void CheckSwitchStates()
    {
        if (context.Stats.IsRecovering)
        {
            context.Combat.StopAttack();
            SwitchState(stateFactory.Recovery);
        }
        else if (!context.Combat.IsAttacking)
        {
            if (!_nextAttack)
                SwitchState(stateFactory.Focus);
            else
                EnterState();
        }
    }

    public override void ExitState()
    {
        context.Movement.BlockMoving = false;
    }
}