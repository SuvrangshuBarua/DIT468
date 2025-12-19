using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [SerializeField] GameObject _element;

    public void Toggle()
    {
        _element.SetActive(!_element.activeSelf);
    }
}
