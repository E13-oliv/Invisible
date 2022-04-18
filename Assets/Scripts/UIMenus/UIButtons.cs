using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtons : MonoBehaviour, ISelectHandler, IDeselectHandler,IPointerEnterHandler
{
    private Color textColor;
    private Color selectedTextColor;
    private Color disabledColor;

    enum ButtonTypeE
    {
        Normal,
        Warning,
        Score
    }

    [Header("Button Type")]
    [SerializeField]
    private ButtonTypeE buttonType;

    [Header("Standard Menus")]
    [SerializeField]
    private Color menuTextColor = Color.white;
    [SerializeField]
    private Color menuSelectedTextColor = Color.red;
    [SerializeField]
    private Color menuDisabledTextColor = new Color(1f, 1f, 1f, 0.25f);

    [Header("Warning Menus")]
    [SerializeField]
    private Color warningTextColor = new Color(0.6f, 0f, 0f, 1f);
    [SerializeField]
    private Color warningSelectedTextColor = Color.white;
    [SerializeField]
    private Color warningDisabledTextColor = new Color(0.6f, 0f, 0f, 0.25f);

    [Header("Score Menus")]
    [SerializeField]
    private Color scoreTextColor = new Color(1f, 1f, 1f, .3f);
    [SerializeField]
    private Color scoreSelectedTextColor = Color.white;
    [SerializeField]
    private Color scoreDisabledTextColor = new Color(1f, 1f, 1f, .3f);

    private void Awake()
    {
        switch (buttonType.ToString())
        {
            case "Warning":
                textColor = warningTextColor;
                selectedTextColor = warningSelectedTextColor;
                disabledColor = warningDisabledTextColor;
                break;

            case "Score":
                textColor = scoreTextColor;
                selectedTextColor = scoreSelectedTextColor;
                disabledColor = scoreDisabledTextColor;
                break;

            default:
                textColor = menuTextColor;
                selectedTextColor = menuSelectedTextColor;
                disabledColor = menuDisabledTextColor;
                break;
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            GetComponentInChildren<Text>().color = selectedTextColor;
        }
        else
        {
            if (GetComponent<Button>())
            {
                if (GetComponent<Button>().interactable == false)
                {
                    GetComponentInChildren<Text>().color = disabledColor;
                }
                else
                {
                    GetComponentInChildren<Text>().color = textColor;
                }
            }
            else if (GetComponent<Toggle>())
            {
                if (GetComponent<Toggle>().interactable == false)
                {
                    GetComponentInChildren<Text>().color = disabledColor;
                }
                else
                {
                    GetComponentInChildren<Text>().color = textColor;
                }
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponentInChildren<Text>().color = selectedTextColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponentInChildren<Text>().color = textColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>())
        {
            if (GetComponent<Button>().interactable == true)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
        else if (GetComponent<Toggle>())
        {
            if (GetComponent<Toggle>().interactable == true)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
    }
}
