using UnityEngine;

public class CharacterAnimations : AnimationPlayer
{
    protected ScriptableCharacterVisuals _visual;
    protected ScriptableAnimationClip _idle;
    protected ScriptableAnimationClip _walk;
    protected bool _isIdle = false;

    protected void Start()
    {
        SubscribeToAnimationComplete(PlayIdle);
    }

    protected void SetVisual(ScriptableCharacterVisuals visual)
    {
        _visual = visual;
        _idle = visual.DefaultIdle;
        _walk = visual.Walking;
    }

    protected void SetIdle(ScriptableAnimationClip newIdle)
    {
        _idle = newIdle;
    }

    protected void SetWalk(ScriptableAnimationClip newWalk)
    {
        _walk = newWalk;
    }

    protected void PlayIdle()
    {
        if (!_isIdle)
        {
            _isIdle = true;
            PlayClip(_idle);
        }
    }

    protected void PlayMovement()
    {
        _isIdle = false;
        PlayClip(_walk);
    }

    protected void PlayCustom(string key)
    {
        _isIdle = false;
        if (_visual.CustomClips.ContainsKey(key))
        {
            PlayClip(_visual.CustomClips[key]);
        }
    }
}
