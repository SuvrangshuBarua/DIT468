using UnityEngine;
using System.Collections.Generic;

// Stores the data for the different rooms in the palace
[CreateAssetMenu(fileName = "ScriptableRoom", menuName = "Scriptable Objects/Room")]
public class ScriptableRoom : ScriptableObject
{
    [SerializeField] GameObject _roomPrefab;
    [SerializeField] List<RoomConnection> _roomPathfindingData;
    [SerializeField] float _npcYLevel;

    public GameObject RoomPrefab { get => _roomPrefab; }
    public List<RoomConnection> Transitions { get => _roomPathfindingData; }
    public float NpcYLevel { get => _npcYLevel; }

    [System.Serializable]
    public struct RoomConnection
    {
        [SerializeField] float _xPositionInRoom;
        [SerializeField] ScriptableRoom _connectingRoom;

        public float XPositionInRoom { get => _xPositionInRoom; }
        public ScriptableRoom ConnectingRoom { get => _connectingRoom; }
    }
}
