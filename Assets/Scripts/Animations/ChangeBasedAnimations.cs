using UnityEngine;
using System.Collections.Generic;

public class ChangeBasedAnimations : AnimationPlayer
{
    [SerializeField] List<PossibleVisual> _versions;
    [SerializeField] ScriptableAnimationClip _defaultClip;
    [SerializeField] Sprite _defaultSprite;

    TimelineSystem _timeline;

    void Start()
    {
        _timeline = LoopingManagers.Instance.TimelineSystem;
        _timeline.SubscribeToChangeOccured(UpdateVisual);
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        foreach(PossibleVisual visual in _versions)
        {
            if (_timeline.IsTimelineValid(visual.ChangesRequired))
            {
                if(visual.ClipToPlay == null)
                {
                    _rederer.sprite = visual.StaticSprite;
                }
                else
                {
                    PlayClip(visual.ClipToPlay);
                }

                return;
            }
        }

        if (_defaultClip == null)
        {
            _rederer.sprite = _defaultSprite;
        }
        else
        {
            PlayClip(_defaultClip);
        }
    }

    [System.Serializable]
    struct PossibleVisual
    {
        [SerializeField] InspectableDictionary<ScriptableTimelineChange, bool> _changesRequired;
        [SerializeField] ScriptableAnimationClip _clipToPlay;
        [SerializeField] Sprite _staticSprite;

        public Dictionary<ScriptableTimelineChange, bool> ChangesRequired { get => _changesRequired.GetDictionary();  }
        public ScriptableAnimationClip ClipToPlay { get => _clipToPlay; }
        public Sprite StaticSprite { get => _staticSprite; }
    }
}
