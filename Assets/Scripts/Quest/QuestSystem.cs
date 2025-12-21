using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{

    [SerializeField] List<ScriptableQuest> quests;

    int currentActiveQuest = -1;

    public ScriptableQuest GetActiveQuest => quests[currentActiveQuest];

    private Dictionary<ScriptableTask, bool> _activeQuestCompletionStatus = new Dictionary<ScriptableTask, bool>();

    public Dictionary<ScriptableTask, bool> GetCurrentTasksStatus => _activeQuestCompletionStatus;

    public bool NewQuestAvailable { get => _newTaskAvailable; }

    bool _newTaskAvailable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InitializeActiveQuestState();

        ConstantManagers.Instance.KnowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);

        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeAdded(OnChangeAddded);
        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeRemoved(OnChangeRemoved);

    }

    private void OnDestroy()
    {
        ConstantManagers.Instance.KnowledgeSystem.UnsubscribeFromKnowledgeGained(OnKnowledgeGained);
        LoopingManagers.Instance.TimelineSystem.UnsubscribeFromChangeAdded(OnChangeAddded);
        LoopingManagers.Instance.TimelineSystem.UnsubscribeFromChangeRemoved(OnChangeRemoved);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeActiveQuestState()
    {
        _activeQuestCompletionStatus.Clear();
        ScriptableQuest activeQuest = quests[currentActiveQuest];
        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                _activeQuestCompletionStatus[task] = false;
            }
        }
        _newTaskAvailable = false;
    }

    private void OnKnowledgeGained(ScriptableKnowledge knowledge)
    {
        Debug.Log($"Knowledge gained: {knowledge.name}");

        // check if knowledge gained is part of the knowledge needed for the current quest
        ScriptableQuest activeQuest = quests[currentActiveQuest];
        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                if (!_activeQuestCompletionStatus[task] && 
                    task.RequirementType == TaskRequirementType.Knowledge &&
                    task.KnowledgeNeeded == knowledge)
                {
                    // mark task as completed, we skip change checking cause its task can only contain one of them.
                    Debug.Log($"Task completed: {task.name}");

                    _activeQuestCompletionStatus[task] = true;

                    //no need to update the ui cause it updates whenever we open it :)

                    // check if quest is completed
                    if (!_activeQuestCompletionStatus.ContainsValue(false))
                    {
                        Debug.Log($"Quest completed: {activeQuest.name}");
                        _newTaskAvailable = true;
                    }
                    break;
                }
            }
        }

    }

    private void OnChangeAddded(ScriptableTimelineChange change)
    {
        Debug.Log($"Change added: {change.name}");
        // check if change added is part of the changes needed for the current quest
        ScriptableQuest activeQuest = quests[currentActiveQuest];
        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                if (!_activeQuestCompletionStatus[task] &&
                    task.RequirementType == TaskRequirementType.Change &&
                    task.ChangesNeedToOccure == change)
                {

                    Debug.Log($"Task completed: {task.name}");

                    _activeQuestCompletionStatus[task] = true;

                    if (!_activeQuestCompletionStatus.ContainsValue(false))
                    {
                        Debug.Log($"Quest completed: {activeQuest.name}");
                        _newTaskAvailable = true;

                        //Here the loop "start" must be refreshed so that this state of the game is part of the timeline. (probably?)
                        

                    }
                    break;
                }
            }
        }
    }

    private void OnChangeRemoved(ScriptableTimelineChange change)
    {

        Debug.Log($"Change removed: {change.name}");
       
        ScriptableQuest activeQuest = quests[currentActiveQuest];
        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                if (_activeQuestCompletionStatus[task] &&
                    task.RequirementType == TaskRequirementType.Change &&
                    task.ChangesNeedToOccure == change)
                {
                    Debug.Log($"Task marked as incomplete: {task.name}");
                    _activeQuestCompletionStatus[task] = false;

                    break;
                }
            }
        }
    }


    public void RevealNextQuest()
    {
        if (_newTaskAvailable)
        {
            currentActiveQuest++;

            if (currentActiveQuest >= quests.Count)
            {
                //do something with that fact
                Debug.Log("All quests completed!");
            }

            InitializeActiveQuestState();
            _newTaskAvailable = false;
        }
    }
}
