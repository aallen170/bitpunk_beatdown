using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewController : MonoBehaviour {

	RB_Controller2D controller;
	Rigidbody2D rb;
	PolygonCollider2D collider;
	Vector3 velocity;

	float skinWidth = -0.03f;

	private float verticalVelocity;
	private float gravity = 14.0f;
	private float jumpForce = 10.0f;

	/*public float jumpHeight = 4;
	public float timeToJumpApex = 0.4f;
	public float moveSpeed = 6;

	public float jumpVelocity = 5;*/

	// Use this for initialization
	void Start () {
		controller = GetComponent<RB_Controller2D> ();
		rb = GetComponent<Rigidbody2D> ();
		collider = GetComponent<PolygonCollider2D> ();

		/*rb.gravityScale = (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		rb.gravityScale = rb.gravityScale / 10;
		//jumpVelocity = Mathf.Abs (rb.gravityScale) * timeToJumpApex;
		print (jumpVelocity);*/
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Z))
			velocity.y = jumpVelocity;
		else
			velocity.y = 0;

		velocity.x = input.x * 6;
		velocity.y = input.y * 6;
		transform.Translate (velocity * Time.deltaTime);*/
	}

	void isGrounded() {
		
	}
}
