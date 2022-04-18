using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITitleManager : MonoBehaviour
{
    // managers
    private GameManager GM;

    [Header("Buttons")]
    [SerializeField]
    private GameObject continueButton;

    [Header("Panels")]
    [SerializeField]
    private GameObject e13Panel;
    [SerializeField]
    private GameObject titlePanel;
    [SerializeField]
    private GameObject overwriteAlertPanel;
    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private GameObject preferencesPanel;
    [SerializeField]
    private GameObject deleteAlertPanel;

    [Header("Scenes ID")]
    [SerializeField]
    private int charactersCreationScene;
    [SerializeField]
    private int levelSelectionScene;

    private bool overwriteSave = false;

    private GameObject activePanel;

    private bool firstLaunch;

    // save manager
    private XmlManager xmlManager;

    private void Awake() {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        firstLaunch = GM.GetFirstLauch();

        if (firstLaunch == true)
        {
            StartCoroutine(FirstLaunchCoroutine());
        }

        // access to xml
        xmlManager = new XmlManager();

        if(xmlManager.DoSaveExists())
        {
            continueButton.GetComponent<Button>().interactable = true;

            overwriteSave = true;

            titlePanel.GetComponent<UIMenuPanel>().SetFirstActiveButton(continueButton);
        }
    }

    private IEnumerator FirstLaunchCoroutine()
    {
        e13Panel.SetActive(true);
        titlePanel.SetActive(false);

        yield return new WaitForSecondsRealtime(3.0f);

        GM.SetFirstLaunch();

        e13Panel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void NewGameClick()
    {
        if (overwriteSave == true)
        {
            ShowOverwriteAlert();
        }
        else
        {
            NewGame();
        }
    }

    public void ShowOverwriteAlert()
    {
        overwriteAlertPanel.SetActive(true);
        titlePanel.SetActive(false);
        activePanel = overwriteAlertPanel;
    }

    public void HideOverwriteAlert()
    {
        overwriteAlertPanel.SetActive(false);
        titlePanel.SetActive(true);
        activePanel = titlePanel;
    }

    public void ShowDeleteAlert()
    {
        deleteAlertPanel.SetActive(true);
        preferencesPanel.SetActive(false);
        activePanel = deleteAlertPanel;
    }

    public void HideDeleteAlert(bool delete)
    {
        deleteAlertPanel.SetActive(false);

        if (delete == true)
        {
            GM.SetGlobalHighScore(0);

            titlePanel.SetActive(true);
            activePanel = titlePanel;
        }
        else
        {
            preferencesPanel.SetActive(true);
            activePanel = preferencesPanel;
        }

    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        titlePanel.SetActive(false);
        activePanel = panel;
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
        titlePanel.SetActive(true);
        activePanel = titlePanel;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(charactersCreationScene);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(levelSelectionScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetActivePanel(GameObject panel)
    {
        activePanel = panel;
    }

    public GameObject GetActivePanel()
    {
        return activePanel;
    }
}
