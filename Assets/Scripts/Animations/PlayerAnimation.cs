using UnityEngine;

public class PlayerAnimation : CharacterAnimations
{
    [SerializeField] ScriptableCharacterVisuals _startingVisual;
    [SerializeField] PlayerMovement _movement;

    new protected void Start()
    {
        base.Start();
        SetVisual(_startingVisual);
        PlayIdle();
        _movement.SubsccribeToMovementChanged(OnMovementChanged);
    }
    
    void OnMovementChanged(int isMoving)
    {
        if (isMoving == 0)
        {
            PlayIdle();
        }
        else
        {
            _rederer.flipX = isMoving < 0;
            PlayMovement();
        }
    }
}
