using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// The movement script for the player
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    int _moveX;
    
    UnityEvent<int> _onChangeMovement = new UnityEvent<int>();
 
    TimeSystem _time;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
    }

    // Stores the horizontal direction that the player has input
    public void OnMove(InputAction.CallbackContext context)
    {
        int movement = (int) context.ReadValue<float>();
        if(movement != _moveX)
        {
            _moveX = movement;
            _onChangeMovement.Invoke(movement);
        }
    }

    void Update()
    {
        if (!_time.IsPaused)
        {
            transform.Translate(new Vector2(1, 0) * _moveX * (_speed * Time.deltaTime));
        }
    }

    public void SubsccribeToMovementChanged(UnityAction<int> action)
    {
        _onChangeMovement.AddListener(action);
    }
}
