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

    public bool fastForwardTime;

    void Awake()
    {
        lerpStarted = false;
    }

	void Start () {
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
            foxes[foxCount - 1].transform.parent.gameObject.SendMessage("PupStart");
            foxCount--;
            lerpStarted = false;
        }
    }

    void LerpCamera()
    {
        camLerp += Time.deltaTime / camLerpTime;
        camLookAt.transform.position = Vector3.Lerp(lerpStartPos, foxes[foxCount - 1].transform.position, camLerp);
    }

    public void StartLerp()
    {
        camLookAt.parent = foxes[foxCount - 1].transform;
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
