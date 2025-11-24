using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class UI_GossipRunner : MonoBehaviour
{
    [SerializeField] UI_TypewriterText _text;
    [SerializeField] Image _icon;
    [SerializeField] CanvasGroup _visible;

    [SerializeField] float _delayPerChar;
    [SerializeField] float _maxDelay;
    [SerializeField] float _minDelay;

    List<ScriptableGossip> _alreadyRan = new List<ScriptableGossip>();
    ScriptableGossip _currentGossip;
    ScriptableKnowledge _currentKnowledge;

    Coroutine _currentlyRunning;
    bool _isBeingWatched = false;

    KnowledgeSystem _knowledgeSystem;

    private void Start()
    {
        _knowledgeSystem = ConstantManagers.Instance.KnowledgeSystem;
    }

    public void SetGossip(ScriptableGossip gossip)
    {
        if(_currentGossip == gossip)
        {
            ShowGossip();
            if(_currentKnowledge != null)
            {
                _knowledgeSystem.AddKnowledge(_currentKnowledge);
            }
        }
        else if (!_alreadyRan.Contains(gossip))
        {
            if(_currentGossip != null)
            {
                StopCoroutine(_currentlyRunning);                
            }

            _alreadyRan.Add(gossip);
            _currentGossip = gossip;
            _currentlyRunning = StartCoroutine(Gossip());
        }
    }

    void ShowGossip()
    {
        _visible.alpha = 1;
        _isBeingWatched = true;
    }

    public void HideGossip()
    {
        _visible.alpha = 0;
        _isBeingWatched = false;
    }
    
    IEnumerator Gossip()
    {
        ShowGossip();

        foreach (var line in _currentGossip.Dialogue)
        {
            string message = line.Line;
            _icon.sprite = line.Speaker.Icon;
            _text.RunLine(message);

            _currentKnowledge = line.Knowledge;
            if (_currentKnowledge != null)
            {
                _knowledgeSystem.AddKnowledge(_currentKnowledge);
            }

            float delay = message.Length * _delayPerChar;
            delay = Mathf.Clamp(delay, _minDelay, _maxDelay);

            yield return new WaitForSeconds(delay);
        }

        _currentGossip = null;
        _currentlyRunning = null;

        HideGossip();
    }

}
