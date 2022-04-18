using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Platforms : MonoBehaviour
{
    [Header("Bahevior")]
    [SerializeField]
    private bool jumpDown;

    [Header("Foreground Objects")]
    [SerializeField]
    private GameObject[] foregroundsObjects;
    [SerializeField]
    private int foregroundsObjectsOrder;

    private bool playerIsOnPlatform;
    private bool playerIsJumpingDown;

    private BoxCollider2D platformCollider;
    private int platformSortingOrder;

    private void Start()
    {
        this.platformCollider = GetComponent<BoxCollider2D>();
        platformSortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetAxis("Vertical") < 0 && CrossPlatformInputManager.GetButtonDown("Jump") && playerIsOnPlatform == true)
        {
            if (jumpDown == true)
            {
                platformCollider.isTrigger = true;
                playerIsJumpingDown = true;

                foreach (GameObject fgObjects in foregroundsObjects)
                {
                    fgObjects.GetComponent<SpriteRenderer>().sortingOrder = platformSortingOrder;
                }

                StartCoroutine(PlayerJumpsDown());
            }
        }
        else if (playerIsOnPlatform == true && playerIsJumpingDown == false)
        {
            foreach (GameObject fgObjects in foregroundsObjects)
            {
                fgObjects.GetComponent<SpriteRenderer>().sortingOrder = foregroundsObjectsOrder;
            }
        }
        else
        {
            foreach (GameObject fgObjects in foregroundsObjects)
            {
                fgObjects.GetComponent<SpriteRenderer>().sortingOrder = platformSortingOrder;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsOnPlatform = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (jumpDown == true)
        {
            if (collision.tag == "PlayerCollider")
            {
                platformCollider.isTrigger = false;

                foreach (GameObject fgObjects in foregroundsObjects)
                {
                    fgObjects.GetComponent<SpriteRenderer>().sortingOrder = foregroundsObjectsOrder;
                }
            }
        }
    }

    private IEnumerator PlayerJumpsDown()
    {
        yield return new WaitForSeconds(.5f);
        playerIsJumpingDown = false;
    }
}
