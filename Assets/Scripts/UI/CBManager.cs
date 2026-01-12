using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class CBManager : MonoBehaviour
{
    [SerializeField] private GameObject _knowledgeButtonPrefab;
    [SerializeField] private RectTransform _knowledgeContainer;
    [SerializeField] private float _buttonSize = 128f;
    
    private List<Vector2> spawnedPositions = new List<Vector2>();
    private List<GameObject> spawnedButtons = new List<GameObject>();
    public UILineRenderer _uiLineRenderer;
    List<ScriptableKnowledge> _spawnedKnowledge = new List<ScriptableKnowledge>();

    public ScriptableKnowledge[] knowledgeToSpawn;

    KnowledgeSystem _knowledgeSystem;
    
    private void Awake()
    {
        _uiLineRenderer.SetVisible(false);
    }

    private void Start()
    {
        _knowledgeSystem = ConstantManagers.Instance.KnowledgeSystem;
        _knowledgeSystem.SubscribeToKnowledgeGained(OnKnowledgeGained);

        foreach (ScriptableKnowledge knowledge in knowledgeToSpawn)
        {
            _knowledgeSystem.AddKnowledge(knowledge);
        }

        foreach(ScriptableKnowledge knowledge in _knowledgeSystem.CollectedKnowledge)
        {
            OnKnowledgeGained(knowledge);
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
        cg.blocksRaycasts = cg.alpha == 1;
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
        if (_knowledgeSystem.HasKnowledge(knowledge) &&
            _knowledgeSystem.HasKnowledge(dependentKnowledge))
        {
            _uiLineRenderer.DrawLine(knowledge.BoardPosition, dependentKnowledge.BoardPosition);
        }
    }

    private void OnKnowledgeGained(ScriptableKnowledge knowledge)
    {
        if (knowledge.HideFromBoard)
            return;
        
        SpawnButton(knowledge, knowledge.BoardPosition);

        foreach(ScriptableKnowledge spawned in _spawnedKnowledge)
        {
            if(spawned.Connections.Contains(knowledge) || knowledge.Connections.Contains(spawned))
            {
                CheckAndDrawKnowledgeConnection(knowledge, spawned);
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
        _spawnedKnowledge.Add(data);


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