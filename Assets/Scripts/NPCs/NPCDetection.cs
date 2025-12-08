using System;
using UnityEngine;
using UnityEngine.Events;

public class NPCDetection : MonoBehaviour
{
    float gracePeriod; // max suspicion before raising alert and we re-loop

    float currentSuspiction;
    bool isSuspicious;

    public Action OnAlertRaised;

    public UnityEvent OnAlertRaisedEvent;
    public bool IsSuspicious { get { return isSuspicious; } }

    public void Setup(float gracePeriod)
    {
        this.gracePeriod = gracePeriod;
        currentSuspiction = 0;
        isSuspicious = false;
    }

    private void Update()
    {
        if (isSuspicious)
        {
            currentSuspiction += Time.deltaTime;

            if (currentSuspiction >= gracePeriod)
            {
                OnAlertRaised?.Invoke();
                OnAlertRaisedEvent?.Invoke();
                isSuspicious = false;
                currentSuspiction = 0;
            }
        }
        else if (currentSuspiction > 0)
        {
            currentSuspiction -= Time.deltaTime;
        }
    }

    public void BecomeSuspicious()
    {
        isSuspicious = true;
    }

    public void CalmDown()
    {
        isSuspicious = false;
    }
}
