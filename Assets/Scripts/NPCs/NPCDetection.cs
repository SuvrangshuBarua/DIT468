using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ScriptableCharacterVisuals;

public class NPCDetection : MonoBehaviour
{
    float currentSuspicion;

    bool isSuspicious;
    bool _isNextToPlayer = false;
    DisguiseManager _disguise;

    [SerializeField] UnityEvent<bool> _onNPCSuspicious = new UnityEvent<bool>();

    NPCTracker self;
    public bool IsSuspicious { get { return isSuspicious; } }

    

    public void Setup(NPCTracker character)
    {
        self = character;
        isSuspicious = false;
        _disguise = LoopingManagers.Instance.Player.Disguise;
        _disguise.SubscribeToVisibilityChanged(OnVisibilityChanged);
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isNextToPlayer = true;
            GetSuspicious();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isNextToPlayer = false;
            CalmDown();
        }
    }
    
    public void OnVisibilityChanged(bool isVisible)
    {
        if (_isNextToPlayer)
        {
            if (isVisible)
            {
                GetSuspicious();
            }
            else
            {
                CalmDown();
            }
        }
    }

    public bool GetSuspicious()
    {
        if (!self.TriggersDetection || _disguise.IsTransparent)
        {
            isSuspicious = false;
            return isSuspicious;
        }


        if (_disguise.CheckIfThePlayerShouldBeHere())
        {
            //slow
            var npcsInRoom = LoopingManagers.Instance.NPCManager.GetNPCsInRoom(self.CurrentRoom);

            print(npcsInRoom.Count);

            // get characters disguise
            ScriptableCharacterVisuals playerType = DisguiseManager.Instance.CurrentCharacterVisual;

            print(playerType);

            //we need the player here
            NPCTracker disguiseIsAlreadyInTheRoom = npcsInRoom.Find(n => n.NPC == playerType);

            print(disguiseIsAlreadyInTheRoom);

            isSuspicious = disguiseIsAlreadyInTheRoom != null;
        }
        else
        {
            isSuspicious = true;
        }


        _onNPCSuspicious.Invoke(isSuspicious);
        Debug.Log("Suspicion is " + isSuspicious.ToString());

        return isSuspicious;
    }

    public void CalmDown()
    {
        Debug.Log("NPC has calmed down.");
        _onNPCSuspicious.Invoke(false);
        isSuspicious = false;        
    }
}
