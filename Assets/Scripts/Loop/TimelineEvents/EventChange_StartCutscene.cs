using UnityEngine;

// Adds a change to the timeline
[System.Serializable]
public class EventChange_StartCutscene : IEventChange
{
    [SerializeField] ScriptableCutscene _cutscene;

    public void OnEventOccured()
    {
        LoopingManagers.Instance.CutsceneManager.PlayCutscene(_cutscene);
    }
}
