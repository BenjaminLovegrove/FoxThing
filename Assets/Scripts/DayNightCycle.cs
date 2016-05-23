using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    //Components
    private Renderer[] renderContainer;
    private Material[] skyboxMats;
    public Light sunLight;

    //Sun Rotation
    public float timeOfDay;
    public bool fastForward;
    public float nightDuration;
    private float nightTimer;

	void Start () {
        //Get the sun + rotations
        sunLight = GameObject.Find("Sun").GetComponent<Light>();

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
        if (!fastForward)
        {
            timeOfDay += Time.deltaTime / 24;
        } else
        {
            timeOfDay += Time.deltaTime;
        }

        //Move Sun and change brightness
        sunLight.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(-transform.right, transform.up), Quaternion.LookRotation(transform.right, transform.forward), timeOfDay);
        if (timeOfDay < 0.5f)
        {
            sunLight.intensity = Mathf.Lerp(0, 1, timeOfDay * 2);
        }
        else {
            sunLight.intensity = Mathf.Lerp(1, 0, (timeOfDay * 2) - 1);
        }

        //Fade night sky
        foreach (Material mat in skyboxMats)
        {
            if (timeOfDay < 0.5f)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(1, 0, timeOfDay * 3));
            } else
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(0, 1, (timeOfDay * 2) - 1));
            }
        }

        if (timeOfDay > 1)
        {
            nightTimer += Time.deltaTime;
            if (nightTimer > nightDuration)
            {
                timeOfDay = 0;
            }
        }
    }
}
