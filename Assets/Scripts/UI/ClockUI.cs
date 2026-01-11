using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockUI : MonoBehaviour
{
    [SerializeField] Image _clockImage;
    [SerializeField] int _maxTime;

    [SerializeField] GameObject _knowledgeIcon;
    [SerializeField] GameObject _questIcon;
    [SerializeField] GameObject _timelineIcon;
    [SerializeField] float _knowledgeDelay;
    
    void Start()
    {
        LoopingManagers.Instance.TimeSystem.SubscribeToTimeUpdate(OnTimeUpdated);
        LoopingManagers.Instance.TimelineSystem.SubscribeToChangeAdded(OnChangeMade);

        ConstantManagers.Instance.KnowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);
        ConstantManagers.Instance.QuestSystem.SubscribeToQuestUpdate(OnQuestUpdated);
    }

    void OnTimeUpdated(int passed)
    {
        float ratio = passed / (_maxTime * 1.0F);
        ratio = 1 -  Mathf.Clamp(ratio, 0, 1);
        _clockImage.fillAmount = ratio;
    }

    void OnKnowledgeGained(ScriptableKnowledge knowledge)
    {
        StartCoroutine(FlashIcon(_knowledgeIcon, _knowledgeDelay));
    }

    void OnChangeMade(ScriptableTimelineChange change)
    {
        if (change.ShowNotification)
        {
            StartCoroutine(FlashIcon(_timelineIcon, _knowledgeDelay));
        }
    }

    void OnQuestUpdated()
    {
        StartCoroutine(FlashIcon(_questIcon, _knowledgeDelay));
    }

    public void ResetLoop()
    {
        LoopingManagers.Instance.LoopSystem.Loop();
    }

    IEnumerator FlashIcon(GameObject icon, float delay)
    {
        icon.SetActive(true);
        yield return new WaitForSeconds(delay);
        icon.SetActive(false);

    }

}
