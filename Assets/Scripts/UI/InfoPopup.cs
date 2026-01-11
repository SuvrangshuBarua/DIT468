using System;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Button _closeButton;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {        
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
        ShowPanel(data.KnowledgeName, data.InformationSummary);
    }

    public void ShowPanel(string title, string description)
    {
        _titleText.text = title;
        _descriptionText.text = description;
        _popupPanel.SetActive(true);
        _canvasGroup.alpha = 1;
    }
}
