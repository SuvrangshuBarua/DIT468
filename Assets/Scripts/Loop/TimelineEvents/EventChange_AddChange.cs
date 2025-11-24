using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_AddChange : IEventChange
{
    [SerializeField] ScriptableTimelineChange _change;

    public void OnEventOccured()
    {
        LoopingManagers.Instance.TimelineSystem.AddChange(_change);
    }
}
