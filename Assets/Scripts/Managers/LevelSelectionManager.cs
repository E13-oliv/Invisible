using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    // managers
    private GameManager GM;

    [Header("Toggle Groups")]
    [SerializeField]
    private ToggleGroup levelsToggleGroup;
    [SerializeField]
    private ToggleGroup charactersToggleGroup;

    [Header("City")]
    [SerializeField]
    private Text cityName;
    [SerializeField]
    private Text cityCCTV;

    [Header("Character")]
    [SerializeField]
    private Text characterName;
    [SerializeField]
    private Text characterBehavior;
    [SerializeField]
    private Text characterSkill;

    [Header("Scores")]
    [SerializeField]
    private GameObject bestScoreTitleText;
    [SerializeField]
    private Text bestScoreText;
    [SerializeField]
    private Text globalScoreText;
    [SerializeField]
    private Text globalHighScoreText;

    private Color gold = new Color(1f, .8f, 0f, 1f);

    private int[] allScores;
    private int globalScore;
    private int globalHighScore;

    //private string[] characters;

    private int characterNum;

    private int levelSceneID;
    private int levelID;

    //private int levelScore;

    private void Start()
    {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        GM.LoadFile("save");

        // calculating global score and get global high score
        allScores = GM.GetScores();
        globalHighScore = GM.GetGlobalHighScore();

        for (int i = 0; i < allScores.Length; i++)
        {
            globalScore += allScores[i];
        }

        globalScoreText.text = globalScore.ToString();

        // if global score is new high score
        if (globalScore >= globalHighScore)
        {
            GM.SetGlobalHighScore(globalScore);
            globalHighScore = globalScore;

            if (globalScore > 0)
            {
                globalHighScoreText.color = gold;
            }
        }

        globalHighScoreText.text = globalHighScore.ToString();
    }

    private void Update()
    {
        // set and display global high score
        int scoreID = levelID * 4 + characterNum;
        int bestScore = GM.GetLevelScore(scoreID);

        bestScoreText.text = bestScore.ToString();
    }

    public void SetSelectedLevel(int selectedLevelID, int selectedLevelSceneID)
    {
        levelID = selectedLevelID;
        levelSceneID = selectedLevelSceneID;
    }

    public void SetCharacterNum(int num)
    {
        characterNum = num;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelSceneID);
    }

    public void GoToMainMenu(int menuSceneID)
    {
        SceneManager.LoadScene(menuSceneID);
    }
}