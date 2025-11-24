using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

// Manages the events that occur in the palace, and adapts based on player actions
public class TimelineSystem : MonoBehaviour
{
    List<ScriptableTimelineEvent> _pendingEvents;
    List<ScriptableTimelineChange> _changesMade = new List<ScriptableTimelineChange>();

    UnityEvent _onChangeOccured = new UnityEvent();

    // Get all events at the start
    private void Start()
    {
        AssetDatabase assets = ConstantManagers.Instance.AssetDatabase;
        if (assets.HasDatabaseLoaded)
        {
            SetEvents();
        }
        else
        {
            assets.SubscribeToLoadedDatabase(SetEvents);
        }
    }

    void SetEvents()
    {
        _pendingEvents = new List<ScriptableTimelineEvent>();
        _pendingEvents.AddRange(ConstantManagers.Instance.AssetDatabase.AllEvents);

        LoopingManagers.Instance.TimeSystem.SubscribeToTimeUpdate(OnTimeChanged);
    }

    // Add or remove changes that the player made to the timeline

    public void AddChange(ScriptableTimelineChange change)
    {
        if (!_changesMade.Contains(change))
        {
            _changesMade.Add(change);
            _onChangeOccured.Invoke();
        }
    }

    public void UndoChange(ScriptableTimelineChange change)
    {
        if (_changesMade.Contains(change))
        {
            _changesMade.Remove(change);
            _onChangeOccured.Invoke();
        }
    }

    // Checks if certain changes are present

    public bool IsTimelineValid(Dictionary<ScriptableTimelineChange, bool> requirements)
    {
        foreach(KeyValuePair<ScriptableTimelineChange, bool> requirement in requirements)
        {
            bool wasMade = WasChangeMade(requirement.Key);
            if(requirement.Value != wasMade)
            {
                return false;
            }
        }

        return true;
    }

    public bool WasChangeMade(ScriptableTimelineChange change)
    {
        return _changesMade.Contains(change);
    }

    
    // Finds wich events should occur at a certain time, and runs them
    public void OnTimeChanged(int time)
    {
        List<ScriptableTimelineEvent> toRemove = new List<ScriptableTimelineEvent>();
        foreach(ScriptableTimelineEvent timelineEvent in _pendingEvents)
        {
            if(timelineEvent.TimeOfEvent <= time)
            {
                RunEvent(timelineEvent);
                toRemove.Add(timelineEvent);
            }
        }

        foreach(ScriptableTimelineEvent timelineEvent in toRemove)
        {
            _pendingEvents.Remove(timelineEvent);
        }
    }

    // Triggers the right branch of an event
    public void RunEvent(ScriptableTriggerEvent triggerEvent)
    {
        foreach(ScriptableTriggerEvent.EventBranch branch in triggerEvent.Branches)
        {
            if (IsTimelineValid(branch.ChangesRequired))
            {
                foreach(IEventChange change in branch.Outcome)
                {
                    change.OnEventOccured();
                }

                return;
            }
        }
    }
    
    public void SubscribeToChangeOccured(UnityAction action)
    {
        _onChangeOccured.AddListener(action);
    }
}
