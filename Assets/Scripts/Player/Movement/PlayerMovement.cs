using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// The movement script for the player
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    float _moveX;

    bool _facingLeft = false;
    UnityEvent<bool> _onChangeFacingLeft = new UnityEvent<bool>();

    TimeSystem _time;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
    }

    // Stores the horizontal direction that the player has input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveX = context.ReadValue<float>();
        
        if (_moveX != 0)
        {
            bool facingLeft = _moveX < 0;
            if(_facingLeft != facingLeft)
            {
                _facingLeft = facingLeft;
                _onChangeFacingLeft.Invoke(_facingLeft);
            }
        }
    }

    void Update()
    {
        if (!_time.IsPaused)
        {
            transform.Translate(new Vector2(1, 0) * _moveX * (_speed * Time.deltaTime));
        }
    }
}
