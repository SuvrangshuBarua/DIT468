using UnityEngine;
using System.Collections.Generic;

// Use to test the event system
[System.Serializable]
public class EventChange_MoveNPC : IEventChange
{
    [SerializeField] ScriptableNPC _npc;
    [SerializeField] ScriptableRoom _destination;
    [SerializeField] float _finalPosition;
    [SerializeField] int _timeToMove;

    public void OnEventOccured()
    {
        NPCManager allNpcs = LoopingManagers.Instance.NPCManager;
        NPCTracker tracker = allNpcs.GetNPC(_npc);
        tracker.MoveNPC(_destination, _finalPosition, _timeToMove);
    }
}
