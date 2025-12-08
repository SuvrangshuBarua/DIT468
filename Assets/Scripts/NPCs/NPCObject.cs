using UnityEngine;
using UnityEngine.Events;

public class NPCObject : MonoBehaviour, IInteractable
{
    NPCTracker _npcData;
    bool _listeningIn = false;
    bool _isPlayerClose = false;

    [SerializeField] UnityEvent<NPCTracker> _onTrackerSet;

    private void Update()
    {
        transform.position = new Vector3(_npcData.CurrentXPoint, _npcData.CurrentRoom.NpcYLevel) * 2;
    }

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
        _onTrackerSet.Invoke(data);


        transform.position = new Vector3(data.CurrentXPoint, data.CurrentRoom.NpcYLevel);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerManager player))
        {
            _isPlayerClose = true;
            if (_npcData.CurrentGossip != null)
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
