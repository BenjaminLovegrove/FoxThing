using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

    private Image thisImg;
    private float loadTimer;
    private float loadDelay;
    private bool fading;

    void Update()
    {
        if (fading)
        {
            loadTimer += Time.deltaTime;
            if (loadDelay > loadTimer)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

	void Start () {
        fading = false;
        thisImg = gameObject.GetComponent<Image>();
        thisImg.CrossFadeAlpha(0, 2f, false);
	}

    public void FadeOut(float timer)
    {
        loadDelay = timer + 2f;
        fading = true;
        thisImg.CrossFadeAlpha(1, timer, false);
    }
	

}
