public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        // При выходе из боя сбрасываем параметры камеры и блокировки
        CameraManager.Instance.SwitchFocus(false);
        CameraManager.Instance.ResetZoom();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        // Читаем ввод передвижения
        float moveInput = InputManager.Instance.Actions.Player.Move.ReadValue<UnityEngine.Vector2>().x;

        if (context.Movement.IsMovingBack())
            context.Movement.SetSpeedModifier(context.Stats.BackstepModifier);
        else if (context.IsRunning)
            context.Movement.SetSpeedModifier(context.Stats.SprintModifier);
        else
            context.Movement.SetSpeedModifier(1);

        context.Movement.SetDirection(moveInput);
        context.Movement.HandleFlip(context.Movement.VelocityX);

        context.View.PlayMove(context.Movement.VelocityX,
            context.IsRunning,
            false,
            context.Movement.IsMovingBack());
    }

    public override void CheckSwitchStates()
    {
        // Если нажата кнопка фокуса — переходим в боевой режим
        if (context.IsFocusing)
            SwitchState(stateFactory.Focus);

        // Если получили урон или вошли в стадию восстановления
        if (context.Stats.IsRecovering)
            SwitchState(stateFactory.Recovery);
    }

    public override void ExitState() { }
}