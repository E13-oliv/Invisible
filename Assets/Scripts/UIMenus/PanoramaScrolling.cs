using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoramaScrolling : MonoBehaviour
{
    private Material backgroundMaterial;

    private float newXPos = 0f;
    private float scrollingSpeed = 2f;

    private void Start()
    {
        backgroundMaterial = GetComponent<Image>().material;
    }

    private void Update()
    {
        newXPos += scrollingSpeed;
        backgroundMaterial.mainTextureOffset = new Vector2(newXPos, 0);
    }
}
