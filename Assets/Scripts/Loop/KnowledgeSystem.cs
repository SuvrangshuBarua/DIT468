using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

// Saves the information that the player has observed. Does not reset
public class KnowledgeSystem : MonoBehaviour
{
    List<ScriptableKnowledge> _collectedKnowledge = new List<ScriptableKnowledge>();
    public List<ScriptableKnowledge> CollectedKnowledge { get => _collectedKnowledge; }

    UnityEvent<ScriptableKnowledge> _onKnowledgeGained = new UnityEvent<ScriptableKnowledge>();

    public void AddKnowledge(ScriptableKnowledge newKnowledge)
    {
        if (!_collectedKnowledge.Contains(newKnowledge))
        {
            _collectedKnowledge.Add(newKnowledge);
            _onKnowledgeGained.Invoke(newKnowledge);
        }
    }

    public bool HasKnowledge(ScriptableKnowledge knowledge)
    {
        return _collectedKnowledge.Contains(knowledge);
    }

    public bool HasKnowledge(List<ScriptableKnowledge> knowledge)
    {
        foreach(ScriptableKnowledge item in knowledge)
        {
            if (!_collectedKnowledge.Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    public void SubscribeToKnowledgeGained(UnityAction<ScriptableKnowledge> action)
    {
        _onKnowledgeGained.AddListener(action);
    }
}
