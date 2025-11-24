using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject _prefabNPC;
    [SerializeField] Transform _npcParent;
    [SerializeField] Transform _doorsParent;

    NPCManager _npcManager;
    ScriptableRoom _currentRoom;
    

    void Start()
    {
        _npcManager = LoopingManagers.Instance.NPCManager;
        _currentRoom = LoopingManagers.Instance.RoomManager.CurrentRoom;
        
        if (_npcManager.AreNPCsLoaded)
        {
            SpawnAllNPCs();
        }
        else
        {
            _npcManager.SubscribeToNPCsLoaded(SpawnAllNPCs);
        }
    }

    void SpawnAllNPCs()
    {
        foreach (NPCTracker npc in _npcManager.GetNPCsInRoom(_currentRoom))
        {
            SpawnNPC(npc);
        }
    }

    void SpawnNPC(NPCTracker npc)
    {
        GameObject newNPC = Instantiate(_prefabNPC, _npcParent);
        newNPC.GetComponent<NPCObject>().Setup(npc);
    }


}
