using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    private ScriptableKnowledge _data;
    private Button _button;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    public void SetData(ScriptableKnowledge data)
    {
        _data = data;
    }

    private void OnButtonClicked()
    {
        if (_data != null)
        {
            //TODO: Show details of the knowledge
            InfoPopup.Instance.ShowPanel(_data);
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }
}
