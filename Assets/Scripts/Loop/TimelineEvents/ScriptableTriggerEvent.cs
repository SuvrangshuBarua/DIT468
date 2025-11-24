using UnityEngine;
using System.Collections.Generic;

// Used for any instances where something happens at a certain point in time
[CreateAssetMenu(fileName = "ScriptableTimelineEvent", menuName = "Scriptable Objects/TriggerEvent")]
public class ScriptableTriggerEvent : ScriptableObject
{
    [SerializeField] List<EventBranch> _branches;
    
    public List<EventBranch> Branches { get => _branches; }

    [System.Serializable]
    public struct EventBranch 
    {
        [SerializeField] InspectableDictionary<ScriptableTimelineChange, bool> _changesRequired;
        [SerializeReference, SubclassSelector] IEventChange[] _outcome;

        public IEventChange[] Outcome { get => _outcome;  }
        public Dictionary<ScriptableTimelineChange, bool> ChangesRequired { get => _changesRequired.GetDictionary(); }
    }
}
