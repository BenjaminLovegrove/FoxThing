using UnityEngine;
using System.Collections;

public class RemoveFadeIn : MonoBehaviour {

    public GameObject fadeInObj;

	void Awake () {
        fadeInObj.SetActive(true);
	}

}
