using UnityEngine.Events;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onCutsceneStart = new();
    [SerializeField] private UnityEvent onCutsceneEnd = new();

    public void StartCutcene()
    {
        onCutsceneStart?.Invoke();
    }

    public void EndCutcene()
    {
        onCutsceneEnd?.Invoke();
    }
}