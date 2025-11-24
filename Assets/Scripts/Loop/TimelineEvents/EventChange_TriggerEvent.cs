using UnityEngine;

// Use to test the event system
[System.Serializable]
public class EventChange_TriggerEvent : IEventChange
{
    [SerializeField] ScriptableTriggerEvent _event;

    public void OnEventOccured()
    {
        LoopingManagers.Instance.TimelineSystem.RunEvent(_event);
    }
}
