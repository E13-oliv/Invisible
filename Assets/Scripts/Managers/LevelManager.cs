using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using Assets.Pixelation.Scripts;
using UnityEngine.Rendering.PostProcessing;

public class LevelManager : MonoBehaviour
{
    // managers
    private GameManager GM;
    private AudioManager AM;

    private GameObject player;

    private string activeCharacterID;

    [Header("Level")]
    [SerializeField]
    private int levelID;
    [SerializeField]
    private float levelGreatTime;
    [SerializeField]
    private float levelBadTime;

    [Header("Cameras")]
    [SerializeField]
    private GameObject mainCamera;
    private Pixelation mainCameraPixelation;
    [SerializeField]
    private PostProcessVolume postProcessingVolume;
    private ColorGrading colorGradingLayer;

    [Header("Player Controllers")]
    [SerializeField]
    private GameObject[] playerControllers;

    [Header("Music")]
    [SerializeField]
    private AudioClip levelMusic;

    [Header("UI")]
    [SerializeField]
    private Text cityNameUI;
    [SerializeField]
    private Text cityNameShadowUI;
    [SerializeField]
    private Text cityNameScoreUI;
    [SerializeField]
    private GameObject cityCamerasUI;
    [SerializeField]
    private GameObject[] cityCamerasIconsUI;
    [SerializeField]
    private Text characterNameUI;
    [SerializeField]
    private Text characterNameShadowUI;
    [SerializeField]
    private GameObject[] characterIcons;
    [SerializeField]
    private Sprite[] characterIconSprites;
    [SerializeField]
    private Text timerUI;
    [SerializeField]
    private Text timerShadowUI;
    [SerializeField]
    private Image anxietyBarUI;
    [SerializeField]
    private Text countdownUI;
    [SerializeField]
    private Text countdownShadowUI;
    [SerializeField]
    private GameObject countdownUIGO;
    [SerializeField]
    private GameObject countdownShadowUIGO;

    private int skillIconPos = 0;

    [Header("Score UI")]
    [SerializeField]
    private Text scoreTime;
    [SerializeField]
    private Text scoreTimeScore;
    [SerializeField]
    private Text scoreAnxiety;
    [SerializeField]
    private Text scoreAnxietyScore;
    [SerializeField]
    private Text scoreScore;
    [SerializeField]
    private GameObject newHighScore;

    [Header("Panels")]
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject deathPanel;
    [SerializeField]
    private GameObject warningPanel;
    [SerializeField]
    private GameObject scorePanel;

    private string[] citiesNames;
    private int[] citiesCCTVs;

    private int characterNum;

    private int characterBehaviorID;
    private int characterSkillID;
    private int characterNameID;
    private string[] charactersNames;
    private string characterName;

    private float characterAnxiety;
    private string characterAnxietyString;

    private float lowSpeed = 5f;
    private float normalSpeed = 6f;
    private float highSpeed = 7f;
    private float playerSpeed;

    private float visionChangeAnxietyLimit = 0.6f;
    private float visionChangeRatio;

    private float pixelationMax = 300f;
    private float pixelationMin = 120f;
    private float pixelationGap;

    private float saturationAnxietyMax = -100f;

    private int inCameraGazeNum = 0;
    private float levelTime = 0.0f;
    private string levelTimeString;

    private float anxietyHighGain = 12f;
    private float anxietyLowGain = 6f;
    private float anxietyHighLoss = -3f;
    private float anxietyLowLoss = -1.5f;
    private float anxietyMax = 100;
    private float anxietyBarWidth = 420f;

    private float timeMaxScore = 1000f;
    private float anxietyMaxScore = 1000f;

    private int levelScore;
    private int timeScore;
    private int anxietyScore;

    private Color bronze = new Color(1f, .6f, .4f, 1f);
    private Color silver = new Color(.65f, .65f, .65f, 1f);
    private Color gold = new Color(1f, .8f, 0f, 1f);

    private int countdownDuration = 3;
    private bool isCountdownActive = true;

    private bool gamePaused = false;

