using UnityEngine;

// An interactable object that lets players change the active room
public class Interactable_StartLoop : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableRoom _roomToLoad;
    [SerializeField] string _message;
    
    public bool CanInteract()
    {
        // [TODO] check if the player talked to amis when a new quest is available
        return true;
    }

    public string GetInteractionPrompt()
    {
        return  _message;
    }

    public virtual void OnInteract()
    {
        LoopingManagers.Instance.TimeSystem.StartTimer();
        LoopingManagers.Instance.RoomManager.ChangeRoom(_roomToLoad, 0);
    }
}
