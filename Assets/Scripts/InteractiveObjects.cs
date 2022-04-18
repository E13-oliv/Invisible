using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveObjects : MonoBehaviour
{
    [System.Serializable]
    private enum objectType
    {
        None = 0,
        LevelEnd = 1,
        DeathFall = 2
    };

    [Header("Object Type")]
    [SerializeField]
    private objectType interactiveObjectType;

    [Header("LevelEnd")]
    [SerializeField]
    private Object nextScene;

    private LevelManager LM;

    private void Start()
    {
        // find GameManager
        GameObject levelManagerGameObject = GameObject.Find("LevelManager");
        LM = levelManagerGameObject.GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerCollider")
        {
            if (interactiveObjectType.ToString() == "LevelEnd")
            {
                LevelEnd();
            }
            else if (interactiveObjectType.ToString() == "DeathFall")
            {
                Death();
            }
        }
    }

    private void LevelEnd()
    {
        LM.EndOfLevel();
    }

    private void Death()
    {
        LM.Death();
    }
}
