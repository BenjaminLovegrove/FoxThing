using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

    private Image thisImg;


	void Start () {
        thisImg = gameObject.GetComponent<Image>();
        thisImg.CrossFadeAlpha(0, 2f, false);
	}
	

}
