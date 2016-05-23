using UnityEngine;
using System.Collections;

public class FoxController : MonoBehaviour {

    public float maxVelocity;
    public float acceleration;
    public float turnSpeed;
    public float jumpPower;
    private Rigidbody foxRB;
    private Animation foxAnim;
    private Vector3 forwardForce;
    private float currentMaxVel;
    private float currentTurnSpeed;
    private bool grounded;
    private float recentlyJumped = 0;

	void Start () {
        foxRB = gameObject.GetComponent<Rigidbody>();
        foxAnim = gameObject.GetComponent<Animation>();
	}
	
	void Update () {
        Animate();
        CheckGrounded();
        Jump();

        recentlyJumped -= Time.deltaTime;
	}

    void FixedUpdate()
    {
        Controls();
    }

    void Jump()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            foxAnim.CrossFade("jump", 0.2f);
            foxRB.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            recentlyJumped = foxAnim["jump"].length * 0.75f;
        }

        //Turn fox, reversed if going backwards - moved to here for non fixed update
        if (Input.GetAxis("Vertical") >= 0)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * currentTurnSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(-Vector3.up * Input.GetAxis("Horizontal") * currentTurnSpeed * Time.deltaTime);
        }
    }

    void Controls()
    {
        //Set max vol for reverse
        if (Input.GetAxis("Vertical") <= 0)
        {
            currentMaxVel = 2f;
            currentTurnSpeed = turnSpeed / 3;
        } else
        {
            currentMaxVel = maxVelocity;
            currentTurnSpeed = turnSpeed;
        }

        //Add forces
        if (foxRB.velocity.sqrMagnitude < currentMaxVel)
        {
            foxRB.AddForce(transform.forward * Input.GetAxis("Vertical") * acceleration * Time.deltaTime);
        }
    }

    void Animate()
    {
        if (recentlyJumped <= 0)
        {
            //Walking anims
            if (Input.GetAxis("Vertical") > 0 || foxRB.velocity.magnitude > 0)
            {
                if (foxRB.velocity.sqrMagnitude > 6f && !foxAnim.IsPlaying("run") && Input.GetAxis("Vertical") > 0)
                {
                    foxAnim.CrossFade("run");
                }
                else if (foxRB.velocity.sqrMagnitude < 6f && !foxAnim.IsPlaying("walk"))
                {
                    foxAnim.CrossFade("walk", 1f);
                }
            }
            else if (foxRB.velocity.sqrMagnitude == 0 && !foxAnim.IsPlaying("idle"))
            {
                foxAnim.CrossFade("idle", 0.1f);
            }

            //Reverse walking backwards anim, only if goin real slow
            if (foxRB.velocity.sqrMagnitude > -1 && foxRB.velocity.magnitude < 1)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (foxAnim["walk"].speed != -1)
                    {
                        foxAnim["walk"].time = 0;
                        foxAnim["walk"].speed = -1;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0 || foxRB.velocity.sqrMagnitude == 0)
                {
                    foxAnim["walk"].speed = 1;
                }
            }
        }
    }

    void CheckGrounded()
    {
        Ray groundRay = new Ray(transform.position, -transform.up);
        grounded = Physics.Raycast(groundRay, 0.25f);
    }
}
