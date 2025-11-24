using UnityEngine;

// Use to test the event system
[System.Serializable]
public class EventChange_Gossip : IEventChange
{
    [SerializeField] ScriptableGossip _gossip;

    public void OnEventOccured()
    {
        NPCManager allNpcs = LoopingManagers.Instance.NPCManager;

        foreach(ScriptableNPC npc in _gossip.NpcsTalking)
        {
            NPCTracker tracker = allNpcs.GetNPC(npc);
            tracker.SetGossip(_gossip);
        }
    }
}
