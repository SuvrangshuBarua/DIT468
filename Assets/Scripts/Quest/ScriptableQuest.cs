using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableQuest", menuName = "Scriptable Objects/ScriptableQuest")]
public class ScriptableQuest : ScriptableObject
{
    [SerializeField] int _questID;
    [SerializeField] string _questName;
    [SerializeField] string _questDescription;

    [SerializeField] bool _isCompleted;

    [SerializeField] List<ScriptableTimelineChange> _changesNeedToHappen;
    [SerializeField] List<ScriptableKnowledge> _knowledgeNeeded;

    public int QuestID => _questID;
    public string QuestName => _questName;
    public string QuestDescription => _questDescription;
    public bool IsCompleted => _isCompleted;
}
