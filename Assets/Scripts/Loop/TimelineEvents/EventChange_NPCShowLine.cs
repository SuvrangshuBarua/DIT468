using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_NPCShowLine : IEventChange
{
    [SerializeField] string _message;
    [SerializeField] ScriptableKnowledge _knowledge;
    [SerializeField] ScriptableNPC _npc;
    [SerializeField] ScriptableNPC _clearPreviousNpc;

    public void OnEventOccured()
    {
        NPCManager npcs = LoopingManagers.Instance.NPCManager;

        if(_clearPreviousNpc != null)
        {
            NPCTracker tracker = npcs.GetNPC(_clearPreviousNpc);
            tracker.ClearLine();
        }

        if(_npc != null)
        {
            NPCTracker tracker = npcs.GetNPC(_npc);
            tracker.SetLine(_message, _knowledge);
        }        
    }
}
