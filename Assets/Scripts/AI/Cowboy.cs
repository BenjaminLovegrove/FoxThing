using UnityEngine;
using System.Collections;

public class Cowboy : MonoBehaviour {

    private bool triggered;
    private AudioSource cowboySFX;
    public AudioClip bellsRigning;
    public AudioClip alertedSFX;
    public AudioClip yellingCrowd;
    private float startVol;


    void Awake () {
        triggered = false;
        cowboySFX = gameObject.GetComponent<AudioSource>();
        startVol = cowboySFX.volume;
    }
	
    void Update()
    {
        if (triggered)
        {
            cowboySFX.volume = Mathf.Lerp(cowboySFX.volume, 0, Time.deltaTime * 2);
        } else
        {
            cowboySFX.volume = Mathf.Lerp(cowboySFX.volume, startVol, Time.deltaTime * 2);
        }
    }

	void OnTriggerEnter (Collider col) {
        Vector3 dir = (col.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        cowboySFX.volume = cowboySFX.volume * 1.5f;
        AudioSource.PlayClipAtPoint(alertedSFX, transform.position, 0.5f);
        AudioSource.PlayClipAtPoint(yellingCrowd, transform.position);
        col.SendMessage("Killed", transform.position);
        triggered = true;
        Invoke("BellToll", 1f);
        Invoke("Untrigger", 10f);
    }

    void BellToll()
    {
        AudioSource.PlayClipAtPoint(bellsRigning, transform.position);
    }

    void Untrigger()
    {
        triggered = false;
    }
}
