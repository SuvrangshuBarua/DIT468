using UnityEngine;
using System.Collections.Generic;

public class NPCObject : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    bool _listeningIn = false;
    bool _isPlayerClose = false;

    NPCDetection _detection;

    public bool CanInteract()
    {
        return _npcData.NPC.Dialogue != null;
    }
    
    public string GetInteractionPrompt()
    {    
        if (_npcData.NPC.Dialogue != null)
        {
            return "Talk";
        }

        return "";
    }

    public void OnInteract()
    {
        OnGossiping(false);
        LoopingManagers.Instance.DialogueRunner.SetDialogue(_npcData.NPC.Dialogue);
    }

    public void Setup(NPCTracker data)
    {
        _npcData = data;
        _npcData.SubscribeToGossip(OnGossiping);

        _detection = new NPCDetection();
        _detection.Setup(_npcData.NPC.DetectionGracePeriod);

        // TEMP
        transform.position = data.NPC.StartingPoint;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = true;
            
            if(_npcData.CurrentRoom.EntitiesAllowedInRoom.Contains(player.Disguise))
            {
                //player is in disquise, they can eavesdrop if there is gossip
                if (_npcData.CurrentGossip != null)
                {
                    LoopingManagers.Instance.Gossip.SetGossip(_npcData.CurrentGossip);
                    _listeningIn = true;
                }
            } 
            else
            {
                _detection.BecomeSuspicious();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = false;

            if(_detection.IsSuspicious)
            {
                _detection.CalmDown();
            } 
            else if (_listeningIn)
            {
                LoopingManagers.Instance.Gossip.HideGossip();
                _listeningIn = false;
            }
        }
    }

    public void OnGossiping(bool isGossiping)
    {
        if(_listeningIn && !isGossiping)
        {
            LoopingManagers.Instance.Gossip.HideGossip();
            _listeningIn = false;
        }

        if(isGossiping && _isPlayerClose)
        {
            LoopingManagers.Instance.Gossip.SetGossip(_npcData.CurrentGossip);
            _listeningIn = true;
        }
    }
}
