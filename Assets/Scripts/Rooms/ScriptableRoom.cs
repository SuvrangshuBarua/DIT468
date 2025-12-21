using UnityEngine;
using System.Collections.Generic;   

// Stores the data for the different rooms in the palace
[CreateAssetMenu(fileName = "ScriptableRoom", menuName = "Scriptable Objects/Room")]
public class ScriptableRoom : ScriptableObject
{
    [SerializeField] GameObject _roomPrefab;
    [SerializeField] List<RoomConnection> _roomPathfindingData;
    [SerializeField] float _npcYLevel;
    [SerializeField] string _roomName;

    public GameObject RoomPrefab { get => _roomPrefab; }

    [SerializeField] List<RoomType> _roomType;
    public List<RoomType> ValidNPCTypes { get => _roomType; }
        
    public List<RoomConnection> Transitions { get => _roomPathfindingData; }
    public float NpcYLevel { get => _npcYLevel; }

    public string RoomName { get => _roomName; }

    [System.Serializable]
    public struct RoomConnection
    {
        [SerializeField] float _xPositionInRoom;
        [SerializeField] ScriptableRoom _connectingRoom;

        public float XPositionInRoom { get => _xPositionInRoom; }
        public ScriptableRoom ConnectingRoom { get => _connectingRoom; }
    }
}

public enum RoomType
{
    Anyone,
    Servants,
    UpperClass,
    Residents,
}
