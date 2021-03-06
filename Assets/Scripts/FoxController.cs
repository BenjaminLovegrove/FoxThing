﻿using UnityEngine;
using System.Collections;

public class FoxController : MonoBehaviour {

    private GameManager GM;

    //Components
    private Rigidbody foxRB;
    private Animation foxAnim;
    private Vector3 forwardForce;
    public AudioSource footstepsSFX;
    public AudioSource footstepsSlowSFX;

    //Designer Variables
    public float maxVelocity;
    public float acceleration;
    public float turnSpeed;
    public float jumpPower;

    private float currentMaxVel;
    private float currentTurnSpeed;
    private bool grounded;
    private bool waterCheck;
    private float recentlyJumped;
    private bool stealthing;

    public bool death;
    private bool successful;
    private bool removedFox;
    private float deathTimer;
    private bool playOnce;

    void Awake()
    {
        foxRB = gameObject.GetComponent<Rigidbody>();
        foxAnim = gameObject.GetComponent<Animation>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        death = false;
        removedFox = false;
        playOnce = false;
        foxAnim["fox_sneak"].speed = 2f;
    }

    void Start () {
        //Start first fox full size
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Update () {
        if (death)
        {
            TheBellsToll();
            return;
        }

        Animate();
        CheckGrounded();
        CheckWater();
        Controls();

        //Jump Timer (for sfx, anims, etc)
        recentlyJumped -= Time.deltaTime;
	}

    void FixedUpdate()
    {
        if (!death)
        {
            Move();
        }
    }

    void Controls()
    {
        //Stealth check
       if (Input.GetButton("Fire3"))
        {
            stealthing = true;
        } else
        {
            stealthing = false;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && grounded && recentlyJumped < 0)
        {
            foxAnim.CrossFade("fox_jump", 0.2f);
            foxRB.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            foxRB.AddForce(transform.forward * 10, ForceMode.Impulse);
            recentlyJumped = foxAnim["fox_jump"].length * 1.25f;
        }

        //Turn fox, reversed if going backwards - moved to here for non fixed update
        if (Input.GetAxis("Vertical") >= -10) //made this redundant (never gets to -10), because of controller vs keyboard differences I dont have time to account for
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * currentTurnSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(-Vector3.up * Input.GetAxis("Horizontal") * currentTurnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        //Set max vol for reverse
        if (stealthing)
        {
            currentMaxVel = 1.5f;
            currentTurnSpeed = turnSpeed / 3;
        } else if (Input.GetAxis("Vertical") <= 0)
        {
            currentMaxVel = 2f;
            currentTurnSpeed = turnSpeed / 3;
        } else
        {
            currentMaxVel = maxVelocity;
            currentTurnSpeed = turnSpeed;
        }

        //Add forces
        if (!waterCheck || (Input.GetAxis("Vertical") < 0))
        {
            if (foxRB.velocity.sqrMagnitude < currentMaxVel)
            {
                foxRB.AddForce(transform.forward * Input.GetAxis("Vertical") * acceleration * Time.deltaTime);
            }
        }

        //Play SFX
        if (Input.GetAxis("Vertical") != 0 && foxRB.velocity.sqrMagnitude > 10f && grounded)
        {
            if (!footstepsSFX.isPlaying)
            {
                footstepsSFX.Play();
            }
        } else
        {
            footstepsSFX.Stop();
        }

        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && foxRB.velocity.sqrMagnitude < 10f && grounded)
        {
            if (!footstepsSlowSFX.isPlaying)
            {
                footstepsSlowSFX.Play();
            }
        } else
        {
            footstepsSlowSFX.Stop();
        }
    }

    void Animate()
    {
        if (recentlyJumped <= 0)
        {
            //Walking anims
            if (Input.GetAxis("Vertical") > 0 || foxRB.velocity.magnitude > 0)
            {

                if (stealthing)
                {
                    if (grounded && !foxAnim.IsPlaying("fox_sneak"))
                    {
                        foxAnim.CrossFade("fox_sneak");
                    }
                }
                else if (foxRB.velocity.sqrMagnitude > 10f && !foxAnim.IsPlaying("fox_run") && Input.GetAxis("Vertical") > 0)
                {
                    foxAnim.CrossFade("fox_run");
                }
                else if (foxRB.velocity.sqrMagnitude < 10f && !foxAnim.IsPlaying("fox_walk"))
                {
                    foxAnim.CrossFade("fox_walk");
                }
            }
            else if (foxRB.velocity.sqrMagnitude == 0f && !foxAnim.IsPlaying("fox_idle"))
            {
                foxAnim.CrossFade("fox_idle");
            }


            //Removed due to new animation
            //Reverse walking backwards anim, only if goin real slow
            /*if (foxRB.velocity.sqrMagnitude > -2 && foxRB.velocity.magnitude < 2)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (foxAnim["fox_walk"].speed != -1)
                    {
                        foxAnim["fox_walk"].time = 0;
                        foxAnim["fox_walk"].speed = -1;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0 || foxRB.velocity.sqrMagnitude == 0)
                {
                    foxAnim["fox_walk"].speed = 1;
                }
            }*/
        }
    }

    void CheckGrounded()
    {
        Ray groundRay = new Ray((transform.position + transform.up * 0.2f), -transform.up);
        grounded = Physics.Raycast(groundRay, 0.4f);
        Debug.DrawRay(transform.position, -transform.up);
    }

    void CheckWater()
    {
        Ray waterRay = new Ray((transform.position + (transform.forward * 0.5f) + (transform.up * 0.2f)), -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(waterRay, out hit, 0.4f))
        {
            if (hit.collider.gameObject.tag == "Water")
            {
                waterCheck = true;
            }
            else
            {
                waterCheck = false;
            }
        }
        Debug.DrawRay((transform.position + (transform.forward * 0.5f) + (transform.up * 0.2f)), -transform.up);
    }

    void EatBunny(Vector3 loc)
    {
        Vector3 dir = (loc - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        successful = true;
        death = true;
        footstepsSFX.Stop();
    }

    void Killed(Vector3 loc)
    {
        Vector3 dir = (loc - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        death = true;
        footstepsSFX.Stop();
    }

    void TheBellsToll()
    {
        deathTimer += Time.deltaTime;
        if (!removedFox)
        {
            GM.FastForward(true);
        }

        if (!playOnce && !successful)
        {
            foxAnim["fox_die"].speed = 0.2f;
            foxAnim["fox_die"].wrapMode = WrapMode.Once;
            foxAnim.CrossFade("fox_die");
            playOnce = true;
        } else if (!playOnce)
        {
            foxAnim["fox_bite"].wrapMode = WrapMode.Once;
            foxAnim.CrossFade("fox_bite");
            foxAnim.PlayQueued("fox_eat");
            foxAnim.PlayQueued("fox_eat");
            foxAnim.PlayQueued("fox_eat");
            playOnce = true;
        }

        if (deathTimer > 4f && !removedFox)
        {
            removedFox = true;
            GM.StartLerp();
        }

        if (deathTimer > 25)
        {
            Destroy(this.gameObject);
        }
    }
}
