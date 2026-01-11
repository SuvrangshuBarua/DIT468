using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    [SerializeField] Transform _questContainer;
    [SerializeField] GameObject _taskElementPrefab;
    [SerializeField] Color _completeColor;
    [SerializeField] GameObject _completeButton;

    //[SerializeField] GameObject _background;

    [SerializeField] TextMeshProUGUI _questTitle;
    //[SerializeField] TextMeshProUGUI _taskDescription;

    private ScriptableQuest _currentQuest;
    private Dictionary<ScriptableTask, bool> _currentTasksStatus;
    private TimeSystem _time;
    
    public void DestroyChildrenInContainer()
    {
        foreach (Transform child in _questContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowQuestDisplay()
    {
        _currentQuest = ConstantManagers.Instance.QuestSystem.GetActiveQuest;
        _currentTasksStatus = ConstantManagers.Instance.QuestSystem.GetCurrentTasksStatus;

        _questTitle.text = "";

        DestroyChildrenInContainer();
        BuildQuestDisplay();
        _time = LoopingManagers.Instance.TimeSystem;
    }

    public void HideDisguiseDisplay()
    {
    }


    private void BuildQuestDisplay()
    {
        _questTitle.text = "";

        if (_currentQuest != null)
        {
            _questTitle.text = _currentQuest.QuestName;

            foreach (Transform child in _questContainer)
                Destroy(child.gameObject);

            bool allValid = true;

            foreach (ScriptableTask task in _currentQuest.TasksNeeded)
            {
                GameObject taskElementObj = Instantiate(_taskElementPrefab, _questContainer);
                Button button = taskElementObj.GetComponentInChildren<Button>();
                Image image = taskElementObj.GetComponentInChildren<Image>();
                TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
                label.text = task.TaskName;

                if (_currentTasksStatus[task] == true)
                {
                    label.fontStyle = FontStyles.Strikethrough;
                    image.color = _completeColor;
                }
                else
                {
                    allValid = false;
                }


                button.onClick.AddListener(() => OnShowTaskDescription(task.TaskName, task.TaskDescription));
            }

            _completeButton.SetActive(allValid);
        }
    }


    void OnShowTaskDescription(string title, string description)
    {
        LoopingManagers.Instance.InfoPopup.ShowPanel(title, description);
    }
}
