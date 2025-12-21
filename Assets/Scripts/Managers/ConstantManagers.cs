using UnityEngine;

// A singleton used to reference any script that does not reset
public class ConstantManagers : MonoBehaviour
{
    public static ConstantManagers Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] KnowledgeSystem _knowledgeSystem;
    public KnowledgeSystem KnowledgeSystem { get => _knowledgeSystem; }

    [SerializeField] QuestSystem _questSystem;

    public QuestSystem QuestSystem { get => _questSystem; }

    [SerializeField] AssetDatabase _assetDatabase;
    public AssetDatabase AssetDatabase { get => _assetDatabase; }

    [SerializeField] DialogueStored _dialogueStored;
    public DialogueStored DialogueStored { get => _dialogueStored; }
}
