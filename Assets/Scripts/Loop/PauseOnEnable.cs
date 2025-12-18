using UnityEngine;

public class PauseOnEnable : MonoBehaviour
{
    [SerializeField] string _key;

    private void OnEnable()
    {
        LoopingManagers.Instance.TimeSystem.Pause(_key);
    }

    private void OnDisable()
    {
        LoopingManagers.Instance.TimeSystem.Unpause(_key);
    }
}
