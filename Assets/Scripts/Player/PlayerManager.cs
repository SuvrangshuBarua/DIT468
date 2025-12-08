using UnityEngine;

// Stores any player scripts that need to be accessed from other scripts
public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerObject { get => gameObject; }

    [SerializeField] PlayerInteraction _interaction;

    [SerializeField] EntityType _disquise;

    public PlayerInteraction Interaction { get => _interaction; }

    public EntityType Disguise { get => _disquise; }
}
