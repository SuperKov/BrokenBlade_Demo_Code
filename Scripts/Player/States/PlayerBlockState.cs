public class PlayerBlockState : PlayerBaseState
{
    public PlayerBlockState(PlayerController context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        context.StartBlocking();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        // Всегда смотрим на мышь
        UnityEngine.Vector3 mousePos = CameraManager.Instance.GetWorldMousePosition();
        float direction = mousePos.x > context.transform.position.x ? 1 : -1;

        context.Movement.HandleFlip(direction);

        context.View.PlayBlock();
    }

    public override void CheckSwitchStates()
    {
        if (context.Stats.IsRecovering)
            SwitchState(stateFactory.Recovery);
        else if (!InputManager.Instance.Actions.Player.Block.IsPressed() && context.IsFocusing)
            SwitchState(stateFactory.Focus);
        else if (!context.IsFocusing)
            SwitchState(stateFactory.Move);
    }

    public override void ExitState()
    {
        context.StopBlocking();
    }
}