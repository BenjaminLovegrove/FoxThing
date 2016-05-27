using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

    private AudioSource BGM;
    private Image thisImg;
    private float loadTimer;
    private float loadDelay;
    private bool fading;
    private bool credits;
    private bool ended;

    //Main menu fade in
    public Image img1;
    public Image img2;
    public Image img3;
    public Text txt1;
    public Text txt2;
    public Image insImg;

    //Credits
    public Image title;
    public Text myName;

    void Update()
    {
        if (fading)
        {
            loadTimer += Time.deltaTime;
            if (loadTimer > loadDelay)
            {
                Application.LoadLevel(Application.loadedLevel);
            }

            if (loadTimer > loadDelay - 15 && !credits)
            {
                credits = true;
                title.CrossFadeAlpha(1, 2f, false);
                myName.CrossFadeAlpha(1, 2f, false);
            }
            if (loadTimer > loadDelay - 8 && !ended)
            {
                ended = true;
                title.CrossFadeAlpha(0, 4f, false);
                myName.CrossFadeAlpha(0, 4f, false);
            }

            if (loadTimer > loadDelay - 8)
            {
                BGM.volume = Mathf.Lerp(BGM.volume, 0, Time.deltaTime / 3);
            }

            if (thisImg.color.a == 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

	void Start () {
        BGM = Camera.main.GetComponent<AudioSource>();
        title.CrossFadeAlpha(0, 0f, false);
        myName.CrossFadeAlpha(0, 0f, false);
        img1.CrossFadeAlpha(0, 0f, false);
        img2.CrossFadeAlpha(0, 0f, false);
        img3.CrossFadeAlpha(0, 0f, false);
        txt1.CrossFadeAlpha(0, 0f, false);
        txt2.CrossFadeAlpha(0, 0f, false);
        insImg.CrossFadeAlpha(0, 0f, false);
        fading = false;
        thisImg = gameObject.GetComponent<Image>();
        thisImg.CrossFadeAlpha(0, 2f, false);
        img1.CrossFadeAlpha(1, 2.5f, false);
        img2.CrossFadeAlpha(1, 2.5f, false);
        img3.CrossFadeAlpha(1, 2.5f, false);
        txt1.CrossFadeAlpha(1, 2.5f, false);
        txt2.CrossFadeAlpha(1, 2.5f, false);
        insImg.CrossFadeAlpha(1, 2.5f, false);
    }

    public void FadeOut(float timer)
    {
        this.gameObject.SetActive(true);
        loadDelay = timer + 25f;
        fading = true;
        thisImg.CrossFadeAlpha(1, timer + 5f, false);
    }
	

}
