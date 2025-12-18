using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] bool _saidByPlayer;
    [SerializeField] string _line;
    
    public string Line { get => _line; }
    public bool SaidByPlayer { get => _saidByPlayer; }
}
