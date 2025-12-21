using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Xml.Serialization;

public class DisguiseManager : MonoBehaviour
{
    public static DisguiseManager Instance { get; private set; }

    [SerializeField] private List<ScriptableCharacterVisuals> availableDisguises;
    public List<ScriptableCharacterVisuals> AvailableDisguise => availableDisguises;

    [SerializeField] ScriptableCharacterVisuals _currentCharacterVisual;
    public ScriptableCharacterVisuals CurrentCharacterVisual { get => _currentCharacterVisual; }

    [SerializeField] private GameObject disguiseDisplay;

    UnityEvent<ScriptableCharacterVisuals> _onDisguiseChange= new UnityEvent<ScriptableCharacterVisuals>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }

    public void AddDisguise(ScriptableCharacterVisuals disguise)
    {
        if (!availableDisguises.Contains(disguise))
        {
            availableDisguises.Add(disguise);
        }
    }

    public void SetCurrentCharacterVisual(ScriptableCharacterVisuals newVisual)
    {
        print(newVisual);
        _currentCharacterVisual = newVisual;

        // Notify other systems about the change with events
        _onDisguiseChange.Invoke(_currentCharacterVisual);

    }

    // Checks if the player can be in the current room
    public bool CheckIfThePlayerShouldBeHere()
    {
        bool canEnter = CanEnterRoom(_currentCharacterVisual, LoopingManagers.Instance.RoomManager.CurrentRoom);

        if (canEnter)
        {
            Debug.Log("Player can enter the room: " + LoopingManagers.Instance.RoomManager.CurrentRoom.ValidNPCTypes);
        }
        else
        {
            Debug.LogWarning("Player cannot enter the room: " + LoopingManagers.Instance.RoomManager.CurrentRoom.ValidNPCTypes);
        }

        return canEnter;
    }

    // Checks if the character can enter the specified room
    public bool CanEnterRoom(ScriptableCharacterVisuals character, ScriptableRoom room)
    {
        return room.ValidNPCTypes.Contains(character.AcceptableRoomType);
    }

    public bool CanEnterRoom(ScriptableRoom room)
    {
        return CanEnterRoom( _currentCharacterVisual, room);
    }

    // Display Disguise UI
    public void DisplayDisguiseUI()
    {
        disguiseDisplay.GetComponent<DisguiseDisplay>().ShowDisguiseDisplay();
    }

    public void SubscribeToDisguiseChanged(UnityAction<ScriptableCharacterVisuals> action)
    {
        _onDisguiseChange.AddListener(action);
    }
}