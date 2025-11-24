using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableGossip", menuName = "Scriptable Objects/ScriptableGossip")]
public class ScriptableGossip : ScriptableObject
{
    [SerializeField] List<ScriptableNPC> _npcsTalking;
    [SerializeField] List<GossipLine> _dialogue;
    [SerializeField] int _talkingDuration;

    public List<ScriptableNPC> NpcsTalking { get => _npcsTalking; }
    public List<GossipLine> Dialogue { get => _dialogue; }
    public int TalkingDuration { get => _talkingDuration; }
    
    [System.Serializable]
    public class GossipLine
    {
        [SerializeField] ScriptableCharacterVisuals _speaker;
        [SerializeField] ScriptableKnowledge _knowledge;
        [SerializeField] string _line;

        public ScriptableCharacterVisuals Speaker { get => _speaker; }
        public ScriptableKnowledge Knowledge { get => _knowledge; }
        public string Line { get => _line; }
    }
}
