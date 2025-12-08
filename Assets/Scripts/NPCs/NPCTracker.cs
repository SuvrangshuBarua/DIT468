using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NPCTracker
{
    ScriptableNPC _npc;
    ScriptableRoom _currentRoom;
    float _currentXPoint = 0;
    NPCManager _npcManager;
    
    ScriptableGossip _currentGossip;
    UnityEvent<bool> _hasGossip = new UnityEvent<bool>();

    bool _isMoving = false;
    bool _isFacingLeft = false;
    UnityEvent<bool> _onChangeMoving = new UnityEvent<bool>();
    UnityEvent<bool> _onChangeFacing = new UnityEvent<bool>();

    TimeSystem _time;
    Coroutine _npcMovement;

    public ScriptableNPC NPC { get => _npc; }
    public ScriptableRoom CurrentRoom { get => _currentRoom; }
    public ScriptableGossip CurrentGossip { get => _currentGossip; }
    public float CurrentXPoint { get => _currentXPoint; }
    public bool IsMoving { get => _isMoving; }
    public bool IsFacingLeft { get => _isFacingLeft; }

    public NPCTracker(ScriptableNPC npc)
    {
        _npc = npc;
        _currentRoom = _npc.StartingRoom;
        _time = LoopingManagers.Instance.TimeSystem;
        _npcManager = LoopingManagers.Instance.NPCManager;
    }


    IEnumerator MoveNPCOverTime(List<PathfindingSection> path, float speed)
    {
        _isMoving = true;
        _onChangeMoving.Invoke(true);

        foreach (PathfindingSection section in path)
        {
            if (_currentRoom != section.Room)
            {
                ScriptableRoom previousRoom = _currentRoom;
                _currentRoom = section.Room;
                _npcManager.OnNPCMovedRoom(this, previousRoom, _currentRoom);
            }

            _currentXPoint = section.StartXPos;

            _isFacingLeft = section.StartXPos > section.EndXPos;
            _onChangeFacing.Invoke(_isFacingLeft);

            float distance = Mathf.Abs(section.StartXPos - section.EndXPos);

            float time = distance / speed;
            float timePassed = 0;

            while (timePassed < time)
            {
                float t = timePassed / time;

                _currentXPoint = section.StartXPos * (1-t) + section.EndXPos * (t);

                timePassed += Time.deltaTime;
                
                yield return null;
            }            
        }


        _isMoving = false;
        _onChangeMoving.Invoke(false);
    }


    public void MoveNPC(ScriptableRoom destination, float finalPositionX, int timeToMove)
    {

        List<PathfindingSection> path = new List<PathfindingSection>();

        if(_currentRoom == destination)
        {
            path.Add(new PathfindingSection(_currentRoom, _currentXPoint, finalPositionX));
        }
        else
        {
            List<ScriptableRoom> roomsToVisit = CalculateRoomPath(_currentRoom, destination, new List<ScriptableRoom>());
            float transitionXPos = _currentRoom.Transitions.Find(x => x.ConnectingRoom == roomsToVisit[0]).XPositionInRoom;
            path.Add(new PathfindingSection(_currentRoom, _currentXPoint, transitionXPos));

            ScriptableRoom previousRoom = _currentRoom;
            for(int i = 0; i < roomsToVisit.Count; i++)
            {
                ScriptableRoom room = roomsToVisit[i];

                float cameFrom = room.Transitions.Find(x => x.ConnectingRoom == previousRoom).XPositionInRoom;
                float goingTo;
                if (i == roomsToVisit.Count - 1)
                {
                    goingTo = finalPositionX;
                }
                else
                {
                    goingTo = room.Transitions.Find(x => x.ConnectingRoom == roomsToVisit[i + 1]).XPositionInRoom;
                }

                path.Add(new PathfindingSection(room, cameFrom, goingTo));
            }
        }

        float distance = 0;
        foreach(PathfindingSection section in path)
        {
            distance += Mathf.Abs(section.StartXPos - section.EndXPos);
        }
        float speed = distance / timeToMove;

        _npcMovement = _npcManager.StartCoroutine(MoveNPCOverTime(path, speed));
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


    public void SubscribeToMovementChange(UnityAction<bool> action)
    {
        _onChangeMoving.AddListener(action);
    }


    public void SubscribeToFacingLeft(UnityAction<bool> action)
    {
        _onChangeFacing.AddListener(action);
    }

    public List<ScriptableRoom> CalculateRoomPath(ScriptableRoom currentRoom, ScriptableRoom destination, List<ScriptableRoom> visited)
    {
        List<ScriptableRoom> adjacentRooms = new List<ScriptableRoom>();

        foreach(var adjacent in currentRoom.Transitions)
        {
            ScriptableRoom adjRoom = adjacent.ConnectingRoom;
            if(adjRoom == destination)
            {
                visited.Add(adjRoom);
                return visited;
            }
        }

        visited.Add(currentRoom);
        int distance = 9999;
        List<ScriptableRoom> toReturn = null;
        foreach (ScriptableRoom adjRoom in adjacentRooms)
        {
            List<ScriptableRoom> found = CalculateRoomPath(adjRoom, destination, visited);

            if(found != null)
            {
                if(distance > found.Count)
                {
                    distance = found.Count;
                    toReturn = found;
                }
            }
        }

        return toReturn;
    }
    

    struct PathfindingSection
    {
        ScriptableRoom _room;
        float _startXPos;
        float _endXPos;

        public PathfindingSection(ScriptableRoom room, float startXPos, float endXPos)
        {
            _room = room;
            _startXPos = startXPos;
            _endXPos = endXPos;
        }

        public ScriptableRoom Room { get => _room; }
        public float StartXPos { get => _startXPos; }
        public float EndXPos { get => _endXPos; }
    }
}
