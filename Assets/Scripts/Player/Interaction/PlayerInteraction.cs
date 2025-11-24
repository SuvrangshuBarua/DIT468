using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// Detects any IInteractable objects that the player passes by.
public class PlayerInteraction : MonoBehaviour
{
    UnityEvent<IInteractable> _newInteractable = new UnityEvent<IInteractable>();
    UnityEvent _afterInteraction = new UnityEvent();

    IInteractable _currentInteraction;
    
    TimeSystem _time;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
    }

    public IInteractable CurrentInteraction { get => _currentInteraction; }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && _currentInteraction != null && _currentInteraction.CanInteract() && !_time.IsPaused)
        {
            _currentInteraction.OnInteract();
            _afterInteraction.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable newobj))
        {
            _currentInteraction = newobj;
            _newInteractable.Invoke(_currentInteraction);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable newobj))
        {
            if(_currentInteraction != newobj)
            {
                _currentInteraction = newobj;
                _newInteractable.Invoke(_currentInteraction);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interaction;
        if (collision.gameObject.TryGetComponent(out interaction))
        {
            if (_currentInteraction == interaction)
            {
                _currentInteraction = null;
                _newInteractable.Invoke(_currentInteraction);
            }
        }
    }

    public void SubscribeToNewInteraction(UnityAction<IInteractable> action)
    {
        _newInteractable.AddListener(action);
    }

    public void SubscribeToAfterInteraction(UnityAction action)
    {
        _afterInteraction.AddListener(action);
    }
}
