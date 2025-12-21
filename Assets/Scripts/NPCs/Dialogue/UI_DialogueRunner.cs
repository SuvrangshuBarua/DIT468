using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_DialogueRunner : MonoBehaviour
{
    [SerializeField] GameObject _closeButton;
    [SerializeField] GameObject _textDisplay;
    [SerializeField] GameObject _optionsDisplay;
    
    [SerializeField] Transform _optionParent;
    [SerializeField] GameObject _optionPrefab;

    [SerializeField] UI_TypewriterText _dialogueText;
    [SerializeField] Image _icon;

    TimelineSystem _timeline;
    KnowledgeSystem _knowledge;
    TimeSystem _time;
    UI_ShowTextBubble _textboxes;
    GameObject _player;
    DialogueStored _dialogueRead;
    QuestSystem _questSystem;

    GameObject _currentNPCObject;
    ScriptableDialogue _currentDialogue;
    ScriptableDialogue.DialogueOption _currentOption;
    List<DialogueLine> _currentLines;
    int _lineIndex;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
        _timeline = LoopingManagers.Instance.TimelineSystem;
        _knowledge = ConstantManagers.Instance.KnowledgeSystem;
        _textboxes = LoopingManagers.Instance.TextBubbles;
        _player = LoopingManagers.Instance.Player.gameObject;
        _dialogueRead = ConstantManagers.Instance.DialogueStored;
        _questSystem = ConstantManagers.Instance.QuestSystem;
    }

    public void SetDialogue(ScriptableDialogue dialogue, GameObject currentNPCObject)
    {
        _time.Pause("Dialogue");
        _currentDialogue = dialogue;
        _currentNPCObject = currentNPCObject;


        ScriptableDialogue.DialogueOption _startingLines = null;
        foreach (ScriptableDialogue.DialogueOption line in dialogue.AutoPlay)
        {
            if (CanChooseOption(line))
            {
                _startingLines = line;
                break;
            }
        }

        if (_startingLines != null)
        {
            ToggleActive(false, true);
            SelectDialogueOption(_startingLines);
        }
        else
        {
            ToggleActive(true, false);
            PopulateOptions();
        }
        
    }
    

    public void SelectDialogueOption(ScriptableDialogue.DialogueOption option)
    {
        _currentOption = option;
        ClearOptions();

        ToggleActive(false, true);
        
        _currentLines = option.Dialogue;
        _lineIndex = 0;

        if (!_dialogueRead.WasRead(option))
        {
            _dialogueRead.AddReadDialogue(option);
        }

        DisplayLine();
    }

    void PopulateOptions()
    {
        foreach (ScriptableDialogue.DialogueOption option in _currentDialogue.AllOptions)
        {
            if (CanChooseOption(option))
            {
                GameObject button = Instantiate(_optionPrefab, _optionParent);
                button.GetComponent<UI_DialogueOption>().Setup(option);
            }
        }

        if(_optionParent.childCount == 0)
        {
            CloseDialogue();
        }
    }

    bool CanChooseOption(ScriptableDialogue.DialogueOption option)
    {
        if (option.ChangesRequired.Count != 0 && !_timeline.IsTimelineValid(option.ChangesRequired))
        {
            return false;
        }

        if (option.KnowledgeRequired.Count != 0 && _knowledge.HasKnowledge(option.KnowledgeRequired))
        {
            return false;
        }

        if(option.VisualRequired != null && option.VisualRequired != DisguiseManager.Instance.CurrentCharacterVisual)
        {
            return false;
        }
        
        if(option.OneTimeOnly && _dialogueRead.WasRead(option))
        {
            return false;
        }

        if (option.ValidDuring.Count != 0  && !option.ValidDuring.Contains(_questSystem.GetActiveQuest))
        {
            return false;
        }

        return true;
    }

    void ToggleActive(bool options, bool dialogue)
    {
        _textDisplay.SetActive(dialogue);
        _optionsDisplay.SetActive(options);
        _closeButton.SetActive(options);
    }


    void ClearOptions()
    {
        foreach(Transform child in _optionParent)
        {
            Destroy(child.gameObject);
        }
    }
        
    public void CloseDialogue()
    {
        ClearOptions();
        ToggleActive(false, false);
        _time.Unpause("Dialogue");
    }   
    
    public void NextLine()
    {
        _lineIndex++;
        HidePreviousLine();
        if (_lineIndex < _currentLines.Count)
        {
            DisplayLine();
        }
        else
        {
            foreach(IEventChange change in _currentOption.OnDialogueComplete)
            {
                change.OnEventOccured();
            }

            ToggleActive(true, false);
            PopulateOptions();
        }
    }

    void HidePreviousLine()
    {
        DialogueLine line = _currentLines[_lineIndex - 1];
        GameObject _characterSpeaking = (line.SaidByPlayer) ? _player : _currentNPCObject.gameObject;

        _textboxes.RemoveText(_characterSpeaking);
    }

    void DisplayLine()
    {
        DialogueLine line = _currentLines[_lineIndex];

        GameObject _characterSpeaking = (line.SaidByPlayer) ? _player : _currentNPCObject.gameObject;

        _textboxes.SetText(_characterSpeaking, line.Line, Color.black);
    }
    
}
