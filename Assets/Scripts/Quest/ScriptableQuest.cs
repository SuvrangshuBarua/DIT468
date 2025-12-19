using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ScriptableQuest", menuName = "Scriptable Objects/ScriptableQuest")]
public class ScriptableQuest : ScriptableObject
{

    // for task and quest ID this looks nice but I dont really see the use for now : https://stackoverflow.com/questions/58984486/create-scriptable-object-with-constant-unique-id
    [SerializeField] int _questID;
    [SerializeField] string _questName;
    [SerializeField] string _questDescription;

    [SerializeField] List<ScriptableTask> _tasksNeeded;

    public int QuestID => _questID;
    public string QuestName => _questName;
    public string QuestDescription => _questDescription;
    public bool IsCompleted => !_tasksNeeded.Any(task => !task.IsCompleted);

    public List<ScriptableTask> TasksNeeded => _tasksNeeded;
}