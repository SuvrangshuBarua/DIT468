using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueStored : MonoBehaviour
{
    List<ScriptableDialogue.DialogueOption> _dialogueRead = new List<ScriptableDialogue.DialogueOption>();
    
    public void AddReadDialogue(ScriptableDialogue.DialogueOption option)
    {
        _dialogueRead.Add(option);
    }

    public bool WasRead(ScriptableDialogue.DialogueOption option)
    {
        return _dialogueRead.Contains(option);
    }
}
