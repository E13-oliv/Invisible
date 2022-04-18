using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCitySwapManager : MonoBehaviour
{
    // managers
    private GameManager GM;

    [Header("Manager")]
    [SerializeField]
    private LevelSelectionManager levelSelectionManager;

    [Header("City")]
    [SerializeField]
    private Sprite[] citySprites;
    [SerializeField]
    private Image cityImage;
    [SerializeField]
    private GameObject[] cityCameraIcons;
    [SerializeField]
    private Text cityNameUI;

    private string[] citiesNames;
    private int[] citiesCCTVs;

    private int cityID = 0;
    private static int numOfCities = 4;

    private void Start()
    {
        // find GameManager
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GM = gameManagerGameObject.GetComponent<GameManager>();

        cityID = GM.GetActiveCityNum();

        // get cities names and CCTVs
        citiesNames = GM.GetCitiesNames();
        citiesCCTVs = GM.GetCitiesCCTVs();

        // dynamicly get cities sprites
        for (int i = 0; i < citiesNames.Length; i++)
        {
            int spriteNum = i + 1;
            citySprites[i] = Resources.Load<Sprite>("LevelSelection/city0" + spriteNum + "-active");
        }

        CityDisplay(cityID);
    }

    private void CityDisplay(int cityID)
    {
        // hide all camera icons
        for (int i = 0; i < cityCameraIcons.Length; i++)
        {
            cityCameraIcons[i].SetActive(false);
        }

        // city camera icons display
        for (int i = 0; i < citiesCCTVs[cityID]; i++)
        {
            cityCameraIcons[i].SetActive(true);
        }

        // city sprite and name display
        cityImage.sprite = citySprites[cityID];
        cityNameUI.text = citiesNames[cityID];

        // + 3 to match with scene ID !SHAME!
        levelSelectionManager.SetSelectedLevel(cityID, cityID + 3);
    }

    public void nextCity()
    {
        if (cityID == numOfCities - 1)
        {
            cityID = 0;
        }
        else
        {
            cityID++;
        }

        GM.SetActiveCityNum(cityID);

        CityDisplay(cityID);
    }

    public void prevCity()
    {
        if (cityID == 0)
        {
            cityID = numOfCities - 1;
        }
        else
        {
            cityID--;
        }

        GM.SetActiveCityNum(cityID);

        CityDisplay(cityID);
    }
}
