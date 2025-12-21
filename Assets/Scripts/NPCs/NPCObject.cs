using UnityEngine;
using UnityEngine.Events;

public class NPCObject : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    bool _isPlayerClose = false;

    UI_ShowTextBubble _textUI;
    [SerializeField] GameObject _talkingIcon;
    bool _isTalking = false;

    [SerializeField] NPCDetection _detection;
    [SerializeField] UnityEvent<NPCTracker> _onTrackerSet;
    
    private void Update()
    {
        transform.position = new Vector3(_npcData.CurrentXPoint, _npcData.CurrentRoom.NpcYLevel) * 2;
    }

    public bool CanInteract()
    {
        return _npcData.NPC.Dialogue != null && !_isTalking;
    }
    
    public string GetInteractionPrompt()
    {    
        if (_npcData.NPC.Dialogue != null)
        {
            return "Talk";
        }

        return "";
    }

    public void OnInteract()
    {
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_npcData.NPC.Dialogue, gameObject);
    }

    public void Setup(NPCTracker data)
    {
        _npcData = data;
        _onTrackerSet.Invoke(data);

        _textUI = LoopingManagers.Instance.TextBubbles;
        _npcData.SubscribeToLineUpdated(OnLineSet);
        OnLineSet();

        _detection.Setup(_npcData);
        
        transform.position = new Vector3(data.CurrentXPoint, data.CurrentRoom.NpcYLevel);
    }

    public void OnLineSet()
    {
        if(_npcData.CurrentLine == "")
        {
            _isTalking = false;
            _textUI.RemoveText(gameObject);
            _talkingIcon.SetActive(false);
        }
        else
        {
            _isTalking = true;
            _textUI.SetText(gameObject, _npcData.CurrentLine, Color.black);
            _textUI.ToggleTextbox(gameObject, _isPlayerClose);
            _talkingIcon.SetActive(!_isPlayerClose);

            if(_isPlayerClose && _npcData.CurrentKnowledge != null)
            {
                ConstantManagers.Instance.KnowledgeSystem.AddKnowledge(_npcData.CurrentKnowledge);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = true;
            //player is in disquise, they can eavesdrop if there is gossip
            if (!_detection.GetSuspicious() && _isTalking)
            {
                _textUI.ToggleTextbox(gameObject, true);
                _talkingIcon.SetActive(false);

                if (_npcData.CurrentKnowledge != null)
                {
                    ConstantManagers.Instance.KnowledgeSystem.AddKnowledge(_npcData.CurrentKnowledge);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = false;

            _detection.CalmDown();

            if (_isTalking)
            {
                _textUI.ToggleTextbox(gameObject, false);
                _talkingIcon.SetActive(true);
            }
        }
    }
    
    private void OnDestroy()
    {
        if (_isTalking && _textUI != null)
        {
            _textUI.RemoveText(gameObject);
        }
    }
}
