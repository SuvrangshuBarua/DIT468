using UnityEngine;
using UnityEngine.Events;

public class NPCObject : MonoBehaviour
{
    NPCTracker _npcData;
    [SerializeField] UnityEvent<NPCTracker> _onTrackerSet;
    
    private void Update()
    {
        transform.position = new Vector3(_npcData.CurrentXPoint, _npcData.CurrentRoom.NpcYLevel) * 2;
    }

    public void Setup(NPCTracker data)
    {
        _npcData = data;
        _onTrackerSet.Invoke(data);
        
        transform.position = new Vector3(data.CurrentXPoint, data.CurrentRoom.NpcYLevel);
    }
}
