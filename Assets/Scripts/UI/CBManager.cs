using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class CBManager : MonoBehaviour
{
    public InspectableDictionary<ScriptableKnowledge, Vector2> _knowledgeToPosition = new InspectableDictionary<ScriptableKnowledge, Vector2>();

    [SerializeField] private GameObject _knowledgeButtonPrefab;
    [SerializeField] private RectTransform _knowledgeContainer;
    [SerializeField] private float _buttonSize = 128f;
    
    private List<Vector2> spawnedPositions = new List<Vector2>();
    private List<GameObject> spawnedButtons = new List<GameObject>();
    public UILineRenderer _uiLineRenderer;

    public ScriptableKnowledge[] knowledgeToSpawn;

    
    private void Awake()
    {
        _uiLineRenderer.SetVisible(false);
    }

    private void Start()
    {
        ConstantManagers.Instance.KnowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);

        foreach (ScriptableKnowledge knowledge in knowledgeToSpawn)
        {
            ConstantManagers.Instance.KnowledgeSystem.AddKnowledge(knowledge);
        }
        //SpawnAllKnowledgeButtons();
    }

    private void OnDestroy()
    {
        ConstantManagers.Instance.KnowledgeSystem.UnsubscribeFromKnowledgeGained(OnKnowledgeGained);
    }
    
    public void ToggleCB()
    {
        CanvasGroup cg = _knowledgeContainer.parent.GetComponent<CanvasGroup>();
        cg.alpha = cg.alpha == 0 ? 1 : 0;
        cg.interactable = cg.alpha == 1;
        _uiLineRenderer.SetVisible(cg.alpha != 0);

        if(cg.alpha == 1)
        {
            LoopingManagers.Instance.TimeSystem.Pause("Conspiracy");
        }
        else
        {
            LoopingManagers.Instance.TimeSystem.Unpause("Conspiracy");
        }
    }

    private void CheckAndDrawKnowledgeConnection(ScriptableKnowledge knowledge, ScriptableKnowledge dependentKnowledge)
    {
        if (_knowledgeToPosition.GetDictionary().ContainsKey(knowledge) && 
            _knowledgeToPosition.GetDictionary().ContainsKey(dependentKnowledge) &&
            ConstantManagers.Instance.KnowledgeSystem.HasKnowledge(knowledge) &&
            ConstantManagers.Instance.KnowledgeSystem.HasKnowledge(dependentKnowledge))
        {
            Vector2 position = _knowledgeToPosition.GetDictionary()[knowledge];
            Vector2 dependentPosition = _knowledgeToPosition.GetDictionary()[dependentKnowledge];
            _uiLineRenderer.DrawLine(position, dependentPosition);
            Debug.Log($"Drawing line from {knowledge.name} ({position}) to {dependentKnowledge.name} ({dependentPosition})");
        }
    }

    private void OnKnowledgeGained(ScriptableKnowledge knowledge)
    {
        if (!_knowledgeToPosition.GetDictionary().ContainsKey(knowledge))
            return;
        Vector2 position = _knowledgeToPosition.GetDictionary()[knowledge];
        SpawnButton(knowledge, position);

        // Check if current knowledge has dependent knowledge
        if (knowledge.DependentKnowledge != null)
        {
            CheckAndDrawKnowledgeConnection(knowledge, knowledge.DependentKnowledge);
        }

        // Check if current knowledge is dependent knowledge for any other known knowledge
        foreach (var knowledgeItem in _knowledgeToPosition.GetDictionary().Keys)
        {
            if (knowledgeItem.DependentKnowledge == knowledge)
            {
                CheckAndDrawKnowledgeConnection(knowledgeItem, knowledge);
            }
        }
    }
    
    GameObject SpawnButton(ScriptableKnowledge data, Vector2 position)
    {
        GameObject btnObj = Instantiate(_knowledgeButtonPrefab, _knowledgeContainer);
        RectTransform rectTransform = btnObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(_buttonSize, _buttonSize);
        
        
        Image btnImage = btnObj.GetComponent<Image>();
        if (btnImage != null && data.Icon != null)
        {
            btnImage.sprite = data.Icon;
        }
        
        //TODO: Store the button data reference
        ButtonController controller = btnObj.GetComponent<ButtonController>();
        if (controller != null)
        {
            controller.SetData(data);
        }
        
        spawnedPositions.Add(position);
        spawnedButtons.Add(btnObj);
        
        return btnObj;
    }
    
    bool IsWithinBounds(Vector2 position)
    {
        Rect rect = _knowledgeContainer.rect;
        float halfSize = _buttonSize / 2f;
        
        return position.x >= rect.xMin + halfSize &&
               position.x <= rect.xMax - halfSize &&
               position.y >= rect.yMin + halfSize &&
               position.y <= rect.yMax - halfSize;
    }
    public void ClearAllButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear();
        spawnedPositions.Clear();
    }
    public void RespawnButtons()
    {
        ClearAllButtons();
        //TODO: Write logic to respawn buttons
    }
}