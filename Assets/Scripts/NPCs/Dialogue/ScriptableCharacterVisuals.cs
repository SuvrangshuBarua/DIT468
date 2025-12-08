using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableDialogue", menuName = "Scriptable Objects/CharacterVisuals")]
public class ScriptableCharacterVisuals : ScriptableObject
{
    [SerializeField] string _characterName;
    [SerializeField] Sprite _icon;
    [SerializeField] ScriptableAnimationClip _walking;
    [SerializeField] ScriptableAnimationClip _defaultIdle;
    [SerializeField] InspectableDictionary<string, ScriptableAnimationClip> _customClips;

    public string CharacterName { get => _characterName;  }
    public Sprite Icon { get => _icon; }
    public ScriptableAnimationClip Walking { get => _walking; }
    public ScriptableAnimationClip DefaultIdle { get => _defaultIdle; }
    public Dictionary<string, ScriptableAnimationClip> CustomClips { get => _customClips.GetDictionary(); }
}
