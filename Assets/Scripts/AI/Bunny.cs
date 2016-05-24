using UnityEngine;
using System.Collections;

public class Bunny : MonoBehaviour {

    public AudioClip bunnyDeath;

    private Animation bunnyAnim;
    private bool triggered;
    private float deadTime;
    private AudioSource bunnySFX;

    void Awake()
    {
        triggered = false;
        bunnySFX = gameObject.GetComponent<AudioSource>();
        bunnyAnim = gameObject.GetComponent<Animation>();
    }

    void Update()
    {


        if (triggered)
        {
            deadTime += Time.deltaTime;
            if (deadTime > 25)
            {
                Destroy(this.gameObject);
            }
        } else
        {
            if (!bunnyAnim.isPlaying)
            {
                bunnyAnim.Play("Idle");
            }
        }
    }
	
	void OnTriggerEnter (Collider col) {
        if (!triggered)
        {
            bunnySFX.volume = bunnySFX.volume * 1.75f;
            bunnySFX.clip = bunnyDeath;
            bunnySFX.loop = false;
            bunnySFX.Play();
            triggered = true;
            bunnyAnim.CrossFade("Run With Death");
            col.SendMessage("EatBunny", transform.position);
        }
	}
}
