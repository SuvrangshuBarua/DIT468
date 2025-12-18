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

    NPCObject _currentNPCObject;
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
    }

    public void SetDialogue(ScriptableDialogue dialogue, NPCObject currentNPCObject)
    {
        _time.Pause("Dialogue");
        _currentDialogue = dialogue;
        _currentNPCObject = currentNPCObject;
        ToggleActive(true, false);
        PopulateOptions();
    }

    public void SelectDialogueOption(ScriptableDialogue.DialogueOption option)
    {
        _currentOption = option;
        ClearOptions();

        ToggleActive(false, true);
        
        _currentLines = option.Dialogue;
        _lineIndex = 0;

        DisplayLine();
    }

    void PopulateOptions()
    {
        foreach (ScriptableDialogue.DialogueOption option in _currentDialogue.AllOptions)
        {
            if (_timeline.IsTimelineValid(option.ChangesRequired) && _knowledge.HasKnowledge(option.KnowledgeRequired))
            {
                GameObject button = Instantiate(_optionPrefab, _optionParent);
                button.GetComponent<UI_DialogueOption>().Setup(option);
            }
        }
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
