using UnityEngine;

public class Interactable_StaticDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] string _message;
    [SerializeField] ScriptableDialogue _dialogue;
    [SerializeField] Color _color;
    
    public bool CanInteract()
    {
        return LoopingManagers.Instance.DialogueRunner.CanTalk(_dialogue);
    }

    public string GetInteractionPrompt()
    {
        return CanInteract() ? _message : "A letter";
    }

    public virtual void OnInteract()
    {
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_dialogue, gameObject, _color);
    }
}
