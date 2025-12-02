using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableDialogue", menuName = "Scriptable Objects/CharacterVisuals")]
public class ScriptableCharacterVisuals : ScriptableObject
{
    [SerializeField] string _characterName;
    [SerializeField] Sprite _icon;

    public string CharacterName { get => _characterName;  }
    public Sprite Icon { get => _icon; }

    [Header("Character Type")]
    [SerializeField] Character CharacterType;
    public Character GetCharacterType() => CharacterType;

    [Header("Acceptable Rooms For This Character")]
    [SerializeField] private List<RoomType> acceptableRoomTypes;
    public List<RoomType> GetAcceptableRoomTypes() => acceptableRoomTypes;

    public enum Character
    {
        Noll,
        Jeanne,
        businessOwner,
        Hair,
        Countess,
        Count,
        CrimeLord
    }

    // Checks if a character can enter the specified room example
    //public bool CanEnterRoom(ScriptableCharacterVisuals character, RoomType room)
    //{
    //    return character.GetAcceptableRoomTypes().Contains(room);
    //}
}
