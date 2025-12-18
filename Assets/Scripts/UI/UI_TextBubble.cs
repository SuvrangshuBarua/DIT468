using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UI_TextBubble : MonoBehaviour
{
    Transform _playerTransform;
    Camera _camera;
    GameObject _keyObject;

    [SerializeField] float _fullOpacityTreshold;
    [SerializeField] float _fullTransparencyTreshold;
    [SerializeField] Vector3 _offset;
    [SerializeField] CanvasGroup _opacity;
    [SerializeField] TextMeshProUGUI _text;
    
    public void SetText(GameObject keyObject, string message, Color textColor)
    {
        _keyObject = keyObject;
        _camera = LoopingManagers.Instance.CameraManager.Camera;
        _playerTransform = LoopingManagers.Instance.Player.transform;

        transform.position = _camera.WorldToScreenPoint(_keyObject.transform.position) + _offset;

        _text.text = message;
        _text.color = textColor;
        StartCoroutine(DisplayBubble());
    }

    public void SetVisibility(float alpha)
    {
        _opacity.alpha = alpha;
    }

    IEnumerator DisplayBubble()
    {
        while (true)
        {
            if(_keyObject == null)
            {
                LoopingManagers.Instance.TextBubbles.RemoveText(_keyObject);
                break;
            }

            transform.position = _camera.WorldToScreenPoint(_keyObject.transform.position + _offset);

            yield return null;
        }
    }
}
