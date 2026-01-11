using System;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    public static InfoPopup Instance { get; private set; }

    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Button _closeButton;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        _canvasGroup = _popupPanel.GetComponent<CanvasGroup>();
        
        if(_closeButton != null)
            _closeButton.onClick.AddListener(HidePanel);
    }

    private void OnDestroy()
    {
        if (_closeButton != null)
            _closeButton.onClick.RemoveListener(HidePanel);
    }

    private void HidePanel()
    {
        _canvasGroup.alpha = 0;
        _popupPanel.SetActive(false);
    }
    
    public void ShowPanel(ScriptableKnowledge data)
    {
        _titleText.text = data.KnowledgeName;
        _descriptionText.text = data.InformationSummary;
        _popupPanel.SetActive(true);
        _canvasGroup.alpha = 1;
    }
}
