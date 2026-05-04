using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public InputSystem_Actions Actions {  get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        // Просто обращаемся к Instance, чтобы сработала логика создания
        var i = Instance;
    }

    protected override void Awake()
    {
        base.Awake();
        Actions = new InputSystem_Actions();
        Actions.Enable();
    }

    public void SwitchPlayerActions(bool mode)
    {
        if (mode)
            Actions.Player.Enable();
        else
            Actions.Player.Disable();
    }
}