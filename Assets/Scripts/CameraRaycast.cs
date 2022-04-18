using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    private LevelManager LM;

    private bool isPlayerIn = false;
    private bool isPlayerSeen = false;

    private GameObject player;

    private int characterBehavior;
    private static int goodBehavior = 2;
    private int characterSkill;
    private static int poiID = 1;

    private enum cameraAngles
    {
        _30 = 30,
        _45 = 45,
        _60 = 60,
        _75 = 75,
        _90 = 90
    }

    [Header("CCTV Sprites")]
    [SerializeField]
    private GameObject cctvCameraRed;
    [SerializeField]
    private GameObject cctvCameraGreen;
    [SerializeField]
    private GameObject cctvCameraActive;
    [SerializeField]
    private GameObject cctvCameraCone;
    [SerializeField]
    private GameObject cctvCameraConeMesh;

    //private float cameraConeXScaleFactor = 1.54f;

    [Header("CCTV Cone Materials")]
    [SerializeField]
    private Material WhiteMaterial;
    [SerializeField]
    private Material RedMaterial;
    [SerializeField]
    private Material GreenMaterial;

    [Header("CCTV Settings")]
    [SerializeField]
    private float cameraDefaultAnglePosition;
    [SerializeField]
    private cameraAngles cameraAngleViewEnum;
    [SerializeField]
    private float camgleAngleMin = -45f;
    [SerializeField]
    private float camgleAngleMax = 45f;
    [SerializeField]
    private bool activeCamera;
    [SerializeField]
    private float cameraReturnSpeed = 2f;
    [SerializeField]
    private bool movingCamera;
    [SerializeField]
    private float cameraMovingSpeed = 2f;

    private float cameraAnglePosition;
    private float cameraAngleView;

    // is the camera following the player
    private bool cameraIsFollowing;

    // is the camera automaticly moving
    private bool cameraIsMoving;
    private bool cameraIsMovingUp;
    private Coroutine cameraIsMovingCR;

    // is the camera returning to its position (not moving camera)
    private bool cameraIsReturningToPosition;
    private Coroutine cameraIsReturningToPositionCR;

    // cameras cones
    private int numberOfPoints = 120;
    private float verticeCameraAnglePosition;
    private Vector2 lastVertice;
    private List<Vector2> coneVertices = new List<Vector2>();
    private Vector2[] coneVerticesArray;

    private void Start()
    {
        // find GameManager
        GameObject levelManagerGameObject = GameObject.Find("LevelManager");
        LM = levelManagerGameObject.GetComponent<LevelManager>();

        player = GameObject.FindWithTag("Player");

        // set base point (center of camera) for the camera cone 
        coneVertices.Add(new Vector2(transform.position.x, transform.position.y));

        if (movingCamera == true)
        {
            cameraIsMoving = true;
            cameraIsMovingCR = StartCoroutine(MoveCameraCoroutine());
        }

        cameraAngleView = (int)cameraAngleViewEnum;

        // set camera gaze cone horizontal scale
        float newXScale = 3;

        switch ((int)cameraAngleViewEnum)
        {
            case 30:
                newXScale = 3f;
                break;
            case 45:
                newXScale = 4.87f;
                break;
            case 60:
                newXScale = 6.75f;
                break;
            case 75:
                newXScale = 8.93f;
                break;
            case 90:
                newXScale = 11.63f;
                break;
        }

        cctvCameraCone.transform.localScale = new Vector3(newXScale, cctvCameraCone.transform.localScale.y, cctvCameraCone.transform.localScale.z);

        // active camera blinking led activation
        if (activeCamera == true)
        {
            cctvCameraActive.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        cameraAnglePosition = transform.eulerAngles.z;
        verticeCameraAnglePosition = cameraAnglePosition - 90;

        coneVertices = new List<Vector2>();
        coneVertices.Add(new Vector2(transform.position.x, transform.position.y));
        DrawCameraCones();

        if (isPlayerIn == true)
        {
            int layer_mask = LayerMask.GetMask("Player", "Platforms", "LevelBounds");

            Vector2 playerPos = player.transform.position;
            Vector2 cameraPos = transform.position;

            Vector2 direction = playerPos - cameraPos;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layer_mask);

            // Does the ray intersect any objects excluding the player layer
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                characterBehavior = LM.GetCharacterBehavior();
                characterSkill = LM.GetCharacterSkill();

                Debug.DrawRay(transform.position, direction, Color.red);
                if (characterBehavior < goodBehavior)
                {
                    cctvCameraConeMesh.GetComponent<MeshRenderer>().material = RedMaterial;
                    cctvCameraRed.SetActive(true);
                }
                else
                {
                    cctvCameraConeMesh.GetComponent<MeshRenderer>().material = GreenMaterial;
                    cctvCameraGreen.SetActive(true);
                }

                SetPlayerSeenState(true);

                if (characterSkill == poiID && activeCamera == true)
                {
                    cameraIsFollowing = true;

                    if (cameraIsReturningToPosition == true)
                    {
                        StopCoroutine(cameraIsReturningToPositionCR);
                        cameraIsReturningToPosition = false;
                    }

                    if (cameraIsMoving == true)
                    {
                        StopCoroutine(cameraIsMovingCR);
                        cameraIsMoving = false;
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.white);

                cctvCameraConeMesh.GetComponent<MeshRenderer>().material = WhiteMaterial;
                cctvCameraRed.SetActive(false);
                cctvCameraGreen.SetActive(false);

                SetPlayerSeenState(false);

                cameraIsFollowing = false;

                if (movingCamera == true)
                {
                    cameraIsMovingCR = StartCoroutine(MoveCameraCoroutine());
                    cameraIsMoving = true;
                }
            }

            if (cameraIsFollowing == true)
            {
                Vector3 dir = hit.collider.gameObject.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

                // angle limitations
                angle = Mathf.Clamp(angle, camgleAngleMin, camgleAngleMax);

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            cameraIsFollowing = false;

            if (movingCamera == true)
            {
                cameraIsMovingCR = StartCoroutine(MoveCameraCoroutine());
                cameraIsMoving = true;
            }
        }

        if (cameraIsFollowing == false && Mathf.RoundToInt(transform.localEulerAngles.z) != cameraDefaultAnglePosition && cameraIsReturningToPosition == false && movingCamera == false)
        {
            cameraIsReturningToPositionCR = StartCoroutine(ReplaceCameraCoroutine());

            cameraIsReturningToPosition = true;
        }

        if (movingCamera == true && cameraIsFollowing == false)
        {

        }
    }

    public void PlayerEnters()
    {
        isPlayerIn = true;
    }

    public void PlayerExits()
    {
        isPlayerIn = false;
        cctvCameraConeMesh.GetComponent<MeshRenderer>().material = WhiteMaterial;
        cctvCameraRed.SetActive(false);
        cctvCameraGreen.SetActive(false);

        SetPlayerSeenState(false);
    }

    private void SetPlayerSeenState(bool state)
    {
        if (isPlayerSeen != state)
        {
            if (state == true)
            {
                LM.SetInCameraGazeNum(1);
            }
            else
            {
                LM.SetInCameraGazeNum(-1);
            }
            isPlayerSeen = state;
        }
    }

    private void DrawCameraCones()
    {
        GetVertices();

        // cone drawing
        coneVerticesArray = coneVertices.ToArray();

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(coneVerticesArray);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[coneVerticesArray.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(coneVerticesArray[i].x, coneVerticesArray[i].y, 0);
        }

        // Create the mesh
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        cctvCameraConeMesh.transform.position = new Vector2(0, 0);
        cctvCameraConeMesh.GetComponent<MeshFilter>().mesh = mesh;
        cctvCameraConeMesh.GetComponent<MeshRenderer>().material = WhiteMaterial;
        cctvCameraConeMesh.GetComponent<MeshRenderer>().sortingLayerName = "CameraCones";
    }

    private void GetVertices()
    {
        int layer_mask = LayerMask.GetMask("LevelBounds", "Platforms");

        float angle = verticeCameraAnglePosition - cameraAngleView / 2;

        GameObject lastObject = null;
        int lastAdd = 0;

        for (int i = 1; i <= numberOfPoints; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layer_mask);

            if (hit.collider.gameObject != lastObject)
            {
                // if not first vertice
                if (lastObject != null && lastVertice != coneVertices[lastAdd])
                {
                    // last vertice from previous object
                    coneVertices.Add(lastVertice);
                    lastAdd++;
                }

                // check if same coordinates to prevent display bug
                if (lastVertice != new Vector2(hit.point.x, hit.point.y))
                {
                    // add first vertice of new object
                    coneVertices.Add(new Vector2(hit.point.x, hit.point.y));
                    lastAdd++;
                }

                // store collider
                lastObject = hit.collider.gameObject;
            }

            // store collider point
            lastVertice = new Vector2(hit.point.x, hit.point.y);

            angle = angle + cameraAngleView / numberOfPoints;
        }

        // store last vertice
        if (lastVertice != coneVertices[lastAdd])
        {
            coneVertices.Add(lastVertice);
        }
    }

    private IEnumerator MoveCameraCoroutine()
    {
        int fromAngle = Mathf.RoundToInt(transform.eulerAngles.z);
        int toAngle;

        if (fromAngle > 180)
        {
            fromAngle = fromAngle - 360;
        }

        while (cameraIsMoving == true)
        {
            if (fromAngle <= camgleAngleMin)
            {
                toAngle = fromAngle++;
                cameraIsMovingUp = true;
            }
            else if (fromAngle >= camgleAngleMax)
            {
                toAngle = fromAngle--;
                cameraIsMovingUp = false;
            }
            else
            {
                if (cameraIsMovingUp == true)
                {
                    toAngle = fromAngle++;
                }
                else
                {
                    toAngle = fromAngle--;
                }
            }

            transform.rotation = Quaternion.AngleAxis(toAngle, Vector3.forward);

            yield return new WaitForSeconds(0.1f / cameraMovingSpeed);
        }

        yield return false;
    }

    private IEnumerator ReplaceCameraCoroutine()
    {
        int fromAngle = Mathf.RoundToInt(transform.eulerAngles.z);
        int toAngle = Mathf.RoundToInt(cameraDefaultAnglePosition);

        if (fromAngle > 180)
        {
            fromAngle = fromAngle - 360;
        }

        while (fromAngle != toAngle)
        {
            if (fromAngle < toAngle)
            {
                fromAngle++;
            }
            else
            {
                fromAngle--;
            }


            transform.rotation = Quaternion.AngleAxis(fromAngle, Vector3.forward);

            yield return new WaitForSeconds(0.1f / cameraReturnSpeed);
        }

        cameraIsReturningToPosition = false;

        yield return false;
    }

    public float getCameraAnglePosition()
    {
        return verticeCameraAnglePosition;
    }

    public float getCameraAngleView()
    {
        return cameraAngleView;
    }
}