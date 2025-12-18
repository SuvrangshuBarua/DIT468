using UnityEngine;

public class NPCAnimation : CharacterAnimations
{
    NPCTracker _tracker;

    public void SetTracker(NPCTracker npc)
    {
        _tracker = npc;
        _visual = npc.NPC;

        _idle = _tracker.IdleClip;
        _walk = _tracker.WalkingClip;
        _tracker.SubscribeToAnimationUpdate(SetAnimation);

        OnMovementChanged(npc.IsMoving);
        OnFacingChanged(npc.IsFacingLeft);

        npc.SubscribeToFacingLeft(OnFacingChanged);
        npc.SubscribeToMovementChange(OnMovementChanged);
    }

    public void OnMovementChanged(bool isMoving)
    {
        if (isMoving)
        {
            PlayMovement();
        }
        else
        {
            PlayIdle();
        }
    }

    public void OnFacingChanged(bool isFacingLeft)
    {
        _rederer.flipX = isFacingLeft;
    }

    public void SetSingleAnimation(ScriptableAnimationClip clip)
    {
        _isIdle = false;
        PlayClip(clip);
    }

    public void SetAnimation()
    {
        _idle = _tracker.IdleClip;
        _walk = _tracker.WalkingClip;

        if (_isIdle)
        {
            PlayClip(_idle);
        }
        else
        {
            PlayClip(_walk);
        }
    }

}
