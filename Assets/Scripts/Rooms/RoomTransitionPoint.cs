using UnityEngine;

// An interactable object that lets players change the active room
public class RoomTransitionPoint : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableRoom _roomToLoad;
    [SerializeField] int _doorIndex;
    [SerializeField] bool _denyEntry = false;
    
    public ScriptableRoom RoomToLoad { get => _roomToLoad; }
    public int DoorIndex { get => _doorIndex; }

    public bool CanInteract()
    {
        return !_denyEntry;
    }

    public string GetInteractionPrompt()
    {
        return (_denyEntry) ? "" : "Enter " + _roomToLoad.RoomName;
    }

    public virtual void OnInteract()
    {        
        LoopingManagers.Instance.RoomManager.ChangeRoom(_roomToLoad, _doorIndex);
    }
}
