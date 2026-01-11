using UnityEngine;
using UnityEngine.Events;

public class NPCObject_Listen : MonoBehaviour
{
    NPCTracker _npcData;
    bool _isPlayerClose = false;

    UI_ShowTextBubble _textUI;
    [SerializeField] GameObject _talkingIcon;
    bool _isTalking = false;
    
    public void OnInteract()
    {
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_npcData.NPC.Dialogue, gameObject, _npcData.NPC.TextColor);
    }

    public void Setup(NPCTracker data)
    {
        _npcData = data;

        _textUI = LoopingManagers.Instance.TextBubbles;
        _npcData.SubscribeToLineUpdated(OnLineSet);
        OnLineSet();        
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
            _textUI.SetText(gameObject, _npcData.CurrentLine, _npcData.NPC.TextColor, false);
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
            if (_isTalking)
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
