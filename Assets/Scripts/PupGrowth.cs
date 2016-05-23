using UnityEngine;
using System.Collections;

public class PupGrowth : MonoBehaviour {

    private GameManager GM;

    public bool grow;
    public float timeToGrow;

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update () {
        if (grow)
        {
            if (transform.localScale.x < 1)
            {
                transform.localScale = new Vector3(transform.localScale.x + (Time.deltaTime / timeToGrow), transform.localScale.y + (Time.deltaTime / timeToGrow), transform.localScale.z + (Time.deltaTime / timeToGrow));
            }
            else
            {
                GM.FastForward(false);
                gameObject.GetComponent<FoxController>().enabled = true;
            }
        }
	}

    void PupStart()
    {
        grow = true;
    }
}
