using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DisguiseDisplay : MonoBehaviour
{
    [SerializeField] GameObject disguisesContainer;

    TimeSystem _time;

    public void DestroyChildrenInContainer()
    {
        foreach (Transform child in disguisesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowDisguiseDisplay()
    {
        DestroyChildrenInContainer();
        gameObject.SetActive(true); 
        ShowDisguseElements();
        _time = LoopingManagers.Instance.TimeSystem;
        _time.Pause("DisguiseUI");
    }

    public void HideDisguiseDisplay()
    {
        _time.Unpause("DisguiseUI");
        gameObject.SetActive(false);
    }

    private void ShowDisguseElements()
    {
        DisguiseManager disguiseManager = DisguiseManager.Instance;
        foreach (var disguise in disguiseManager.AvailableDisguise)
        {
            GameObject disguiseElement = new GameObject("DisguiseElement");
            disguiseElement.transform.SetParent(disguisesContainer.transform);
            Image disguiseImage = disguiseElement.AddComponent<Image>();
            disguiseImage.sprite = disguise.Icon;
            Button disguiseButton = disguiseElement.AddComponent<Button>();

            disguiseButton.onClick.AddListener(() =>
            {
                disguiseManager.SetCurrentCharacterVisual(disguise);
                HideDisguiseDisplay();
            });
        }
    }
}
