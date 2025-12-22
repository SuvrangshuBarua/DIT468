using UnityEngine;
using UnityEngine.Events;

public class NPCObject_Talk : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    
    bool _isTalking = false;
    

    public bool CanInteract()
    {
        return _npcData.NPC.Dialogue != null && !_isTalking;
    }
    
    public string GetInteractionPrompt()
    {    
        if (_npcData.NPC.Dialogue != null)
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
