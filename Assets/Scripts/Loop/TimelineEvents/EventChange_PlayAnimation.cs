using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_PlayAnimation : IEventChange
{
    [SerializeField] ScriptableAnimationClip _overrideWalkAnimation;
    [SerializeField] ScriptableAnimationClip _overrideIdleAnimation;
    [SerializeField] ScriptableNPC _npc;
    [SerializeField] bool _facingLeft;

    public void OnEventOccured()
    {
        NPCManager npcs = LoopingManagers.Instance.NPCManager;
        NPCTracker tracker = npcs.GetNPC(_npc);
        tracker.SetFacingDirection(_facingLeft);
        tracker.SetAnimation(_overrideIdleAnimation, _overrideWalkAnimation);
    }
}
