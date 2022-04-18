using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundsParallax : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField]
    private Camera mainCamera;

    [Header("Parallax factor")]
    [SerializeField]
    private float parallaxFactor;

    private Material backgroundMaterial;

    private void Start()
    {
        backgroundMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        float newXPos = mainCamera.transform.position.x * parallaxFactor;
        backgroundMaterial.mainTextureOffset = new Vector2(newXPos, 0);
    }
}
