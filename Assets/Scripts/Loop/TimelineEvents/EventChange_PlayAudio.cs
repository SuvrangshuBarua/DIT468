using UnityEngine;
using System.Collections.Generic;

// Use to test the event system
[System.Serializable]
public class EventChange_PlayAudio : IEventChange
{
    [SerializeField] string _audioName;
    [SerializeField] List<ScriptableRoom> _roomsToPlayIn;

    public void OnEventOccured()
    {
        if(_roomsToPlayIn.Contains(LoopingManagers.Instance.RoomManager.CurrentRoom))
        {
            ConstantManagers.Instance.SoundManager.PlayOneShot(_audioName);
        }
    }
}
