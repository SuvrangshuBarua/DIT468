using UnityEngine;
using System.Collections.Generic;

// Use to test the event system
[System.Serializable]
public class EventChange_TeleportNPC : IEventChange
{
    [SerializeField] ScriptableNPC _npc;
    [SerializeField] ScriptableRoom _destination;
    [SerializeField] float _finalPosition;

    public void OnEventOccured()
    {
        NPCManager allNpcs = LoopingManagers.Instance.NPCManager;
        NPCTracker tracker = allNpcs.GetNPC(_npc);
        tracker.TeleportNPC(_destination, _finalPosition);
    }
}
