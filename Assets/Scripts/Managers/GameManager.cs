using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // managers
    private AudioManager AM;

    private bool gamePaused = false;

    private bool firstLaunch = true;

    // congif manager
    private XmlManager xmlManager;

    // characters variables
    [SerializeField]
    private string activeCharacter;
    private int activeCharacterNum;
    private string[] charactersFirstNames = new string[] { "Mary", "John", "Linda", "Jesse", "Karen", "Dany", "Chris", "Erin", "Riley", "Logan" };
    private string[] charactersBehaviors = new string[] { "Doesn't want\nto be seen", "Doesn't like\nto be seen", "Likes to be seen", "Wants to be seen" };
    private string[] charactersSkills = new string[] { " ", "Person of Interest", "Slow", "Fast" };

    // cities variables
    private int activeCityNum;
    private string[] citiesNames = new string[] { "Shanghai", "London", "Moscow", "Paris" };
    private int[] citiesCCTVs = new int[] { 4, 3, 2, 1 };

    [SerializeField]
    private Text testLog;

    public void TestLog(string content)
    {
        testLog.text = content;
    }

    private void Start()
    {
        // find AudioManager
        GameObject audioManagerGameObject = GameObject.Find("AudioManager");
        AM = audioManagerGameObject.GetComponent<AudioManager>();

        // access to xml
        xmlManager = new XmlManager();

        // load config
        LoadFile("config");

        // set volumes in audio mixer
        AM.SetVolume("MusicVol", GetMusicVolume());
        AM.SetVolume("SfxVol", GetSfxVolume());
    }

    private void Update()
    {
        // pause mode
        if (gamePaused == true)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    // PUBLIC METHODS
    public void LoadFile(string fileType)
    {
        xmlManager.LoadFile(fileType);
    }

    public void SaveFile(string fileType)
    {
        xmlManager.SaveFile(fileType);
    }

    public int GetMusicVolume()
    {
        return xmlManager.GetMusicVolume();
    }

    public void SetMusicVolume(int volume)
    {
        xmlManager.SetMusicVolume(volume);
        xmlManager.SaveFile("config");
    }

    public int GetSfxVolume()
    {
        return xmlManager.GetSfxVolume();
    }

    public void SetSfxVolume(int volume)
    {
        xmlManager.SetSfxVolume(volume);
        xmlManager.SaveFile("config");
    }

    public bool GetPauseState()
    {
        return gamePaused;
    }

    public void SetPauseState(bool state)
    {
        gamePaused = state;
    }

    public string GetActiveCharacter()
    {
        return activeCharacter;
    }

    public void SetActiveCharacter(string characterID)
    {
        activeCharacter = characterID;
    }

    public int GetActiveCharacterNum()
    {
        return activeCharacterNum;
    }

    public void SetActiveCharacterNum(int characterNum)
    {
        activeCharacterNum = characterNum;
    }

    public string[] GetCharacterFeatures()
    {
        return xmlManager.GetCharacters();
    }

    public string[] GetCharactersFirstNames()
    {
        return charactersFirstNames;
    }

    public string[] GetCharactersBehaviors()
    {
        return charactersBehaviors;
    }

    public string[] GetCharactersSkills()
    {
        return charactersSkills;
    }

    public int GetActiveCityNum()
    {
        return activeCityNum;
    }

    public void SetActiveCityNum(int cityNum)
    {
        activeCityNum = cityNum;
    }

    public string[] GetCitiesNames()
    {
        return citiesNames;
    }

    public int[] GetCitiesCCTVs()
    {
        return citiesCCTVs;
    }

    public int[] GetScores()
    {
        return xmlManager.GetScores();
    }

    public int GetLevelScore(int scoreID)
    {
        return xmlManager.GetLevelScore(scoreID);
    }

    public void SetLevelScore(int levelScore, int scoreID)
    {
        xmlManager.SetLevelScore(levelScore, scoreID);
    }

    public int GetGlobalHighScore()
    {
        return xmlManager.GetGlobalHighScore();
    }

    public void SetGlobalHighScore(int score)
    {
        xmlManager.SetGlobalHighScore(score);
    }

    public bool GetFirstLauch()
    {
        return firstLaunch;
    }

    public void SetFirstLaunch()
    {
        firstLaunch = false;
    }
}