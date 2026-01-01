using UnityEngine;
using System.Collections.Generic;

public class RoomInstance : MonoBehaviour
{
    [SerializeField] Transform _doorsParent;
    [SerializeField] private Collider2D _confinerCollider;
    [SerializeField] List<GameObject> _destroyBeforeUnload;

    List<RoomTransitionPoint> _roomTransitions;

    ScriptableRoom _room;
    
    public void Setup(ScriptableRoom room)
    {
        _room = room;

        _roomTransitions = new List<RoomTransitionPoint>();
        
        foreach(Transform trans in _doorsParent)
        {
            if(trans.gameObject.TryGetComponent(out RoomTransitionPoint door))
            {
                _roomTransitions.Add(door);
            }
        }
        
        LoopingManagers.Instance.CameraManager.SetupConfiner(_confinerCollider); 
    }

    public Vector2 GetPositionOfDoor(ScriptableRoom _connectingRoom, int index)
    {
        foreach(RoomTransitionPoint door in _roomTransitions)
        {
            if(door.DoorIndex == index && door.RoomToLoad == _connectingRoom)
            {
                return door.transform.position;
            }
        }

        return new Vector2();
    }

    public void UnloadGameobjects()
    {
        foreach(GameObject obj in _destroyBeforeUnload)
        {
            obj.SetActive(false);
            Destroy(obj);
        }
    }
}
