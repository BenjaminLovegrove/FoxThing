using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Components
    private GameObject[] foxes;
    private Transform camLookAt;

    //Variables
    private bool gameStarted;
    public float gameTimer;
    public float camLerp = 1;
    public float camLerpTime;
    private Vector3 lerpStartPos;
    private int foxCount;
    private bool lerpStarted;
    private Transform endCamPos;
    private GameObject bunnyProp;

    //Hacks for camera "cinemaic" smoothing
    private Vector3 lerpToPos;
    private Vector3 tempPos;

    public bool fastForwardTime;

    void Awake()
    {
        endCamPos = GameObject.Find("EndCamPos").transform;
        lerpStarted = false;
        foxes = GameObject.FindGameObjectsWithTag("FoxLookAt");
        camLookAt = GameObject.Find("LookAtObj").transform;
        foxCount = foxes.Length;
        bunnyProp = GameObject.Find("Rabbit-Dead");
    }

    void Update() {
        if (gameStarted)
        {
            gameTimer += Time.deltaTime;
        }

        if (camLerp < 1)
        {
            LerpCamera();
        } else if (lerpStarted)
        {
            if (foxCount > 0)
            {
                foxes[foxCount - 1].transform.parent.gameObject.SendMessage("PupStart");
            }
            foxCount--;
            lerpStarted = false;
        }
    }

    void LerpCamera()
    {
        if (foxCount > 0)
        {
            camLerp += Time.deltaTime / camLerpTime;

            if (camLerp < 0.85f)
            {
                foxes[foxCount - 1].transform.Translate(transform.up * 2f * Time.deltaTime);
                tempPos = foxes[foxCount - 1].transform.position;
            } else
            {
                foxes[foxCount - 1].transform.position = Vector3.Lerp(tempPos, lerpToPos, (camLerp - 0.85f) / 0.15f);
            }

        } else
        {
            camLerp += Time.deltaTime / (camLerpTime * 1.5f);
            camLookAt.transform.position = Vector3.Lerp(lerpStartPos, endCamPos.transform.position, camLerp);
        }

        camLookAt.transform.position = Vector3.Lerp(lerpStartPos, foxes[foxCount - 1].transform.position, camLerp);
    }

    public void StartLerp()
    {
        if (foxCount > 0)
        {
            camLookAt.parent = foxes[foxCount - 1].transform;
            lerpToPos = foxes[foxCount - 1].transform.position;
            if (foxCount == 1)
            {
                Destroy(bunnyProp);
            }
        } else
        {
            camLookAt.parent = endCamPos;
        }
        lerpStartPos = camLookAt.transform.position;
        camLerp = 0;
        lerpStarted = true;
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void FastForward(bool check)
    {
        if (check)
        {
            fastForwardTime = true;
        } else
        {
            fastForwardTime = false;
        }
    }


}
