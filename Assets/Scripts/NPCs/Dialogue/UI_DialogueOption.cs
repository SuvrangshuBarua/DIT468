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

        if(!ConstantManagers.Instance.DialogueStored.WasRead(option))
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
