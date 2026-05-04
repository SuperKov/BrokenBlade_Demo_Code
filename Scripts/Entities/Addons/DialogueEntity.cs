using UnityEngine;

public class DialogueEntity : MonoBehaviour, IUsable
{
    [SerializeField] private string Dialogue;

    public void Use()
    {
        DialogueManager.Instance.StartDialogue(Dialogue);
    }
}