using UnityEngine;
using System.Collections.Generic;

// Can be used for interactable objects in a level with simple conditions
public class GenericInteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] List<Interactions> _possibleInteractions;
    [SerializeField] string _defaultPrompt;

    TimelineSystem _timeline;

    private void Start()
    {
        _timeline = LoopingManagers.Instance.TimelineSystem;
    }

    public bool CanInteract()
    {
        return GetValidInteraction() != null;
    }

    public string GetInteractionPrompt()
    {
        Interactions currentInteraction = GetValidInteraction();

        if(currentInteraction == null)
        {
            return _defaultPrompt;
        }
        else
        {
            return currentInteraction.Prompt;
        }
    }

    public void OnInteract()
    {
        foreach (IEventChange change in GetValidInteraction().Outcome)
        {
            change.OnEventOccured();
        }
    }

    Interactions GetValidInteraction()
    {
        foreach(Interactions interaction in _possibleInteractions)
        {
            if (_timeline.IsTimelineValid(interaction.ChangesRequired))
            {
                return interaction;
            }
        }

        return null;
    }


    [System.Serializable]
    internal class Interactions
    {
        [SerializeField] InspectableDictionary<ScriptableTimelineChange, bool> _changesRequired;
        [SerializeReference, SubclassSelector] IEventChange[] _outcome;
        [SerializeField] string _prompt;

        public IEventChange[] Outcome { get => _outcome; }
        public Dictionary<ScriptableTimelineChange, bool> ChangesRequired { get => _changesRequired.GetDictionary(); }
        public string Prompt { get => _prompt; }
    }
}
