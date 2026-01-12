using UnityEngine;
using System.Collections.Generic;

public enum TaskRequirementType
{
    Knowledge,
    Change
}

[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableTask", menuName = "Scriptable Objects/ScriptableTask")]
public class ScriptableTask : ScriptableObject
{
    [SerializeField] int _taskID;
    [SerializeField] string _taskName;

    [TextArea(15, 20)]
    [SerializeField] string _taskDescription;

    // forcing either knowledge or change needed to happen to complete the task. This can be probably skipped later.
    [SerializeField] TaskRequirementType _taskRequirementType;

    [SerializeField] ScriptableTimelineChange _changesNeedToOccure;
    [SerializeField] List<ScriptableKnowledge> _knowledgeNeeded;

    public int TaskID => _taskID;
    public string TaskName => _taskName;
    public string TaskDescription => _taskDescription;

    public TaskRequirementType RequirementType => _taskRequirementType;

    public ScriptableTimelineChange ChangesNeedToOccure => _taskRequirementType == TaskRequirementType.Change ? _changesNeedToOccure : null;
    public List<ScriptableKnowledge> KnowledgeNeeded => _taskRequirementType == TaskRequirementType.Knowledge ? _knowledgeNeeded : null;
}
