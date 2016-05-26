using UnityEngine;
using System.Collections;

public class Cowboy : MonoBehaviour {

    private bool triggered;
    private AudioSource cowboySFX;
    public AudioClip bellsRigning;
    public AudioClip alertedSFX;
    public AudioClip yellingCrowd;
    private float startVol;
    private SphereCollider myCollider;
    private float startRadius;
    public AudioSource bellToll;


    void Awake () {
        triggered = false;
        cowboySFX = gameObject.GetComponent<AudioSource>();
        startVol = cowboySFX.volume;
        myCollider = gameObject.GetComponent<SphereCollider>();
        startRadius = myCollider.radius;
    }
	
    void Update()
    {
        /*
        if (triggered)
        {
            cowboySFX.volume = Mathf.Lerp(cowboySFX.volume, 0, Time.deltaTime * 2);
        } else
        {
            cowboySFX.volume = Mathf.Lerp(cowboySFX.volume, startVol, Time.deltaTime * 2);
        }*/

        if (Input.GetButton("Fire3"))
        {
            myCollider.radius = startRadius * 0.7f;
        }
    }

    void OnTriggerStay (Collider col)
    {
        if (!triggered)
        {
            Vector3 offsetPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 dir = (col.transform.position - offsetPos).normalized;
            Ray FoxCheck = new Ray(offsetPos, dir);
            RaycastHit hit;
            Debug.DrawRay(offsetPos, (col.transform.position - transform.position), Color.white);
            if (Physics.Raycast(FoxCheck, out hit, 50f))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    AlertEnemy(hit.collider.transform.position, hit.collider.gameObject);
                    triggered = true;
                }
            }
        }
    }

	void AlertEnemy (Vector3 pos, GameObject fox) {
        triggered = true;
        Vector3 dir = (pos - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        cowboySFX.volume = cowboySFX.volume * 1.5f;
        AudioSource.PlayClipAtPoint(alertedSFX, transform.position, 0.7f);
        if (fox.gameObject.name == "Collider")
        {
            fox.SendMessageUpwards("Killed", transform.position);
        } else
        {
            fox.SendMessage("Killed", transform.position);
        }
        Invoke("BellToll", 1f);
        //Invoke("Untrigger", 10f);
    }

    void BellToll()
    {
        bellToll.Play();
    }

    void Untrigger()
    {
        triggered = false;
    }
}
