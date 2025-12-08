using UnityEngine;

public class NPCAnimation : CharacterAnimations
{
    public void SetTracker(NPCTracker npc)
    {
        SetVisual(npc.NPC);
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

}
