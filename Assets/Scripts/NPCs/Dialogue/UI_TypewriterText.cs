using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class UI_TypewriterText : MonoBehaviour
{
    [SerializeField] float _secondDelay;
    [SerializeField] TextMeshProUGUI _text;

    [SerializeField] UnityEvent _onComplete;

    string _currentMessage;
    Coroutine _currentWriting;
    bool _isWriting = false;

    public void RunLine(string line)
    {
        if(_currentWriting != null)
        {
            StopCoroutine(_currentWriting);
            _currentWriting = null;
        }
        _currentMessage = line;
        _currentWriting = StartCoroutine(WriteLine(line));
    }
       
    public void EndLine()
    {
        StopCoroutine(_currentWriting);
        _currentWriting = null;
        _text.maxVisibleCharacters = _currentMessage.Length;
        _onComplete.Invoke();
    }

    IEnumerator WriteLine(string msg)
    {
        _isWriting = true;
        _text.text = msg;
        _text.maxVisibleCharacters = 0;

        for (int i = 0; i < msg.Length; i++)
        {
            char c = msg[i];
            _text.maxVisibleCharacters++;
            yield return new WaitForSeconds(_secondDelay);
        }

        _currentWriting = null;
        _isWriting = false;
        _onComplete.Invoke();
    }

}
