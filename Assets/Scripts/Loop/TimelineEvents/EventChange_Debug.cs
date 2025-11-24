using UnityEngine;

// Use to test the event system
[System.Serializable]
public class EventChange_Debug : IEventChange
{
    [SerializeField] string _message;

    public void OnEventOccured()
    {
        Debug.Log(_message);
    }
}
