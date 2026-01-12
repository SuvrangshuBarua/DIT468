using UnityEngine;
using UnityEngine.Events;

public class NPCObject_Talk : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    UI_DialogueRunner _dialogue;
    
    bool _isTalking = false;

    private void Start()
    {
        _dialogue = LoopingManagers.Instance.DialogueRunner;
    }

    public bool CanInteract()
    {
        return _npcData.NPC.Dialogue != null && !_isTalking && _dialogue.CanTalk(_npcData.NPC.Dialogue) && !DisguiseManager.Instance.IsTransparent;
    }
    
    public string GetInteractionPrompt()
    {    
        if (CanInteract())
        {
            return "Talk";
        }

        return "";
    }

    public void OnInteract()
    {
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_npcData.NPC.Dialogue, gameObject, _npcData.NPC.TextColor);
    }

    public void Setup(NPCTracker data)
    {
        _npcData = data;
        
        _npcData.SubscribeToLineUpdated(OnLineSet);
        OnLineSet();
        
    }

    public void OnLineSet()
    {
        _isTalking = _npcData.CurrentLine != "";
    }    
}
