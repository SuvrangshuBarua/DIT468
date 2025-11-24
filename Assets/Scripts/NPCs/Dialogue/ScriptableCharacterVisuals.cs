using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableDialogue", menuName = "Scriptable Objects/CharacterVisuals")]
public class ScriptableCharacterVisuals : ScriptableObject
{
    [SerializeField] string _characterName;
    [SerializeField] Sprite _icon;

    public string CharacterName { get => _characterName;  }
    public Sprite Icon { get => _icon; }
}
