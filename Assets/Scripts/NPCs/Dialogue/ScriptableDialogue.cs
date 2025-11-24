using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableDialogue", menuName = "Scriptable Objects/ScriptableDialogue")]
public class ScriptableDialogue : ScriptableObject
{
    [SerializeField] List<DialogueOption> _allOptions;

    public List<DialogueOption> AllOptions { get => _allOptions; }

    [System.Serializable]
    public class DialogueOption
    {
        [SerializeField] InspectableDictionary<ScriptableTimelineChange, bool> _changesRequired;
        [SerializeField] List<ScriptableKnowledge> _knowledgeRequired;
        [SerializeField] ScriptableCharacterVisuals _visualRequired;
        [SerializeField] string _startingLine;
        [SerializeField] List<DialogueLine> _dialogue; 
        [SerializeReference, SubclassSelector] IEventChange[] _onDialogueComplete;

        public Dictionary<ScriptableTimelineChange, bool> ChangesRequired { get => _changesRequired.GetDictionary();  }
        public List<ScriptableKnowledge> KnowledgeRequired { get => _knowledgeRequired; }
        public ScriptableCharacterVisuals VisualRequired { get => _visualRequired; }
        public string StartingLine { get => _startingLine; }
        public List<DialogueLine> Dialogue { get => _dialogue; }
        public IEventChange[] OnDialogueComplete { get => _onDialogueComplete; }
    }
    
}
