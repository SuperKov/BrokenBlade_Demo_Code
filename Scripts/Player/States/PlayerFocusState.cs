public class PlayerFocusState : PlayerBaseState
{
    public PlayerFocusState(PlayerController context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        context.Movement.SetSpeedModifier(1);
        CameraManager.Instance.SwitchFocus(true);
        CameraManager.Instance.SetZoom(context.Stats.FocusZoom);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        float moveInput = InputManager.Instance.Actions.Player.Move.ReadValue<UnityEngine.Vector2>().x;

        UnityEngine.Vector3 mousePos = CameraManager.Instance.GetWorldMousePosition();
        float direction = mousePos.x > context.transform.position.x ? 1 : -1;

        context.Movement.SetDirection(moveInput);
        context.Movement.HandleFlip(direction);

        context.View.PlayMove(context.Movement.VelocityX,
            false,
            true,
            context.Movement.IsMovingBack());
    }

    public override void CheckSwitchStates()
    {
        if (context.Stats.IsRecovering) SwitchState(stateFactory.Recovery);
        else if (!context.IsFocusing) SwitchState(stateFactory.Move);
        else if (InputManager.Instance.Actions.Player.Attack.WasPressedThisFrame() && !context.Block.IsBlocking)
            SwitchState(stateFactory.Attack);
        else if (InputManager.Instance.Actions.Player.Block.IsPressed() && context.Block.CanBlock() && !context.Combat.IsAttacking)
            SwitchState(stateFactory.Block);
    }

    public override void ExitState() { }
}