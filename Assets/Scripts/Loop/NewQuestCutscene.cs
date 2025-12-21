using UnityEngine;
using System.Collections;

public class NewQuestCutscene : MonoBehaviour
{
    [SerializeField] ScriptableDialogue _startingLines;
    [SerializeField] float _delay;

    private void Start()
    {
        QuestSystem quests = ConstantManagers.Instance.QuestSystem;
        if (quests.NewQuestAvailable)
        {
            StartCoroutine(DelayCutscene());
        }
    }

    IEnumerator DelayCutscene()
    {
        LoopingManagers.Instance.TimeSystem.Pause("NewQuest");
        yield return new WaitForSeconds(_delay);

        LoopingManagers.Instance.TimeSystem.Unpause("NewQuest");
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_startingLines, gameObject);
    }
}
