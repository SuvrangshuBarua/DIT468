using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the time passing, as well as pausing
public class TimeSystem : MonoBehaviour
{
    [SerializeField] int _time;
    [SerializeField] float _secondsBetweenTicks;
    
    public int Time { get => _time; }

    List<string> _pauseRequests = new List<string>();
    bool _isPaused = false;
    public bool IsPaused { get => _isPaused; }      

    UnityEvent<int> _onTimeUpdate = new UnityEvent<int>();
    UnityEvent<bool> _onIsPausedChanged = new UnityEvent<bool>();

    List<(int, UnityAction)> _timers = new List<(int, UnityAction)>();

    // Updates the time
    public void Start()
    {
        StartTimer();
    }

    void StartTimer()
    {
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_secondsBetweenTicks);

            if (!_isPaused)
            {
                _time++;

                List<(int, UnityAction)> toRemove = new List<(int, UnityAction)>();
                foreach (var timer in _timers)
                {
                    if(timer.Item1 <= _time)
                    {
                        toRemove.Add(timer);
                        timer.Item2.Invoke();
                    }
                }

                foreach (var timer in toRemove)
                {
                    _timers.Remove(timer);
                }
                
                _onTimeUpdate.Invoke(_time);
            }
        }        
    }
    
    public void SubscribeToTimeUpdate(UnityAction<int> action)
    {
        _onTimeUpdate.AddListener(action);
    }

    // Pause time, with robustness for when multiple scripts try to pause and unpause
    public void Pause(string key)
    {
        if (_pauseRequests.Contains(key))
        {
            return;
        }

        _pauseRequests.Add(key);

        if (!_isPaused)
        {
            _isPaused = true;
            _onIsPausedChanged.Invoke(true);
        }
    }

    public void Unpause(string key)
    {
        if (!_pauseRequests.Contains(key))
        {
            return;
        }

        _pauseRequests.Remove(key);

        if (_pauseRequests.Count == 0)
        {
            _isPaused = false;
            _onIsPausedChanged.Invoke(false);
        }
    }


    public void SubscribeToPausedChanged(UnityAction<bool> action)
    {
        _onIsPausedChanged.AddListener(action);
    }
    
    public void SetTimer(int delay, UnityAction action)
    {
        _timers.Add((delay + _time, action));
    }
}
