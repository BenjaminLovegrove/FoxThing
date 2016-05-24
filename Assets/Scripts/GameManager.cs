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
    private Vector3 lerpToPos;

    public bool fastForwardTime;

    void Awake()
    {
        endCamPos = GameObject.Find("EndCamPos").transform;
        lerpStarted = false;
        foxes = GameObject.FindGameObjectsWithTag("FoxLookAt");
        camLookAt = GameObject.Find("LookAtObj").transform;
        foxCount = foxes.Length;
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
                foxes[foxCount - 1].transform.position = lerpToPos;
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
            camLookAt.transform.position = Vector3.Lerp(lerpStartPos, foxes[foxCount - 1].transform.position, camLerp);

            if (camLerp < 0.85f)
            {
                foxes[foxCount - 1].transform.Translate(transform.up * 2f * Time.deltaTime);
            } else
            {
                foxes[foxCount - 1].transform.Translate(-transform.up * 1f * Time.deltaTime);
            }

        } else
        {
            camLerp += Time.deltaTime / (camLerpTime * 1.5f);
            camLookAt.transform.position = Vector3.Lerp(lerpStartPos, endCamPos.transform.position, camLerp);
        }
    }

    public void StartLerp()
    {
        if (foxCount > 0)
        {
            camLookAt.parent = foxes[foxCount - 1].transform;
            lerpToPos = foxes[foxCount - 1].transform.position;
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
