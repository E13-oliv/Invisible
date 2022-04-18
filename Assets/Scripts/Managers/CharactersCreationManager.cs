using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharactersCreationManager : MonoBehaviour
{
    // managers
    private GameManager GM;
    private XmlManager xmlManager;

    [Header("Characters boxes")]
    [SerializeField]
    private GameObject[] characters;

    [SerializeField]
    private GameObject[] charactersBehaviorText;

    [SerializeField]
    private GameObject[] charactersSkillText;

    [SerializeField]
    private GameObject[] behaviorIcons;

    [SerializeField]
    private GameObject[] skillIcons;

    [SerializeField]
    private Sprite[] charactersModel;

    [Header("Scenes")]
    [SerializeField]
    private int levelSelectionSceneID;

    private string[] charactersFeatures = new string[] { "0", "0", "0", "0" };

    private string[] charactersBehaviorsString;
    private string[] charactersSkillsString;
    private string[] charactersFirstNamesString;

    private string[] charactersBehaviors = new string[] { "0", "1", "2", "3" };
    private string[] charactersSkills = new string[] { "0", "1", "2", "3" };
    private string[] charactersName = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    private string[] charactersModels = new string[] { "0", "1", "2", "3" };

    private void Start()
    {
        // access to xml
        xmlManager = new XmlManager();

        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        if (xmlManager.DoSaveExists() == false)
        {
            xmlManager.NewSaveFile();
        }

        charactersBehaviorsString = GM.GetCharactersBehaviors();
        charactersSkillsString = GM.GetCharactersSkills();
        charactersFirstNamesString = GM.GetCharactersFirstNames();

        CreatCharacters();
    }

    private void CreatCharacters()
    {
        int i = 0;

        // arrays randomization
        charactersBehaviors = RandomizeArray(charactersBehaviors);
        charactersSkills = RandomizeArray(charactersSkills);
        charactersName = RandomizeArray(charactersName);
        charactersModels = RandomizeArray(charactersModels);

        // characters features
        // x___ : behavior (0 -> 3)
        // 0 : doesn't want to bn seen++
        // 1 : doesn't want to be seen
        // 2 : want to be seen
        // 3 : want to be seen++
        // _x__ : skills and features (0 -> 3)
        // 0 : none
        // 1 : person of interest
        // 2 : old person
        // 3 : young person
        // __x_ : name (0 –> 9)
        // 0 : Mary
        // 1 : John
        // 2 : Linda
        // 3 : Jesse
        // 4 : Karen
        // 5 : Dany
        // 6 : Chris
        // 7 : Erin
        // 8 : Riley
        // 9 : Logan
        // ___x : model (0 -> 3)
        // 0 : char_01
        // 1 : char_02
        // 2 : char_03
        // 3 : char_04

        foreach (GameObject character in characters)
        {
            string characterFeatures = charactersBehaviors[i] + charactersSkills[i] + charactersName[i] + charactersModels[i];

            // character feature storage
            charactersFeatures[i] = characterFeatures;

            // character sprite display
            character.GetComponentInChildren<Image>().sprite = charactersModel[int.Parse(charactersModels[i])];

            // character first name display
            character.GetComponent<Text>().text = charactersFirstNamesString[int.Parse(charactersName[i])];

            int behaviorID = int.Parse(charactersBehaviors[i]);
            int skillID = int.Parse(charactersSkills[i]);

            // character behavior and skill display
            string behavior = charactersBehaviorsString[behaviorID];
            string skill = charactersSkillsString[skillID];

            charactersBehaviorText[i].GetComponent<Text>().text = behavior;
            charactersSkillText[i].GetComponent<Text>().text = skill;

            // behavior and skills icons display
            switch (behaviorID)
            {
                case 0:
                    behaviorIcons[i * 4].SetActive(true);
                    behaviorIcons[i * 4 + 1].SetActive(true);
                    break;
                case 1:
                    behaviorIcons[i * 4].SetActive(true);
                    break;
                case 2:
                    behaviorIcons[i * 4 + 2].SetActive(true);
                    break;
                case 3:
                    behaviorIcons[i * 4 + 2].SetActive(true);
                    behaviorIcons[i * 4 + 3].SetActive(true);
                    break;
            }

            switch (skillID)
            {
                case 0:
                    skillIcons[i * 4].SetActive(true);
                    break;
                case 1:
                    skillIcons[i * 4 + 1].SetActive(true);
                    break;
                case 2:
                    skillIcons[i * 4 + 2].SetActive(true);
                    break;
                case 3:
                    skillIcons[i * 4 + 3].SetActive(true);
                    break;
            }
            i++;
        }
    }

    private string[] RandomizeArray(string[] arr)
    {
        for (var i = arr.Length - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }

        return arr;
    }

    public void Reroll()
    {
        // reset behavior and skill icons
        for (int i = 0; i < behaviorIcons.Length; i++)
        {
            behaviorIcons[i].SetActive(false);
        }

        for (int i = 0; i < skillIcons.Length; i++)
        {
            skillIcons[i].SetActive(false);
        }

        CreatCharacters();
    }

    public void Next()
    {
        xmlManager.SaveCharacters(charactersFeatures);
        xmlManager.SaveFile("");
        SceneManager.LoadScene(levelSelectionSceneID);
    }
}
