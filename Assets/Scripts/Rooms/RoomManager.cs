using UnityEngine;

// Loads and unloads rooms
public class RoomManager : MonoBehaviour
{
    [SerializeField] ScriptableRoom _startingRoom;
    [SerializeField] Vector2 _startingPosition;
    [SerializeField] Transform _roomParent;

    Transform _playerObject;

    ScriptableRoom _currentRoom = null;
    RoomInstance _currentRoomObject = null;

    public ScriptableRoom CurrentRoom { get => _currentRoom; }

    void Start()
    {
        _playerObject = LoopingManagers.Instance.Player.PlayerObject.transform;
        ChangeRoom(_startingRoom, 0);
    }

    public void ChangeRoom(ScriptableRoom newRoom, int doorIndex)
    {
        ScriptableRoom previousRoom = _currentRoom;
        Vector2 spawnPosition = _startingPosition;

        if (_currentRoom != null)
        {
            Destroy(_currentRoomObject.gameObject);
        }

        _currentRoom = newRoom;
        _currentRoomObject = Instantiate(newRoom.RoomPrefab, _roomParent).GetComponent<RoomInstance>();
        _currentRoomObject.Setup(_currentRoom);

        if(previousRoom != null)
        {
            spawnPosition = _currentRoomObject.GetPositionOfDoor(previousRoom, doorIndex);
        }

        _playerObject.position = spawnPosition;
    }

}
