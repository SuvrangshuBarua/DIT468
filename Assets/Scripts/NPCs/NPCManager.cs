using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public bool AreNPCsLoaded { get; private set; }
    UnityEvent _onNPCsLoaded = new UnityEvent();

    List<NPCTracker> _allNPCs = new List<NPCTracker>();

    void Start()
    {
        AssetDatabase assets = ConstantManagers.Instance.AssetDatabase;
        if (assets.HasDatabaseLoaded)
        {
            PopulateNPCs();
        }
        else
        {
            assets.SubscribeToLoadedDatabase(PopulateNPCs);
        }
    }
    
    void PopulateNPCs()
    {
        foreach(ScriptableNPC npc in ConstantManagers.Instance.AssetDatabase.AllNPCs)
        {
            _allNPCs.Add(new NPCTracker(npc));
        }
        AreNPCsLoaded = true;
        _onNPCsLoaded.Invoke();
    }

    public List<NPCTracker> GetNPCsInRoom(ScriptableRoom room)
    {
        List<NPCTracker> npcsInRoom = new List<NPCTracker>();
        foreach (NPCTracker npc in _allNPCs)
        {
            if(npc.CurrentRoom == room)
            {
                npcsInRoom.Add(npc);
            }
        }
        return npcsInRoom;
    }

    public NPCTracker GetNPC(ScriptableNPC npc)
    {
        foreach (NPCTracker tracker in _allNPCs)
        {
            if (tracker.NPC == npc)
            {
                return tracker;
            }
        }
        return null;
    }

    public void MoveNPC(ScriptableNPC npc, ScriptableRoom destination, int finalPositionX, int timeToMove)
    {

    }

    public void SubscribeToNPCsLoaded(UnityAction action)
    {
        _onNPCsLoaded.AddListener(action);
    }
}
