using UnityEngine;

// A singleton used to reference any script that resets per loop
public class LoopingManagers : MonoBehaviour
{
    public static LoopingManagers Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] LoopSystem _loopSystem;
    public LoopSystem LoopSystem { get => _loopSystem; }

    [SerializeField] TimelineSystem _timelineSystem;
    public TimelineSystem TimelineSystem { get => _timelineSystem; }

    [SerializeField] TimeSystem _timeSystem;
    public TimeSystem TimeSystem { get => _timeSystem; }

    [SerializeField] RoomManager _roomManager;
    public RoomManager RoomManager { get => _roomManager; }

    [SerializeField] PlayerManager _player;
    public PlayerManager Player { get => _player; }
    
    [SerializeField] NPCManager _npcs;
    public NPCManager NPCManager { get => _npcs; }

    [SerializeField] UI_DialogueRunner _dialogue;
    public UI_DialogueRunner DialogueRunner { get => _dialogue; }

    [SerializeField] UI_GossipRunner _gossip;
    public UI_GossipRunner Gossip { get => _gossip; }
}
