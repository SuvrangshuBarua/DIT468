using UnityEngine;

// An interactable object that lets players change the active room
public class RoomTransitionPoint : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableRoom _roomToLoad;
    [SerializeField] int _doorIndex;

    public ScriptableRoom RoomToLoad { get => _roomToLoad; }
    public int DoorIndex { get => _doorIndex; }

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionPrompt()
    {
        return "Enter";
    }

    public void OnInteract()
    {        
        LoopingManagers.Instance.RoomManager.ChangeRoom(_roomToLoad, _doorIndex);
    }
}
