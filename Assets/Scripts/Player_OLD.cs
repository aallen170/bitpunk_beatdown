using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]

public class Player_OLD : MonoBehaviour {
	/*public float jumpHeight = 5;
	public float jumpDuration = 0.4f;*/
	public float moveSpeed = 15;
	public float jumpPower = 50;
	public float fallSpeed = 100;
	public float groundedTraction = 50;
	public float airialTraction = 20;
	public float divekickSpeed = 40;
	public float dashSpeed = 40;
	public float dashActiveFrames = 15;

	float dashFrameCount = 0;

	bool doubleJumped = false;
	bool divekicked = false;
	bool facingLeft = false;
	bool facingRight = true;
	bool dashed = false;
	bool moving = false;
	bool crouching = false;

	bool grounded;
	bool climbingSlope;
	bool descendingSlope;

	public KeyCode jumpKey = KeyCode.Z;
	public KeyCode actionKey = KeyCode.X;

	float accelerationTimeGrounded;
	float accelerationTimeAirborne;

	float timeToJumpApex;
	float gravity;

	float velocityXSmoothing;

	Vector3 velocity;

	Controller2D controller;

	SpriteRenderer marker;

	void OnValidate() {
		/*jumpHeight = Mathf.Clamp (jumpHeight, 0.1f, 20f);
		jumpDuration = Mathf.Clamp (jumpDuration, 0.1f, 5f);*/
		moveSpeed = Mathf.Clamp (moveSpeed, 0.1f, 50f);
		fallSpeed = Mathf.Clamp (fallSpeed, 1f, 1000f);
		jumpPower = Mathf.Clamp (jumpPower, 1f, 200f);
		dashSpeed = Mathf.Clamp (dashSpeed, 1f, 50f);
	}

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();
		marker = GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		grounded = controller.collisions.below;
		climbingSlope = controller.collisions.climbingSlope;
		descendingSlope = controller.collisions.descendingSlope;

		gravity = -fallSpeed * 2f;
		accelerationTimeGrounded = 1f / groundedTraction;
		accelerationTimeAirborne = 1f / airialTraction;

		//timeToJumpApex = jumpDuration / 2f;

		/*gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);*/

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (input.x == 0 && input.y == 0)
			moving = false;
		else
			moving = true;

		if (!divekicked && input.x > 0) {
			facingLeft = false;
			facingRight = true;
			marker.flipX = false;
		}

		if (!divekicked && input.x < 0) {
			facingLeft = true;
			facingRight = false;
			marker.flipX = true;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow) && facingRight && !climbingSlope && !descendingSlope && !crouching) {
			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y - 0.25f, transform.position.z);
			transform.localScale = new Vector3 (1f, 0.5f, 1f);
			crouching = true;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow) && facingLeft && !climbingSlope && !descendingSlope && !crouching) {
			transform.position = new Vector3 (transform.position.x + 0.25f, transform.position.y - 0.25f, transform.position.z);
			transform.localScale = new Vector3 (1f, 0.5f, 1f);
			crouching = true;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow) && facingRight && !climbingSlope && !descendingSlope && crouching) {
			transform.position = new Vector3 (transform.position.x + 0.25f, transform.position.y + 0.25f, transform.position.z);
			transform.localScale = new Vector3 (0.5f, 1f, 1f);
			crouching = false;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow) && facingLeft && !climbingSlope && !descendingSlope && crouching) {
			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y + 0.25f, transform.position.z);
			transform.localScale = new Vector3 (0.5f, 1f, 1f);
			crouching = false;
		}

		if (crouching && !grounded && facingRight) {
			transform.position = new Vector3 (transform.position.x + 0.25f, transform.position.y + 0.25f, transform.position.z);
			transform.localScale = new Vector3 (0.5f, 1f, 1f);
			crouching = false;
		}

		if (crouching && !grounded && facingLeft) {
			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y + 0.25f, transform.position.z);
			transform.localScale = new Vector3 (0.5f, 1f, 1f);
			crouching = false;
		}

		if (Input.GetKeyDown (actionKey) && !controller.collisions.below && !divekicked)
			divekicked = true;

		if (controller.collisions.below) {
			divekicked = false;
			doubleJumped = false;
		}

		if (moving && Input.GetKeyDown (actionKey) && controller.collisions.below && !dashed) {
			dashed = true;
		}

		if (dashed) {
			dashFrameCount++;
		}

		if (dashed && dashFrameCount >= dashActiveFrames && controller.collisions.below) {
			dashed = false;
			dashFrameCount = 0;
		}

		if (dashed)
			print ("dashing");

		if (!dashed)
			print ("not dashing");

		if (facingLeft)
			print ("facing left");

		if (facingRight)
			print ("facing right");

		if (Input.GetKeyDown (jumpKey) && controller.collisions.below) {
			velocity.y = jumpPower;
		}

		if (Input.GetKeyDown (jumpKey) && !controller.collisions.below && !doubleJumped) {
			velocity.y = jumpPower;
			doubleJumped = true;
		}

		if (facingRight && divekicked && !controller.collisions.below) {
			velocity.y = -divekickSpeed;
			velocity.x = divekickSpeed;
			controller.Move (velocity * Time.deltaTime);
		}

		if (facingLeft && divekicked && !controller.collisions.below) {
			velocity.y = -divekickSpeed;
			velocity.x = -divekickSpeed;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && dashed) {
			float targetVelocityX = input.x * dashSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && !dashed) {
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}
	}
}
