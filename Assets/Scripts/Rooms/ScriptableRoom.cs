using UnityEngine;
using System.Collections.Generic;

// Stores the data for the different rooms in the palace
[CreateAssetMenu(fileName = "ScriptableRoom", menuName = "Scriptable Objects/Room")]
public class ScriptableRoom : ScriptableObject
{
    [SerializeField] GameObject _roomPrefab;

    public GameObject RoomPrefab { get => _roomPrefab; }
}
