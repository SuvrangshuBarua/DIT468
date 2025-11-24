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

    ScriptableDialogue _currentDialogue;
    ScriptableDialogue.DialogueOption _currentOption;
    List<DialogueLine> _currentLines;
    int _lineIndex;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
        _timeline = LoopingManagers.Instance.TimelineSystem;
        _knowledge = ConstantManagers.Instance.KnowledgeSystem;
    }

    public void SetDialogue(ScriptableDialogue dialogue)
    {
        _time.Pause("Dialogue");
        _currentDialogue = dialogue;
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

        _icon.sprite = _currentLines[0].Speaker.Icon;
        _dialogueText.RunLine(_currentLines[0].Line);
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
        if(_lineIndex < _currentLines.Count)
        {
            _icon.sprite = _currentLines[_lineIndex].Speaker.Icon;
            _dialogueText.RunLine(_currentLines[_lineIndex].Line);
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
    
}
