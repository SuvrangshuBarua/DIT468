using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ScriptableCharacterVisuals;

public class NPCDetection : MonoBehaviour
{
    float currentSuspicion;

    bool isSuspicious;

    UnityEvent OnAlertRaisedEvent;

    NPCTracker self;
    public bool IsSuspicious { get { return isSuspicious; } }

    public void Setup(NPCTracker character)
    {
        self = character;
        isSuspicious = false;
    }

    private void Update()
    {
        if (isSuspicious)
        {
            currentSuspicion += Time.deltaTime;

            if (currentSuspicion >= self.NPC.DetectionGracePeriod)
            { 
                isSuspicious = false;
                currentSuspicion = 0;

                // Leave this here for future
                OnAlertRaisedEvent?.Invoke();


                Debug.Log("Alarm is Triggered");
                //loop back cause you got caught
                LoopingManagers.Instance.LoopSystem.Loop();
            }
        }
        else if (currentSuspicion > 0)
        {
            currentSuspicion -= Time.deltaTime;
        }
    }

    public bool GetSuspicious()
    {
        if (!self.NPC.WillTriggerDetection)
        {
            isSuspicious = false;
            return isSuspicious;
        }


        if (DisguiseManager.Instance.CheckIfThePlayerShouldBeHere())
        {
            //slow
            var npcsInRoom = LoopingManagers.Instance.NPCManager.GetNPCsInRoom(self.CurrentRoom);

            // get characters disguise
            Character playerType = DisguiseManager.Instance.CurrentCharacterVisual.GetCharacterType();

            //we need the player here
            NPCTracker disguiseIsAlreadyInTheRoom = npcsInRoom.Find(n => n.NPC.GetCharacterType() == playerType);

            isSuspicious = disguiseIsAlreadyInTheRoom != null;
        }
        else
        {
            isSuspicious = true;
        }

        Debug.Log("Suspicion is " + isSuspicious.ToString());

        return isSuspicious;
    }

    public void CalmDown()
    {
        Debug.Log("NPC has calmed down.");
        isSuspicious = false;        
    }
}
