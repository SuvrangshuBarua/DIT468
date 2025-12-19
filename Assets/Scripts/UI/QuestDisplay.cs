using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] Transform _questContainer;
    [SerializeField] GameObject _taskElementPrefab;
    [SerializeField] GameObject _background;

    [SerializeField] TextMeshProUGUI _questTitle;
    [SerializeField] TextMeshProUGUI _taskDescription;

    private ScriptableQuest _currentQuest;
    private Dictionary<ScriptableTask, bool> _currentTasksStatus;
    private TimeSystem _time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        _currentQuest = ConstantManagers.Instance.QuestSystem.GetActiveQuest;
        _currentTasksStatus = ConstantManagers.Instance.QuestSystem.GetCurrentTasksStatus;

        _questTitle.text = "";
        _taskDescription.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyChildrenInContainer()
    {
        foreach (Transform child in _questContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowQuestDisplay()
    {
        DestroyChildrenInContainer();
        _background.SetActive(true);
        BuildQuestDisplay();
        _time = LoopingManagers.Instance.TimeSystem;
        _time.Pause("QuestUI");
    }

    public void HideDisguiseDisplay()
    {
        _time.Unpause("QuestUI");
        _background.SetActive(false);
    }


    private void BuildQuestDisplay()
    {
        _questTitle.text = "";
        _taskDescription.text = "";

        if (_currentQuest != null)
        {
            _questTitle.text = _currentQuest.QuestName;

            foreach (Transform child in _questContainer)
                Destroy(child.gameObject);

            foreach (ScriptableTask task in _currentQuest.TasksNeeded)
            {
                GameObject taskElementObj = Instantiate(_taskElementPrefab, _questContainer);
                Button button = taskElementObj.GetComponentInChildren<Button>();
                TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
                label.text = task.TaskName;

                if (_currentTasksStatus[task] == true)
                {
                    label.fontStyle = FontStyles.Strikethrough;
                    label.color = Color.gray;
                    
                    //the button should still be interactable in case the player wants to remember something from the description.
                
                }


                button.onClick.AddListener(() => OnShowTaskDescription(task.TaskDescription));
            }
        }
    }


    void OnShowTaskDescription(string description)
    {
        _taskDescription.text = description;
    }
}
