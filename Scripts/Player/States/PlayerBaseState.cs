public abstract class PlayerBaseState
{
    protected PlayerController context;
    protected PlayerStateFactory stateFactory;

    public PlayerBaseState(PlayerController controller, PlayerStateFactory factory)
    {
        context = controller;
        stateFactory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();
        newState.EnterState();
        stateFactory.CurrentState = newState;
    }
}