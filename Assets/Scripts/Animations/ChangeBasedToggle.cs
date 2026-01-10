using UnityEngine;
using System.Collections.Generic;

public class ChangeBasedToggle : MonoBehaviour
{
    [SerializeField] InspectableDictionary<ScriptableTimelineChange, bool> _changesRequired;
    [SerializeField] GameObject _toToggle;

    TimelineSystem _timeline;

    void Start()
    {
        _timeline = LoopingManagers.Instance.TimelineSystem;
        _timeline.SubscribeToChangeOccured(UpdateVisual);
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        bool state = _timeline.IsTimelineValid(_changesRequired.GetDictionary());
        _toToggle.SetActive(state);
    }
}
