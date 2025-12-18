using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_PlayAnimation : IEventChange
{
    [SerializeField] ScriptableAnimationClip _overrideWalkAnimation;
    [SerializeField] ScriptableAnimationClip _overrideIdleAnimation;
    [SerializeField] ScriptableNPC _npc;

    public void OnEventOccured()
    {
        NPCManager npcs = LoopingManagers.Instance.NPCManager;
        NPCTracker tracker = npcs.GetNPC(_npc);
        tracker.SetAnimation(_overrideIdleAnimation, _overrideWalkAnimation);
    }
}
