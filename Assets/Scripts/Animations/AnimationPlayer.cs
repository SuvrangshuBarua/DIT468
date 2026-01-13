using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationPlayer: MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _rederer;
    Coroutine _currentlyPlaying;

    UnityEvent _onAnimationStarted = new UnityEvent();
    UnityEvent _onAnimationComplete = new UnityEvent();

    TimeSystem _time;

    public void PlayClip(ScriptableAnimationClip clip)
    {
        _time = LoopingManagers.Instance.TimeSystem;

        if(_currentlyPlaying != null)
        {
            StopCoroutine(_currentlyPlaying);
        }

        _onAnimationStarted.Invoke();
        _currentlyPlaying = StartCoroutine(Play(clip));
    }

    IEnumerator Play(ScriptableAnimationClip clip)
    {
        if(clip.StaticSprite != null)
        {
            _rederer.sprite = clip.StaticSprite;           
        }
        else
        {
            foreach (var loop in clip.PlayOnStart)
            {
                int frameIndex = 0;
                while (loop.HasFrame(frameIndex))
                {
                    (Sprite, float) frame = loop.GetNextFrame(frameIndex);
                    _rederer.sprite = frame.Item1;
                    yield return new WaitForSeconds(frame.Item2);

                    frameIndex++;
                }                
            }

            if (clip.PlayLoop.Length != 0)
            {
                while (true)
                {
                    foreach (var loop in clip.PlayLoop)
                    {
                        int frameIndex = 0;
                        while (loop.HasFrame(frameIndex))
                        {
                            (Sprite, float) frame = loop.GetNextFrame(frameIndex);
                            _rederer.sprite = frame.Item1;
                            yield return new WaitForSeconds(frame.Item2);

                            frameIndex++;
                        }
                    }
                }
            }
            _onAnimationComplete.Invoke();
            _currentlyPlaying = null;
        }

        
    }

    public void SubscribeToAnimationStarted(UnityAction action)
    {
        _onAnimationStarted.AddListener(action);
    }

    public void SubscribeToAnimationComplete(UnityAction action)
    {
        _onAnimationComplete.AddListener(action);
    }
}
