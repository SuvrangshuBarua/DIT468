using UnityEngine;
using System.Collections.Generic;

// Information aboth the plot that the player can collect
[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableKnowledge", menuName = "Scriptable Objects/ScriptableKnowledge")]
public class ScriptableKnowledge : ScriptableObject
{
    [SerializeField] private string _knowledgeName;
    [SerializeField] private string _informationSummary;
    //[SerializeField] private KnowledgeProvider _knowledgeProvider;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ScriptableKnowledge _dependentKnowledge;
    [SerializeField] Vector2 _boardPosition;
    [SerializeField] bool _hideFromBoard;
    [SerializeField] List<ScriptableKnowledge> _connections;

    public string KnowledgeName { get => _knowledgeName; }
    //public KnowledgeProvider KnowledgeProvider { get => _knowledgeProvider; }
    public Sprite Icon { get => _icon; }
    public string InformationSummary { get => _informationSummary; }
    public ScriptableKnowledge DependentKnowledge { get => _dependentKnowledge; }
    public Vector2 BoardPosition { get => _boardPosition; }
    public bool HideFromBoard { get => _hideFromBoard; }
    public List<ScriptableKnowledge> Connections { get => _connections; }
}

public enum KnowledgeProvider
{
    ProOne = 0,
    ProTwo = 1,
    ProThree = 2
}