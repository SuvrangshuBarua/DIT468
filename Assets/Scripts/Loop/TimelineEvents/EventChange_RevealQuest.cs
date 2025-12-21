using UnityEngine;

// Undo a change to the timeline
[System.Serializable]
public class EventChange_RevealQuest : IEventChange
{
    public void OnEventOccured()
    {
        ConstantManagers.Instance.QuestSystem.RevealNextQuest();
    }
}
