using UnityEngine;

public class Interactable_StaticDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] string _message;
    [SerializeField] ScriptableDialogue _dialogue;
    [SerializeField] Color _color;
    
    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionPrompt()
    {
        return  _message;
    }

    public virtual void OnInteract()
    {
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_dialogue, gameObject, _color);
    }
}
