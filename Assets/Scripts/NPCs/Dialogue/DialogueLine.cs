using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] ScriptableCharacterVisuals _speaker;
    [SerializeField] string _line;

    public ScriptableCharacterVisuals Speaker { get => _speaker; }
    public string Line { get => _line; }
}
