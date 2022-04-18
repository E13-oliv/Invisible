using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCharacterSwapManager : MonoBehaviour
{
    // managers
    private GameManager GM;

    [Header("Manager")]
    [SerializeField]
    private LevelSelectionManager levelSelectionManager;

    [Header("Characters")]
    [SerializeField]
    private Sprite[] characterSprites;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Text characterNameUI;
    [SerializeField]
    private GameObject[] behaviorIcons;
    [SerializeField]
    private GameObject[] skillsIcons;

    private string[] charactersNames;

    private string[] characters;

    private int characterID;
    private static int numOfChar = 4;

    private void Start()
    {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        characterID = GM.GetActiveCharacterNum();

        // get charactersfeatures
        characters = GM.GetCharacterFeatures();

        // get characters names, behaviors and skills
        charactersNames = GM.GetCharactersFirstNames();

        // load characters sprites
        for (int i = 0; i < characters.Length; i++)
        {
            int spriteNum = int.Parse(characters[i].Substring(3, 1)) + 1;
            characterSprites[i] = Resources.Load<Sprite>("LevelSelection/character0" + spriteNum + "-active");
        }

        CharacterDisplay();
    }

    private void Update()
    {
        characterImage.sprite = characterSprites[characterID];

        GM.SetActiveCharacter(characters[characterID]);
        GM.SetActiveCharacterNum(characterID);
    }

    private void CharacterDisplay()
    {
        // display selected character name
        int nameID = int.Parse(characters[characterID].Substring(2, 1));
        characterNameUI.text = charactersNames[nameID];

        // hide active behavior icons
        for (int i = 0; i < behaviorIcons.Length; i++)
        {
            behaviorIcons[i].SetActive(false);
        }

        // display bahavior icons
        int behaviorID = int.Parse(characters[characterID].Substring(0, 1));

        switch (behaviorID)
        {
            case 0:
                behaviorIcons[0].SetActive(true);
                behaviorIcons[1].SetActive(true);
                break;

            case 1:
                behaviorIcons[0].SetActive(true);
                break;

            case 2:
                behaviorIcons[2].SetActive(true);
                break;

            case 3:
                behaviorIcons[2].SetActive(true);
                behaviorIcons[3].SetActive(true);
                break;
        }

        // hide active behavior icons
        for (int i = 0; i < skillsIcons.Length; i++)
        {
            skillsIcons[i].SetActive(false);
        }

        // display skill icon
        int skillID = int.Parse(characters[characterID].Substring(1, 1));

        skillsIcons[skillID].SetActive(true);
    }

    public void nextChar()
    {
        if (characterID == numOfChar - 1)
        {
            characterID = 0;
        }
        else
        {
            characterID++;
        }

        levelSelectionManager.SetCharacterNum(characterID);

        CharacterDisplay();
    }

    public void prevChar()
    {
        if (characterID == 0)
        {
            characterID = numOfChar -1;
        }
        else
        {
            characterID--;
        }

        levelSelectionManager.SetCharacterNum(characterID);

        CharacterDisplay();
    }
}
