using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_DialogueOption : MonoBehaviour
{
    ScriptableDialogue.DialogueOption _option;
    UI_DialogueRunner _runner;

    [SerializeField] TextMeshProUGUI _prompt;
    [SerializeField] Image _background;
    [SerializeField] Color _newColor;

    public void Setup(ScriptableDialogue.DialogueOption option)
    {
        _option = option;
        _runner = LoopingManagers.Instance.DialogueRunner;

        bool hasChange = false;
        foreach(var outcome in option.OnDialogueComplete)
        {
            if(outcome is EventChange_AddChange)
            {
                hasChange = true;
                break;
            }
        }

        if(!ConstantManagers.Instance.DialogueStored.WasRead(option) || hasChange)
        {
            _background.color = _newColor;
        }

        _prompt.text = option.StartingLine;
    }

    public void OnClick()
    {
        _runner.SelectDialogueOption(_option);
    }
}
