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
    
    [SerializeField] UI_ShowTextBubble _textBubbles;
    public UI_ShowTextBubble TextBubbles { get => _textBubbles; }

    [SerializeField] CutsceneManager _cutscenes;
    public CutsceneManager CutsceneManager { get => _cutscenes; }

    [SerializeField] CameraManager _cameraManager;
    public CameraManager CameraManager { get => _cameraManager; }

    [SerializeField] ScreenTransition _transition;
    public ScreenTransition Transition { get => _transition; }

    [SerializeField] InfoPopup _infoPopup;
    public InfoPopup InfoPopup { get => _infoPopup; }
}
