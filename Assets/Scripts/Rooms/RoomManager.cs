using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// Loads and unloads rooms
public class RoomManager : MonoBehaviour
{
    [SerializeField] ScriptableRoom _startingRoom;
    [SerializeField] Vector2 _startingPosition;
    [SerializeField] Transform _roomParent;
    [SerializeField] UnityEvent<ScriptableRoom> _onRoomSet = new UnityEvent<ScriptableRoom>();
    [SerializeField] float _fadeOutDuration;
    [SerializeField] float _fadeInDuration;


    Transform _playerObject;

    ScriptableRoom _currentRoom = null;
    RoomInstance _currentRoomObject = null;

    ScriptableRoom _roomToLoad;

    public ScriptableRoom CurrentRoom { get => _currentRoom; }

    void Start()
    {
        _playerObject = LoopingManagers.Instance.Player.PlayerObject.transform;
        _playerObject.gameObject.SetActive(true);
        _roomToLoad = _startingRoom;
        OnTransitioned();
    }



    public void ChangeRoom(ScriptableRoom newRoom, int doorIndex)
    {
        _roomToLoad = newRoom;
        LoopingManagers.Instance.TimeSystem.Pause("RoomChange");
        LoopingManagers.Instance.Transition.FadeOut(_fadeOutDuration, OnTransitioned);
    }

    void OnTransitioned()
    {
        ScriptableRoom previousRoom = _currentRoom;
        Vector2 spawnPosition = _startingPosition;

        if (_currentRoom != null)
        {
            _currentRoomObject.UnloadGameobjects();
            Destroy(_currentRoomObject.gameObject);
        }

        _currentRoom = _roomToLoad;
        _currentRoomObject = Instantiate(_currentRoom.RoomPrefab, _roomParent).GetComponent<RoomInstance>();
        _currentRoomObject.Setup(_currentRoom);

        if (previousRoom != null)
        {
            spawnPosition = _currentRoomObject.GetPositionOfDoor(previousRoom, 0);
        }

        _playerObject.position = spawnPosition;
        _onRoomSet.Invoke(_currentRoom);

        LoopingManagers.Instance.TimeSystem.Unpause("RoomChange");
        LoopingManagers.Instance.CameraManager.SetCameraPosition(spawnPosition + new Vector2(0,3));
        LoopingManagers.Instance.Transition.FadeIn(_fadeInDuration);
    }
    

    public void SubscribeToNewRoom(UnityAction<ScriptableRoom> action)
    {
        _onRoomSet.AddListener(action);
    }
}
