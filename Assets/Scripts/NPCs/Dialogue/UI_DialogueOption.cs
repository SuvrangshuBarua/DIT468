using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_DialogueOption : MonoBehaviour
{
    ScriptableDialogue.DialogueOption _option;
    UI_DialogueRunner _runner;

    [SerializeField] TextMeshProUGUI _prompt;
    [SerializeField] Image _icon;

    public void Setup(ScriptableDialogue.DialogueOption option)
    {
        _option = option;
        _runner = LoopingManagers.Instance.DialogueRunner;

        _icon.sprite = option.VisualRequired.Icon;
        _prompt.text = option.StartingLine;
    }

    public void OnClick()
    {
        _runner.SelectDialogueOption(_option);
    }
}
