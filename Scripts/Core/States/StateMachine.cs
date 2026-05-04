using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IEntityState CurrentState { get; private set; }

    public void Initialize(IEntityState initialState)
    {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(IEntityState newState)
    {
        CurrentState?.Exit(); // Завершаем старое
        CurrentState = newState;
        CurrentState.Enter(); // Запускаем новое
    }
}