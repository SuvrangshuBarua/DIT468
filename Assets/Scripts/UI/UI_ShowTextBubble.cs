using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_ShowTextBubble : MonoBehaviour
{
    Dictionary<GameObject, UI_TextBubble> _currentTextboxes = new Dictionary<GameObject, UI_TextBubble>();
    Transform _playerTransform;

    [SerializeField] GameObject _prefabTextbox;
    [SerializeField] Transform _textboxParent;
    [SerializeField] float _fullOpacityTreshold;
    [SerializeField] float _fullTransparencyTreshold;

    private void Start()
    {
        LoopingManagers.Instance.RoomManager.SubscribeToNewRoom(OnRoomChange);
    }

    void OnRoomChange(ScriptableRoom room)
    {
        foreach(UI_TextBubble bubble in _currentTextboxes.Values)
        {
            bubble.StopAllCoroutines();
            Destroy(bubble.gameObject);
        }

        _currentTextboxes = new Dictionary<GameObject, UI_TextBubble>();
    }

    public void SetText(GameObject keyObject, string message, Color textColor)
    {
        RemoveText(keyObject);

        GameObject newTextbox = Instantiate(_prefabTextbox, _textboxParent);
        UI_TextBubble textbox = newTextbox.GetComponent<UI_TextBubble>();
        textbox.SetText(keyObject, message, textColor);
        _currentTextboxes.Add(keyObject, textbox);

    }

    public void RemoveText(GameObject keyObject)
    {
        if (_currentTextboxes.ContainsKey(keyObject))
        {
            UI_TextBubble previousTextbox = _currentTextboxes[keyObject];
            previousTextbox.StopAllCoroutines();
            Destroy(previousTextbox.gameObject);
            _currentTextboxes.Remove(keyObject);
        }
    }

    public void ToggleTextbox(GameObject keyObject, bool show)
    {
        if(_currentTextboxes.TryGetValue(keyObject, out UI_TextBubble textbox))
        {
            textbox.SetVisibility((show) ? 1 : 0);
        }
    }
    
}
