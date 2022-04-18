using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject firstActiveButton;

    private void Start()
    {
        activeFirstButton();
    }

    private void OnEnable()
    {
        activeFirstButton();
    }

    private void activeFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(firstActiveButton);
    }

    public void SetFirstActiveButton(GameObject activeButton)
    {
        firstActiveButton = activeButton;
    }
}
