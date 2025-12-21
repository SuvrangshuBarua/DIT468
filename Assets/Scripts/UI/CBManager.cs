using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class CBManager : MonoBehaviour
{
    [SerializeField] private GameObject _knowledgeButtonPrefab;
    [SerializeField] private RectTransform _knowledgeContainer;
    [SerializeField] private List<ScriptableKnowledge> _knowledgeList;

    [SerializeField] private int _maxAttempts = 50;
    [SerializeField] private float _clusterRadius = 200f;
    [SerializeField] private float _buttonSize = 128f;
    [SerializeField] private float _minDistance = 100f;

    private List<Vector2> spawnedPositions = new();
    private List<GameObject> spawnedButtons = new();
    private Dictionary<KnowledgeProvider, Vector2> _typeClusterCenters = new();
    private HashSet<ScriptableKnowledge> _spawnedKnowledgeButtonData = new();
    
    private void Start()
    {
        SpawnAllKnowledgeButtons();
    }

    private void SpawnAllKnowledgeButtons()
    {
        foreach (ScriptableKnowledge knowledge in _knowledgeList)
        {
            if (_spawnedKnowledgeButtonData.Contains(knowledge))
            {
                continue;
            }

            if (!_typeClusterCenters.ContainsKey(knowledge.KnowledgeProvider))
            {
                Vector2 clusterCenter = GetRandomClusterCenter();
                _typeClusterCenters[knowledge.KnowledgeProvider] = clusterCenter;
            }
            
            var newPosition = GetClusteredPosition(_typeClusterCenters[knowledge.KnowledgeProvider], _clusterRadius);
            if (newPosition != Vector2.zero)
            {
                SpawnButton(knowledge, newPosition);
                _spawnedKnowledgeButtonData.Add(knowledge);
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
        /*ButtonController controller = btnObj.GetComponent<ButtonController>();
        if (controller != null)
        {
            controller.Initialize(data);
        }*/
        
        spawnedPositions.Add(position);
        spawnedButtons.Add(btnObj);
        
        return btnObj;
    }

    private Vector2 GetRandomClusterCenter()
    {
        Rect rect = _knowledgeContainer.rect;
        float margin = _clusterRadius + _buttonSize;
        
        for(int attempt = 0; attempt < _maxAttempts * 2; attempt++)
        {
            Vector2 randomCenter = new Vector2(Random.Range(rect.xMin + margin, rect.xMax - margin), Random.Range(rect.yMin + margin, rect.yMax - margin));
            
            bool validCenter = true;
            foreach (Vector2 existingCenter in _typeClusterCenters.Values)
            {
                if (Vector2.Distance(existingCenter, randomCenter) < _clusterRadius * 2f)
                {
                    validCenter = false;
                    break;
                }
            }
            if (validCenter)
            {
                return randomCenter;
            }
        }
        return new Vector2(Random.Range(rect.xMin + margin, rect.xMax - margin), Random.Range(rect.yMin + margin, rect.yMax - margin));
    }
    private Vector2 GetClusteredPosition(Vector2 centerPos, float radius)
    {
        for (int attempt = 0; attempt < _maxAttempts; attempt++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(0f, radius);
            
            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );
            
            Vector2 newPos = centerPos + offset;
            
            if (IsWithinBounds(newPos) && IsPositionValid(newPos))
            {
                return newPos;
            }
        }
        
        Debug.LogWarning("Could not find valid clustered position");
        return Vector2.zero;
    }
    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 existingPos in spawnedPositions)
        {
            if (Vector2.Distance(position, existingPos) < _minDistance)
            {
                return false;
            }
        }
        return true;
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
        _typeClusterCenters.Clear();
    }
    
    public void RespawnButtons()
    {
        ClearAllButtons();
        SpawnAllKnowledgeButtons();
    }
}
