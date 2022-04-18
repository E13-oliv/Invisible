using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private GameObject player;

    [Header("Camera offset")]
    [SerializeField]
    private float camXOffset;
    [SerializeField]
    private float camYOffset;

    private float camMovementSpeed = 9.0f;

    [Header("Level Bounds")]
    [SerializeField]
    private GameObject levelLeftBound;
    [SerializeField]
    private GameObject levelRightBound;

    private void Update()
    {
        // set new camera X position
        float targetXpos = player.transform.position.x + camXOffset;

        float speed;

        speed = camMovementSpeed;

        Vector3 targetPos = new Vector3(targetXpos, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Max(levelLeftBound.transform.position.x,Mathf.Min(levelRightBound.transform.position.x,transform.position.x)), transform.position.y, transform.position.z);
    }

    public void setPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
