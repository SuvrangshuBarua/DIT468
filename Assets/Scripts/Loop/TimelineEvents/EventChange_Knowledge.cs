using UnityEngine;

// Unlocks knowledge
[System.Serializable]
public class EventChange_Knowledge : IEventChange
{
    [SerializeField] ScriptableKnowledge _knowledge;

    public void OnEventOccured()
    {
        ConstantManagers.Instance.KnowledgeSystem.AddKnowledge(_knowledge);
    }
}
