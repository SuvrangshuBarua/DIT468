using UnityEngine;

// Add to any script that lets players interact with an object
// Requires a Collider2D set to trigger
public interface IInteractable
{
    void OnInteract();
    bool CanInteract();
    string GetInteractionPrompt();
}
