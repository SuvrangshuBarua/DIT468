using UnityEngine;
using System.Collections.Generic;

public class NPCObject : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    bool _listeningIn = false;
    bool _isPlayerClose = false;

    [SerializeField] NPCDetection _detection;

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

        _detection.Setup(_npcData);

        // TEMP
        transform.position = data.NPC.StartingPoint;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = true;
            //player is in disquise, they can eavesdrop if there is gossip
            if (!_detection.GetSuspicious() && _npcData.CurrentGossip != null)
            {
                LoopingManagers.Instance.Gossip.SetGossip(_npcData.CurrentGossip);
                _listeningIn = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = false;

            _detection.CalmDown();

            if (_listeningIn)
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
