using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class CharacterControllerScript : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;
	public GameObject trail;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;
	public float maxJumps = 5f;
	int doubleJump = 0;

	bool collided = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update()
	{

		float move = Input.GetAxis("Horizontal");
		if (grounded && Input.GetKey (KeyCode.DownArrow) && move == 0) {
			anim.SetBool ("Crouch", true);
		} else {
			anim.SetBool("Crouch", false);
		}

		if (grounded && Input.GetKeyDown (KeyCode.Space) && Input.GetKey (KeyCode.DownArrow) && collided == true) {
			collided = false;
			StartCoroutine(jumpCoro());
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce - 400));
		} 
		else if (grounded && Input.GetKeyDown (KeyCode.Space) && doubleJump < maxJumps){
			doubleJump++;
			anim.SetBool ("Ground", false);
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
		}
		else {
			grounded = true;
		}
	}

	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);	
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

		float move = Input.GetAxis("Horizontal");

		anim.SetFloat ("Speed", Mathf.Abs (move));

		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		if (grounded) {
			doubleJump = 0;
		}

		if (move > 0 || move < 0 || !grounded) {
			trail.gameObject.GetComponent<ParticleSystem> ().enableEmission = true;
		} else {
			trail.gameObject.GetComponent<ParticleSystem> ().enableEmission = false;
		}

		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}
	}

	IEnumerator jumpCoro(){
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		yield return  StartCoroutine(Wait(.3f));
		GetComponent<BoxCollider2D>().enabled = true;
		GetComponent<CircleCollider2D>().enabled = true;
	}

	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Platform") {
			collided = true;
		} else {
			collided = false;
			GetComponent<BoxCollider2D> ().enabled = true;
			GetComponent<CircleCollider2D> ().enabled = true;
		}

		if (col.gameObject.tag == "Ground") {
			collided = false;
			GetComponent<BoxCollider2D> ().enabled = true;
			GetComponent<CircleCollider2D> ().enabled = true;
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		trail.GetComponent<Transform> ().transform.rotation = trail.GetComponent<Transform> ().transform.rotation * Quaternion.Euler(0, 180, 0);
	}
}