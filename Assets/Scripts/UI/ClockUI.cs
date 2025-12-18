using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockUI : MonoBehaviour
{
    [SerializeField] Image _clockImage;
    [SerializeField] int _maxTime;

    [SerializeField] GameObject _knowledgeIcon;
    [SerializeField] float _knowledgeDelay;
    
    void Start()
    {
        LoopingManagers.Instance.TimeSystem.SubscribeToTimeUpdate(OnTimeUpdated);

        ConstantManagers.Instance.KnowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);
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
