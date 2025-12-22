using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomTextUI : MonoBehaviour
{
    [SerializeField] GameObject _invalidRoom;
    [SerializeField] TextMeshProUGUI _roomName;
    [SerializeField] List<RectTransform> _toUpdate;
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
        if (room != null)
        {
            _roomName.text = room.RoomName;
            _invalidRoom.SetActive(!_disguise.CanEnterRoom(room));

            foreach (RectTransform trans in _toUpdate)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(trans);
            }
        }        
    }
}
