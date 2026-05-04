using UnityEngine;

public abstract class StateNode : MonoBehaviour, IEntityState
{
    protected EntityController Controller;

    public void Initialize(EntityController controller)
    {
        Controller = controller;
    }

    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();


    protected void TransitionTo(StateNode nextState)
    {
        if (nextState != null)
            Controller.StateMachine.ChangeState(nextState);
        else
            Debug.LogWarning($"[AI] {gameObject.name}: Попытка перехода в пустое состояние из {GetType().Name}");
    }
}
