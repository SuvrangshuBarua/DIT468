using UnityEngine;

// Undo a change to the timeline
[System.Serializable]
public class EventChange_SetDialogue : IEventChange
{
    public void OnEventOccured()
    {
        LoopingManagers.Instance.TimeSystem.UnpauseAll();

        LoopingManagers.Instance.TimeSystem.StartTimer();

        //LoopingManagers.Instance.Transition.FadeIn(0);
    }
}
