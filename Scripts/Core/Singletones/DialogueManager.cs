using UnityEngine;
using Yarn.Unity;

public class DialogueManager : Singleton<DialogueManager>
{
    private DialogueRunner _runner;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        // Просто обращаемся к Instance, чтобы сработала логика создания
        var i = Instance;
    }

    protected override void Awake()
    {
        base.Awake();

    }

    public void StartDialogue(string id)
    {
        if (_runner.IsDialogueRunning) return;
        
        InputManager.Instance.SwitchPlayerActions(false);

        _runner.StartDialogue(id);

        _runner.onDialogueComplete.AddListener(OnDialogueCompleted);
    }

    public void StopDialogue()
    {
        if (!_runner.IsDialogueRunning) return;

        InputManager.Instance.SwitchPlayerActions(true);
    }

    private void OnDialogueCompleted()
    {
        InputManager.Instance.SwitchPlayerActions(true);

        _runner.onDialogueComplete.RemoveListener(OnDialogueCompleted);
    }
}