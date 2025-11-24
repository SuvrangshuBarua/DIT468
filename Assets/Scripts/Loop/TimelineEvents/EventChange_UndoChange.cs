using UnityEngine;

// Undo a change to the timeline
[System.Serializable]
public class EventChange_UndoChange : IEventChange
{
    [SerializeField] ScriptableTimelineChange _change;

    public void OnEventOccured()
    {
        LoopingManagers.Instance.TimelineSystem.UndoChange(_change);
    }
}
