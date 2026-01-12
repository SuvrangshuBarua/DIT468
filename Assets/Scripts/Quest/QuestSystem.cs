using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestSystem : MonoBehaviour
{

    [SerializeField] List<ScriptableQuest> quests;

    int currentActiveQuest = -1;

    public ScriptableQuest GetActiveQuest => quests[currentActiveQuest];

    private Dictionary<ScriptableTask, bool> _activeQuestCompletionStatus = new Dictionary<ScriptableTask, bool>();

    public Dictionary<ScriptableTask, bool> GetCurrentTasksStatus => _activeQuestCompletionStatus;

    public bool NewQuestAvailable { get => _newTaskAvailable; }

    bool _newTaskAvailable = true;

    bool _firstSetup = true;

    UnityEvent _onQuestUpdated = new UnityEvent();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InitializeActiveQuestState();

        ConstantManagers.Instance.KnowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);

        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeAdded(OnChangeAddded);
        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeRemoved(OnChangeRemoved);
    }

    public void SetAfterNewLoop()
    {
        if (_firstSetup)
        {
            return;
        }

        RefreshTaskValidity();
        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeAdded(OnChangeAddded);
        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeRemoved(OnChangeRemoved);
    }

    private void OnDestroy()
    {
        ConstantManagers.Instance.KnowledgeSystem.UnsubscribeFromKnowledgeGained(OnKnowledgeGained);
        LoopingManagers.Instance.TimelineSystem.UnsubscribeFromChangeAdded(OnChangeAddded);
        LoopingManagers.Instance.TimelineSystem.UnsubscribeFromChangeRemoved(OnChangeRemoved);
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
        RefreshTaskValidity();
        _newTaskAvailable = false;
        _firstSetup = false;
        _onQuestUpdated.Invoke();
    }

    private void OnKnowledgeGained(ScriptableKnowledge knowledge)
    {
        OnUpdateMade();
    }

    private void OnChangeAddded(ScriptableTimelineChange change)
    {
        OnUpdateMade();
    }

    public void RefreshTaskValidity()
    {
        ScriptableQuest activeQuest = quests[currentActiveQuest];

        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                bool state = false;
                if(task.RequirementType == TaskRequirementType.Change)
                {
                    state = LoopingManagers.Instance.TimelineSystem.WasChangeMade(task.ChangesNeedToOccure);
                }
                else
                {
                    state = ConstantManagers.Instance.KnowledgeSystem.HasKnowledge(task.KnowledgeNeeded);
                }

                if(_activeQuestCompletionStatus[task] != state)
                {
                    _activeQuestCompletionStatus[task] = state;
                    _onQuestUpdated.Invoke();
                }            
            }
        }
    }

    public bool AllTasksCompleted()
    {
        ScriptableQuest activeQuest = quests[currentActiveQuest];

        if (activeQuest != null)
        {
            foreach (ScriptableTask task in activeQuest.TasksNeeded)
            {
                if (!_activeQuestCompletionStatus[task])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OnChangeRemoved(ScriptableTimelineChange change)
    {
        OnUpdateMade();
    }

    void OnUpdateMade()
    {
        if(currentActiveQuest < 0)
        {
            return;
        }

        RefreshTaskValidity();
        if (AllTasksCompleted())
        {
            _newTaskAvailable = true;
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

    public void SubscribeToQuestUpdate(UnityAction action)
    {
        _onQuestUpdated.AddListener(action);
    }
}
