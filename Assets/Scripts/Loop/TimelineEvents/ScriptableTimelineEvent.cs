using UnityEngine;
using System.Collections.Generic;

// Used for any instances where something happens at a certain point in time
[CreateAssetMenu(fileName = "ScriptableTimelineEvent", menuName = "Scriptable Objects/TimelineEvent")]
public class ScriptableTimelineEvent : ScriptableTriggerEvent
{
    [SerializeField] int _timeOfEvent;
    [SerializeField] bool _skipOnLoad;

    public int TimeOfEvent { get => _timeOfEvent; }
    public bool SkipOnLoad { get => _skipOnLoad; }
    
}
