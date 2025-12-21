using UnityEngine;
using TMPro;

public class RoomTextUI : MonoBehaviour
{
    [SerializeField] GameObject _invalidRoom;
    [SerializeField] TextMeshProUGUI _roomName;
    DisguiseManager _disguise;

    void Start()
    {
        RoomManager roomMan = LoopingManagers.Instance.RoomManager;
        _disguise = LoopingManagers.Instance.Player.Disguise;
        roomMan.SubscribeToNewRoom(SetText);
        SetText(roomMan.CurrentRoom);
    }

    void SetText(ScriptableRoom room)
    {
        Debug.Log(room);
        if (room != null)
        {
            _roomName.text = room.RoomName;
            _invalidRoom.SetActive(!_disguise.CanEnterRoom(room));
        }        
    }
}
