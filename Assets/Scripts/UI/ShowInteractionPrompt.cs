using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowInteractionPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _prompt;

    [SerializeField] CanvasGroup _container;
    [SerializeField] List<RectTransform> _toUpdate;

    [SerializeField] GameObject _inputPrompt;

    PlayerInteraction _interactionSystem;
    TimelineSystem _timeline;

    IInteractable _lastInteraction = null;

    void Start()
    {
        _timeline = LoopingManagers.Instance.TimelineSystem;
        _interactionSystem = LoopingManagers.Instance.Player.Interaction;

        TimeSystem time = LoopingManagers.Instance.TimeSystem;
        time.SubscribeToPausedChanged(OnPause);
        OnPause(time.IsPaused);

        _timeline.SubscribeToChangeOccured(Refresh);
        _interactionSystem.SubscribeToNewInteraction(ShowPrompt);
        _interactionSystem.SubscribeToAfterInteraction(Refresh);
    }

    public void OnPause(bool pause)
    {
        _container.alpha = (pause || _lastInteraction == null) ? 0 : 1;
    }

    public void Refresh()
    {
        ShowPrompt(_lastInteraction);
    }

    public void ShowPrompt(IInteractable interaction)
    {
        if(interaction == null)
        {
            _prompt.text = "";
            _container.alpha = 0;
        }
        else
        {
            string prompt = interaction.GetInteractionPrompt();
            _prompt.text = prompt;

            _inputPrompt.SetActive(interaction.CanInteract());

            foreach (RectTransform trans in _toUpdate)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(trans);
            }
            _container.alpha = (prompt == "") ? 0 : 1;
        }


        _lastInteraction = interaction;
    }
}
