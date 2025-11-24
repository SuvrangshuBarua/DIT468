using UnityEngine;

// Information aboth the plot that the player can collect
[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableKnowledge", menuName = "Scriptable Objects/ScriptableKnowledge")]
public class ScriptableKnowledge : ScriptableObject
{
    [SerializeField] string _informationSummary;

    public string InformationSummary { get => _informationSummary; }
}
