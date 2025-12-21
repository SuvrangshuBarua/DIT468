using UnityEngine;

public class Interactable_StartLoop : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableRoom _roomToLoad;
    [SerializeField] string _message;
    
    public bool CanInteract()
    {
        return !ConstantManagers.Instance.QuestSystem.NewQuestAvailable;
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
