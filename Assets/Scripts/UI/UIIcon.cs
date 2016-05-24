using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIIcon : MonoBehaviour {

    private GameManager GM;

    private RectTransform bunnyIcon;
    private Image bunnyImg;
    private Transform posMarker;
    private Transform playerCam;

	void Awake () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        bunnyImg = GetComponentInChildren<Image>();
        bunnyIcon = gameObject.GetComponent<RectTransform>();
        posMarker = gameObject.transform.parent.transform;
        playerCam = Camera.main.transform;
	}
	
	void Update () {
        //Position icon
        bunnyIcon.position = playerCam.position + ((posMarker.position - playerCam.position).normalized * 6);

        //If too close, remove marker
        if (Vector3.Distance(playerCam.position, posMarker.position) < 20)
        {
            bunnyImg.color = new Color(bunnyImg.color.r, bunnyImg.color.g, bunnyImg.color.b, (Mathf.Lerp(0, 1, (Vector3.Distance(playerCam.position, posMarker.position) - 10) / 10)));
        }

        //No marker before game starts or when moving camera
        if (GM.gameTimer <= 0 || GM.camLerp < 1)
        {
            bunnyImg.enabled = false;
        } else
        {
            bunnyImg.enabled = true;
        }

        //Set rotation to look at camera
        bunnyIcon.rotation = playerCam.rotation;
	}
}
