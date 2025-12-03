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
        _currentCharacterVisual = newVisual;
    }

    // Checks if the player can be in the current room
    public bool CheckIfThePlayerShouldBeHere()
    {
        bool canEnter = CanEnterRoom(_currentCharacterVisual, LoopingManagers.Instance.RoomManager.CurrentRoom.RoomType);

        if (canEnter)
        {
            Debug.Log("Player can enter the room: " + LoopingManagers.Instance.RoomManager.CurrentRoom.RoomType);
        }
        else
        {
            Debug.LogWarning("Player cannot enter the room: " + LoopingManagers.Instance.RoomManager.CurrentRoom.RoomType);
        }

        return canEnter;
    }

    // Checks if the character can enter the specified room
    public bool CanEnterRoom(ScriptableCharacterVisuals character, RoomType room)
    {
        return character.GetAcceptableRoomTypes().Contains(room);
    }

    // Display Disguise UI
    public void DisplayDisguiseUI()
    {
        disguiseDisplay.GetComponent<DisguiseDisplay>().ShowDisguiseDisplay();
    }
}