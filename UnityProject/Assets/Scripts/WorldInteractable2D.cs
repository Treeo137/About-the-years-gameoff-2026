using UnityEngine;

public abstract class WorldInteractable2D : MonoBehaviour
{
    [SerializeField] private string objectId;
    [SerializeField] private string prompt = "Enter 调查";

    public string ObjectId => objectId;
    public virtual string Prompt => prompt;

    public virtual bool CanInteract => isActiveAndEnabled;

    public abstract void Interact();
}
