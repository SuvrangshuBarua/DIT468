using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableNPC", menuName = "Scriptable Objects/NPC")]
public class ScriptableNPC : ScriptableCharacterVisuals
{
    [SerializeField] ScriptableDialogue _dialogue;
    [SerializeField] ScriptableRoom _startingRoom;
    [SerializeField] float _startingPointX;
    [SerializeField] bool _skipOnLoad;
    [SerializeField] float _detectionGracePeriod;
    [SerializeField] bool _willTriggerDetection;

    public ScriptableRoom StartingRoom { get => _startingRoom;  }
    public float StartingPointX { get => _startingPointX; }
    public bool SkipOnLoad { get => _skipOnLoad; }
    public ScriptableDialogue Dialogue { get => _dialogue; }
    public float DetectionGracePeriod { get => _detectionGracePeriod; }
    public bool WillTriggerDetection { get => _willTriggerDetection; }
}
