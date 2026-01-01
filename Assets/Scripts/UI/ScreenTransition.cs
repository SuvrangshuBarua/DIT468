using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] CanvasGroup _transparency;
    [SerializeField] float _startingFade;
    [SerializeField] float _fadeHold;

    private void Awake()
    {
        _transparency.alpha = 1;
    }
    
    public void FadeOut(float duration, UnityAction action)
    {
        StartCoroutine(FadeCoroutine(duration, false, action));
    }
    
    public void FadeIn(float duration)
    {
        StartCoroutine(FadeCoroutine(duration, true, null));
    }

    IEnumerator FadeCoroutine(float duration, bool fadeIn, UnityAction action)
    {
        int startAlpha = (fadeIn) ? 1 : 0;
        int endAlpha = (fadeIn) ? 0 : 1;
        float elapsedTime = 0;
        _transparency.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _transparency.alpha = startAlpha * (1 - t) + endAlpha * (t);
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        _transparency.alpha = endAlpha;

        yield return new WaitForSeconds(_fadeHold);
        if(action != null)
        {
            action.Invoke();
        }
    }
}
