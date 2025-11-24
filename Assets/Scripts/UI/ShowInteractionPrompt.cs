using UnityEngine;
using TMPro;

public class ShowInteractionPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _prompt;

    [SerializeField] CanvasGroup _container;

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
        _container.alpha = (pause) ? 0 : 1;
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
        }
        else
        {
            string prompt = interaction.CanInteract() ? "[E] " : "";
            prompt += interaction.GetInteractionPrompt();
            _prompt.text = prompt;
        }


        _lastInteraction = interaction;
    }
}
