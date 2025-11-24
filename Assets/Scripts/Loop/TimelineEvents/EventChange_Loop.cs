using UnityEngine;

// Triggers the loop to restart
[System.Serializable]
public class EventChange_Loop : IEventChange
{
    public void OnEventOccured()
    {
        LoopingManagers.Instance.LoopSystem.Loop();
    }
}
