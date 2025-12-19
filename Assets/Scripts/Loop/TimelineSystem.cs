using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

// Manages the events that occur in the palace, and adapts based on player actions
public class TimelineSystem : MonoBehaviour
{
    List<ScriptableTimelineEvent> _pendingEvents;
    List<ScriptableTimelineChange> _changesMade = new List<ScriptableTimelineChange>();

    UnityEvent _onChangeOccured = new UnityEvent();

    //Context aware callbacks. created two distinct callbacks to make sure that the correct one is called when dealing with quests.
    UnityEvent<ScriptableTimelineChange> _onChangeAddded = new UnityEvent<ScriptableTimelineChange>();
    UnityEvent<ScriptableTimelineChange> _onChangeRemoved = new UnityEvent<ScriptableTimelineChange>();

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
            _onChangeAddded.Invoke(change);
        }
    }

    public void UndoChange(ScriptableTimelineChange change)
    {
        if (_changesMade.Contains(change))
        {
            _changesMade.Remove(change);
            _onChangeOccured.Invoke();
            _onChangeRemoved.Invoke(change);
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
    
    //Subscribe
    public void SubscribeToChangeOccured(UnityAction action)
    {
        _onChangeOccured.AddListener(action);
    }

    public void SubscribeToChangeAdded(UnityAction<ScriptableTimelineChange> action)
    {
        _onChangeAddded.AddListener(action);
    }
    public void SubscribeToChangeRemoved(UnityAction<ScriptableTimelineChange> action)
    {
        _onChangeRemoved.AddListener(action);
    }

    //Unsubscribe
    public void UnsubscribeFromChangeOccured(UnityAction action)
    {
        _onChangeOccured.RemoveListener(action);
    }
    public void UnsubscribeFromChangeAdded(UnityAction<ScriptableTimelineChange> action)
    {
        _onChangeAddded.RemoveListener(action);
    }
    public void UnsubscribeFromChangeRemoved(UnityAction<ScriptableTimelineChange> action)
    {
        _onChangeRemoved.RemoveListener(action);
    }

}
