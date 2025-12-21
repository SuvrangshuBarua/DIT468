using UnityEngine;

// Information aboth the plot that the player can collect
[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableKnowledge", menuName = "Scriptable Objects/ScriptableKnowledge")]
public class ScriptableKnowledge : ScriptableObject
{
    [SerializeField] string _informationSummary;
    [SerializeField] KnowledgeProvider _knowledgeProvider;
    [SerializeField] Sprite _icon;
    
    public KnowledgeProvider KnowledgeProvider { get => _knowledgeProvider; }
    public Sprite Icon { get => _icon; }
    public string InformationSummary { get => _informationSummary; }
}

public enum KnowledgeProvider
{
    ProOne = 0,
    ProTwo = 1,
    ProThree = 2
}