    private void Awake()
    {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        // get cities names
        citiesNames = GM.GetCitiesNames();

        // display city name (GUI and score panel)
        cityNameUI.text = citiesNames[levelID];
        cityNameShadowUI.text = citiesNames[levelID];
        cityNameScoreUI.text = citiesNames[levelID];

        // display cctv cameras icons
        citiesCCTVs = GM.GetCitiesCCTVs();

        int numOfCameras = citiesCCTVs[levelID];

        switch (numOfCameras)
        {
            case 1:
                cityCamerasUI.transform.position = new Vector3(cityCamerasUI.transform.position.x + 40, cityCamerasUI.transform.position.y, cityCamerasUI.transform.position.z);
                cityCamerasIconsUI[1].SetActive(true);
                break;
            case 2:
                cityCamerasIconsUI[1].SetActive(true);
                cityCamerasIconsUI[2].SetActive(true);
                break;
            case 3:
                cityCamerasUI.transform.position = new Vector3(cityCamerasUI.transform.position.x + 40, cityCamerasUI.transform.position.y, cityCamerasUI.transform.position.z);
                cityCamerasIconsUI[0].SetActive(true);
                cityCamerasIconsUI[1].SetActive(true);
                cityCamerasIconsUI[2].SetActive(true);
                break;
            case 4:
                cityCamerasIconsUI[0].SetActive(true);
                cityCamerasIconsUI[1].SetActive(true);
                cityCamerasIconsUI[2].SetActive(true);
                cityCamerasIconsUI[3].SetActive(true);
                break;
        }

        // get active character and num (0-3)
        activeCharacterID = GM.GetActiveCharacter();
        characterNum = GM.GetActiveCharacterNum();

        // get character behavior, skill and name ID
        characterBehaviorID = int.Parse(activeCharacterID.Substring(0, 1));
        characterSkillID = int.Parse(activeCharacterID.Substring(1, 1));
        characterNameID = int.Parse(activeCharacterID.Substring(2, 1));
        charactersNames = GM.GetCharactersFirstNames();

        // display character name
        characterName = charactersNames[characterNameID];
        characterNameUI.text = characterName;
        characterNameShadowUI.text = characterName;

        // display character behavior icons
        switch (characterBehaviorID)
        {
            case 0:
                characterIcons[0].GetComponent<Image>().sprite = characterIconSprites[0];
                characterIcons[1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                characterIcons[1].GetComponent<Image>().sprite = characterIconSprites[0];
                skillIconPos = 2;
                break;

            case 1:
                characterIcons[0].GetComponent<Image>().sprite = characterIconSprites[0];
                skillIconPos = 1;
                break;

            case 2:
                characterIcons[0].GetComponent<Image>().sprite = characterIconSprites[1];
                skillIconPos = 1;
                break;

            case 3:
                characterIcons[0].GetComponent<Image>().sprite = characterIconSprites[1];
                characterIcons[1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                characterIcons[1].GetComponent<Image>().sprite = characterIconSprites[1];
                skillIconPos = 2;
                break;
        }

        // display character skill
        if (characterSkillID != 0)
        {
            characterIcons[skillIconPos].GetComponent<Image>().color = new Color(1, 1, 1, 1);

            switch (characterSkillID)
            {
                case 1:
                    characterIcons[skillIconPos].GetComponent<Image>().sprite = characterIconSprites[2];
                    break;

                case 2:
                    characterIcons[skillIconPos].GetComponent<Image>().sprite = characterIconSprites[3];
                    break;

                case 3:
                    characterIcons[skillIconPos].GetComponent<Image>().sprite = characterIconSprites[4];
                    break;
            }
        }

        // activating right player controller
        GameObject activePlayerController = playerControllers[int.Parse(activeCharacterID.Substring(3, 1))];

        mainCamera.GetComponent<CameraMovements>().setPlayer(activePlayerController);
        activePlayerController.SetActive(true);

        // set player speed
        if (characterSkillID == 2) // old person
        {
            playerSpeed = lowSpeed;
        }
        else if (characterSkillID == 3) // young person
        {
            playerSpeed = highSpeed;
        }
        else
        {
            playerSpeed = normalSpeed;
        }

        activePlayerController.GetComponent<PlatformerCharacter2D>().m_MaxSpeed = playerSpeed;
    }

    private void Start()
    {
        // find GameManager
        GameObject audioManagerGameObject = GameObject.Find("AudioManager");
        AM = audioManagerGameObject.GetComponent<AudioManager>();

        // remove pause mode in case it is on
        GM.SetPauseState(false);

        player = GameObject.FindGameObjectWithTag("Player");

        // disable user controls (during countdown)
        player.GetComponent<Platformer2DUserControl>().enabled = false;

        mainCameraPixelation = mainCamera.GetComponent<Pixelation>();
        pixelationGap = pixelationMax - pixelationMin;

        visionChangeRatio = 1 / (1 - visionChangeAnxietyLimit);

        postProcessingVolume.profile.TryGetSettings<ColorGrading>(out colorGradingLayer);

        StartCoroutine(CountdownCoroutine(countdownDuration));

        AM.LevelStartAudio(levelMusic);
    }

    private void FixedUpdate()
    {
        //inGazeUI.text = inCameraGazeNum.ToString();

        if (isCountdownActive == false)
        {
            // Character anxiety management
            if (inCameraGazeNum > 0)
            {
                if(characterBehaviorID == 0)
                {
                    characterAnxiety += Time.deltaTime * anxietyHighGain * inCameraGazeNum;
                }
                else if (characterBehaviorID == 1)
                {
                    characterAnxiety += Time.deltaTime * anxietyLowGain * inCameraGazeNum;
                }
                else if (characterBehaviorID == 2)
                {
                    characterAnxiety += Time.deltaTime * anxietyLowLoss * inCameraGazeNum;
                }
                else
                {
                    characterAnxiety += Time.deltaTime * anxietyHighLoss * inCameraGazeNum;
                }
            }
            else
            {
                if (characterBehaviorID == 0)
                {
                    characterAnxiety += Time.deltaTime * anxietyHighLoss;
                }
                else if (characterBehaviorID == 1)
                {
                    characterAnxiety += Time.deltaTime * anxietyLowLoss;
                }
                else if (characterBehaviorID == 2)
                {
                    characterAnxiety += Time.deltaTime * anxietyLowGain;
                }
                else
                {
                    characterAnxiety += Time.deltaTime * anxietyHighGain;
                }
            }

            characterAnxiety = Mathf.Clamp(characterAnxiety, 0, anxietyMax);
            characterAnxietyString = (Mathf.Round(characterAnxiety * 10) / 10).ToString("#0.0");
            //anxietyUI.text = characterAnxietyString;

            levelTime += Time.deltaTime;
        }

        if (levelTime == 0)
        {
            timerUI.text = "0.0";
            timerShadowUI.text = "0.0";
        }
        else
        {
            levelTimeString = (Mathf.Round(levelTime * 10) / 10).ToString("#0.0");
            timerUI.text = levelTimeString;
            timerShadowUI.text = levelTimeString;
        }

        float anxietyFactor = characterAnxiety / anxietyMax;

        // if anxiety reach the limit of vision change
        if (anxietyFactor > visionChangeAnxietyLimit)
        {
            // anxiety visual management
            float visionChangeFactor = (1 - anxietyFactor) * visionChangeRatio;

            float blockCount = pixelationGap * visionChangeFactor + pixelationMin;

            mainCameraPixelation.enabled = true;
            mainCameraPixelation.BlockCount = blockCount;

            colorGradingLayer.saturation.value = saturationAnxietyMax + visionChangeFactor * Mathf.Abs(saturationAnxietyMax);

            // anxiety audio management
            float audioChangeFactor = Mathf.Abs(visionChangeFactor - 1);
            AM.SetHeartBeat(audioChangeFactor);
        }
        else
        {
            // anxiety visual management
            colorGradingLayer.saturation.value = 0;
            mainCameraPixelation.enabled = false;

            // anxiety audio management
            AM.SetHeartBeat(0);
        }

        // anxiety bar display
        Color anxietyGood = Color.green;
        Color anxietyMedium = Color.yellow;
        Color anxietyBad = Color.red;

        float anxietyRatio = characterAnxiety / anxietyMax;

        if (anxietyRatio <= 0.5f)
        {
            anxietyBarUI.color = Color.Lerp(anxietyGood, anxietyMedium, anxietyRatio * 2);
        }
        else
        {
            anxietyBarUI.color = Color.Lerp(anxietyMedium, anxietyBad, (anxietyRatio -0.5f) * 2);
        }


        float anxietyBarW = anxietyRatio * anxietyBarWidth;
        anxietyBarW = Mathf.Round(anxietyBarW / 10) * 10;
        anxietyBarUI.rectTransform.sizeDelta = new Vector2(anxietyBarW, anxietyBarUI.rectTransform.sizeDelta.y);

        // if anxiety reach its maximum
        if (characterAnxiety >= anxietyMax)
        {
            Death();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        int countdown = seconds;

        while (countdown >= 0)
        {
            if(countdown != 0)
            {
                countdownUI.text = countdown.ToString();
                countdownShadowUI.text = countdown.ToString();
                yield return new WaitForSeconds(1);
            }
            else
            {
                countdownUI.text = "GO";
                countdownShadowUI.text = "GO";
                player.GetComponent<Platformer2DUserControl>().enabled = true;
                yield return new WaitForSeconds(0.5f);
            }

            countdown--;
        }

        countdownUIGO.SetActive(false);
        countdownShadowUIGO.SetActive(false);
        isCountdownActive = false;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        pausePanel.SetActive(false);
        warningPanel.SetActive(false);
        GM.SetPauseState(false);
        player.GetComponent<Platformer2DUserControl>().enabled = true;
    }

    public void Death()
    {
        // anxiety audio management
        AM.SetHeartBeat(0);

        deathPanel.SetActive(true);
        GM.SetPauseState(true);
    }

    public void PauseGame()
    {
        gamePaused = true;
        pausePanel.SetActive(true);
        GM.SetPauseState(true);
        player.GetComponent<Platformer2DUserControl>().enabled = false;
    }

    public void DisplayWarning()
    {
        warningPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void HideWarning()
    {
        warningPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void GoToLevelSelection(int levelSelectionSceneID)
    {
        AM.BackToTitleAudio();
        SceneManager.LoadScene(levelSelectionSceneID);
    }

    public void SetInCameraGazeNum(int num)
    {
        inCameraGazeNum += num;
    }

    public void EndOfLevel()
    {
        // pause game
        GM.SetPauseState(true);

        // anxiety audio management
        AM.SetHeartBeat(0);

        scorePanel.SetActive(true);

        scoreTime.text = levelTimeString;
        scoreAnxiety.text = characterAnxietyString;

        ScoreCalculation();

        // display score texts and change color (gold, silver, bronze)
        scoreTimeScore.text = timeScore.ToString();

        if (timeScore > timeMaxScore * .8)
        {
            scoreTimeScore.color = gold;
        }
        else if (timeMaxScore > timeMaxScore / 2)
        {
            scoreTimeScore.color = silver;
        }
        else 
        {
            scoreTimeScore.color = bronze;
        }

        scoreAnxietyScore.text = anxietyScore.ToString();

        if (anxietyScore > timeMaxScore * .8)
        {
            scoreAnxietyScore.color = gold;
        }
        else if (anxietyScore > anxietyMaxScore / 2)
        {
            scoreAnxietyScore.color = silver;
        }
        else
        {
            scoreTimeScore.color = bronze;
        }

        scoreScore.text = levelScore.ToString();

        // get level high score
        int scoreID = levelID * 4 + characterNum;
        int levelHighScore = GM.GetLevelScore(scoreID);

        // if new high score –> save
        if (levelScore > levelHighScore)
        {
            newHighScore.SetActive(true);
            scoreScore.color = gold;

            GM.SetLevelScore(levelScore, scoreID);
            GM.SaveFile("save");
        }
    }

    private void ScoreCalculation()
    {
        if (levelTime < levelGreatTime)
        {
            timeScore = (int)timeMaxScore;
        }
        else if(levelTime < levelBadTime)
        {
            float timeGap = levelBadTime - levelGreatTime;
            float timeInTimeGap = levelTime - levelGreatTime;
            timeScore = (int)Mathf.Round((timeGap - timeInTimeGap) / timeGap * timeMaxScore);
        }
        else
        {
            timeScore = 0;
        }

        anxietyScore = (int)((anxietyMax - characterAnxiety) / anxietyMax * anxietyMaxScore);

        levelScore = timeScore + anxietyScore;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetCharacterBehavior()
    {
        return characterBehaviorID;
    }

    public int GetCharacterSkill()
    {
        return characterSkillID;
    }
}