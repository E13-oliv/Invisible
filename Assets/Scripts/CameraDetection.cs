using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    [SerializeField]
    private CameraRaycast cameraRayCast;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cameraRayCast.GetComponent<CameraRaycast>().PlayerEnters();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cameraRayCast.GetComponent<CameraRaycast>().PlayerExits();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cameraRayCast.GetComponent<CameraRaycast>().PlayerEnters();
        }
    }
}
