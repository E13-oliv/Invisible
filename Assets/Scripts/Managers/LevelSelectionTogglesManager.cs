using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionTogglesManager : MonoBehaviour
{

    [Header("Level Toggle")]
    [SerializeField]
    private int levelSceneID;

    [Header("Character Toggle")]
    [SerializeField]
    private string character;

    public void SetCharacter(string characterID)
    {
        character = characterID;
    }

    public string GetCharacter()
    {
        return character;
    }

    public int GetLevelSceneID()
    {
        return levelSceneID;
    }
}
