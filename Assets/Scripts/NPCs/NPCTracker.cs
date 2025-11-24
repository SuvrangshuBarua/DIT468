using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NPCTracker
{
    ScriptableNPC _npc;
    ScriptableRoom _currentRoom;
    Vector2 _currentPoint;

    bool _isMoving;

    ScriptableGossip _currentGossip;
    UnityEvent<bool> _hasGossip = new UnityEvent<bool>();
    TimeSystem _time;

    public ScriptableNPC NPC { get => _npc; }
    public ScriptableRoom CurrentRoom { get => _currentRoom; }
    public ScriptableGossip CurrentGossip { get => _currentGossip; }

    public NPCTracker(ScriptableNPC npc)
    {
        _npc = npc;
        _currentRoom = _npc.StartingRoom;
        _time = LoopingManagers.Instance.TimeSystem;
    }


    IEnumerator MoveNPC()
    {
        yield return null;
    }

    public void SetGossip(ScriptableGossip gossip)
    {
        _currentGossip = gossip;
        _hasGossip.Invoke(true);
        _time.SetTimer(gossip.TalkingDuration, EndGossip);
    }

    public void EndGossip()
    {
        _currentGossip = null;
        _hasGossip.Invoke(false);
    }

    public void SubscribeToGossip(UnityAction<bool> action)
    {
        _hasGossip.AddListener(action);
    }
}
