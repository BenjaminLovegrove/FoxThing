using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    private GameManager GM;

    //Components
    private Renderer[] renderContainer;
    private Material[] skyboxMats;
    public Light sunLight;
    public Light moonLight;
    private float maxMoonlight;

    //Sun Rotation
    public float timeOfDay;
    public float nightDuration;
    private float nightTimer;

    //Fast forwarding
    public float timeMod;
    public float normalDay;
    public float FFDay;

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeMod = normalDay;
    }

    void Start () {
        //Get the sun + rotations
        sunLight = GameObject.Find("Sun").GetComponent<Light>();
        moonLight = GameObject.Find("Moon").GetComponent<Light>();
        maxMoonlight = moonLight.intensity;

        //Find all fading skybox materials
        renderContainer = gameObject.GetComponentsInChildren<Renderer>();
        int x = 0;
        skyboxMats = new Material[renderContainer.Length];
        foreach (Renderer rend in renderContainer)
        {
            skyboxMats[x] = rend.material;
            x++;
        }

    }

    void Update()
    {
        CycleLighting();
    }

    void CycleLighting () {
        if (GM.gameTimer > 0)
        {
            if (!GM.fastForwardTime)
            {
                timeMod = Mathf.Lerp(timeMod, normalDay, Time.deltaTime / 2);
            }
            else
            {
                timeMod = Mathf.Lerp(timeMod, FFDay, Time.deltaTime / 2);
            }

            timeOfDay += Time.deltaTime / timeMod;
        }



        //Move Sun and change brightness
        sunLight.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(-transform.right, transform.up), Quaternion.LookRotation(transform.right, transform.forward), timeOfDay);
        if (timeOfDay < 0.5f)
        {
            sunLight.intensity = Mathf.Lerp(0, 1, timeOfDay * 2);
            moonLight.intensity = Mathf.Lerp(maxMoonlight, 0, timeOfDay * 2);
        }
        else {
            sunLight.intensity = Mathf.Lerp(1, 0, (timeOfDay * 2) - 1);
            moonLight.intensity = Mathf.Lerp(0, maxMoonlight, (timeOfDay * 2) - 1);
        }

        //Fade night sky
        foreach (Material mat in skyboxMats)
        {
            if (timeOfDay < 0.5f)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(1, 0.05f, timeOfDay * 2.5f));
            } else
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(0.05f, 1, (timeOfDay * 2) - 1));
            }
        }

        if (timeOfDay > 1)
        {
            nightTimer += Time.deltaTime;
            if (nightTimer > nightDuration)
            {
                nightTimer = 0;
                timeOfDay = 0;
            }
        }
    }
}
