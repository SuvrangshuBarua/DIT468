using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_SetNPCDetection : IEventChange
{
    [SerializeField] ScriptableNPC _npc;
    [SerializeField] bool _detectPlayer;

    public void OnEventOccured()
    {
        NPCManager npcs = LoopingManagers.Instance.NPCManager;
        NPCTracker tracker = npcs.GetNPC(_npc);
        tracker.TriggersDetection = _detectPlayer;
    }
}
