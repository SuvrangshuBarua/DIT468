using UnityEngine;

// Stores any player scripts that need to be accessed from other scripts
public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerObject { get => gameObject; }

    [SerializeField] PlayerInteraction _interaction;
    public PlayerInteraction Interaction { get => _interaction; }
}
