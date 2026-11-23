using UnityEngine;

public class Interactable2D : MonoBehaviour
{
    [SerializeField] private string displayName = "陌生人";

    [SerializeField, TextArea(2, 5)]
    private string[] dialogueLines;

    public void Interact(DialogueUI dialogueUI)
    {
        dialogueUI.Open(displayName, dialogueLines);
    }
}
