using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableNPC", menuName = "Scriptable Objects/NPC")]
public class ScriptableNPC : ScriptableCharacterVisuals
{
    [SerializeField] ScriptableDialogue _dialogue;
    [SerializeField] ScriptableRoom _startingRoom;
    [SerializeField] Vector2 _startingPoint;
    [SerializeField] bool _skipOnLoad;

    public ScriptableRoom StartingRoom { get => _startingRoom;  }
    public Vector2 StartingPoint { get => _startingPoint; }
    public bool SkipOnLoad { get => _skipOnLoad; }
    public ScriptableDialogue Dialogue { get => _dialogue; }
}
