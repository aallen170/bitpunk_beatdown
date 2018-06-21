using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]

public class Player1 : MonoBehaviour {

	[Range(1, 50)] public float moveSpeed = 15;
	[Range(1, 100)] public float jumpPower = 50;
	[Range(1, 300)] public float fallSpeed = 100;
	[Range(1, 100)] public float groundedTraction = 50;
	[Range(1, 100)] public float airialTraction = 20;
	[Range(1, 100)] public float divekickSpeed = 40;
	[Range(0, 100)] public float slideAttackSpeed = 20;
	[Range(1, 100)] public float dashSpeed = 40;
	[Range(0, 20)] public float crouchSpeed = 5;
	[Range(1, 30)] public float dashActiveFrames = 15;
	[Range(1, 120)] public float clingActiveFrames = 30;
	[Range(1, 30)] public float slideAttackActiveFrames = 6;
	[Range(1, 200)] public float slideAttackCooldownFrames = 5;
	[Range(1, 30)] public float guardActiveFrames = 15;

	float dashFrameCount = 0;
	float clingFrameCount = 0;
	float slideAttackFrameCount = 0;
	float guardFrameCount = 0;

	bool doubleJumped = false;
	[HideInInspector] public bool divekicked = false;
	[HideInInspector] public bool slideAttacked = false;
	[HideInInspector] public bool guarded = false;
	bool canSlideAttack = true;
	bool facingLeft = false;
	bool facingRight = true;
	bool dashed = false;
	bool dashedLeft = false;
	bool dashedRight = false;
	bool moving = false;
	public static bool crouching = false;
	bool movingLeft = false;
	bool movingRight = false;
	bool rotated = false;

	bool canMove = true;

	bool grounded;
	bool climbingSlope;
	bool descendingSlope;

	public KeyCode jumpKey = KeyCode.Period;
	public KeyCode actionKey = KeyCode.Slash;

	float accelerationTimeGrounded;
	float accelerationTimeAirborne;

	float timeToJumpApex;
	float gravity;

	float velocityXSmoothing;

	Vector3 velocity;

	Controller2D controller;

	SpriteRenderer playerSprite;
	SpriteRenderer playerIcon;
	SpriteRenderer opponentSprite;
	SpriteRenderer opponentIcon;

	Vector3 standingPos;

	Vector2 input;

	bool clinging = false;

	public Sprite runLeftSprite, runRightSprite, upClingLeftSprite, upClingRightSprite, upLeftClingSprite,
	upRightClingSprite, leftClingSprite, rightClingSprite, jumpFallLeft, jumpFallRight, crouchLeftSprite,
	crouchRightSprite, divekickRightSprite, divekickLeftSprite, slideAttackLeftSprite, slideAttackRightSprite,
	guardRightSprite, guardLeftSprite, hitbox;

	Collider2D collidedObject;

	BoxCollider2D boxCollider;

	bool clipping;
	float clipAmount;

	bool leftCollision, rightCollision, bottomCollision, topCollision;

	[HideInInspector] public bool dead = false;

	GameObject respawnUp, respawnDown, respawnRight, respawnLeft, opponent;

	BoxCollider2D activeAreaUp, activeAreaDown, activeAreaRight, activeAreaLeft;

	[HideInInspector] public bool inUpArea, inDownArea, inLeftArea, inRightArea;

	Player2 p2Script;

	P1Score p1Score;
	P2Score p2Score;

	AudioSource deathSound, clingSound;

	bool canPlayClingSound = true;

	public static bool p1Win = false;
	public static bool p2Win = false;

	Canvas p1WinCanvas, p2WinCanvas;

	bool lockFacing = false;

	void Start () {
		controller = GetComponent<Controller2D> ();
		playerSprite = GetComponent<SpriteRenderer> ();
		playerIcon = GetComponentInChildren<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		respawnUp = GameObject.FindGameObjectWithTag ("RespawnUp");
		respawnDown = GameObject.FindGameObjectWithTag ("RespawnDown");
		respawnLeft = GameObject.FindGameObjectWithTag ("RespawnLeft");
		respawnRight = GameObject.FindGameObjectWithTag ("RespawnRight");
		if (gameObject.tag == "Player1") {
			opponent = GameObject.FindGameObjectWithTag ("Player2");
		}
		if (gameObject.tag == "Player2") {
			opponent = GameObject.FindGameObjectWithTag ("Player1");
		}
		activeAreaUp = GameObject.FindGameObjectWithTag ("ActiveAreaUp").GetComponent<BoxCollider2D> ();
		activeAreaDown = GameObject.FindGameObjectWithTag ("ActiveAreaDown").GetComponent<BoxCollider2D> ();
		activeAreaLeft = GameObject.FindGameObjectWithTag ("ActiveAreaLeft").GetComponent<BoxCollider2D> ();
		activeAreaRight = GameObject.FindGameObjectWithTag ("ActiveAreaRight").GetComponent<BoxCollider2D> ();
		p2Script = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player2> ();
		opponentSprite = opponent.GetComponent<SpriteRenderer> ();
		opponentIcon = opponent.GetComponentInChildren<SpriteRenderer> ();
		p1Score = GameObject.FindGameObjectWithTag ("P1Score").GetComponent<P1Score> ();
		p2Score = GameObject.FindGameObjectWithTag ("P2Score").GetComponent<P2Score> ();
		p1Win = p2Win = false;
		opponentSprite.enabled = true;
		opponentIcon.enabled = true;
		p1WinCanvas = GameObject.FindGameObjectWithTag ("P1Win").GetComponent<Canvas> ();
		p2WinCanvas = GameObject.FindGameObjectWithTag ("P2Win").GetComponent<Canvas> ();
		p1WinCanvas.enabled = p2WinCanvas.enabled = false;
		deathSound = GetComponents<AudioSource> () [0];
		clingSound = GetComponents<AudioSource> () [1];
	}

	void Update () {

		UpdateScore ();

		CheckActiveArea ();

		DetectDeath ();

		DetectDirectionalInputs ();

		ColPhysChecks ();

		DetectMovement ();

		DetectDivekicking ();

		DetectCrouching ();

		DetectSlideAttack ();

		DetectDash ();

		DetectJumping ();

		DetectClinging ();

		DetectGuard ();

		MovePlayer ();
	}

	void UpdateScore() {
		if (p1Score.gameScore == 5) {
			p1Win = true;
			p1WinCanvas.enabled = true;
		}
		if (p2Score.gameScore == 5) {
			p2Win = true;
			p2WinCanvas.enabled = true;
		}

		if (p1Win && gameObject.tag == "Player1")
			Destroy (opponent);
		if (p2Win && gameObject.tag == "Player2")
			Destroy (opponent);
	}

	void CheckActiveArea() {
		if (boxCollider.bounds.center.x < activeAreaLeft.bounds.max.x && boxCollider.bounds.center.y < activeAreaLeft.bounds.max.y) {
			inLeftArea = true;
			inRightArea = inUpArea = inDownArea = false;
		}
		if (boxCollider.bounds.center.x > activeAreaRight.bounds.min.x && boxCollider.bounds.center.y < activeAreaRight.bounds.max.y) {
			inRightArea = true;
			inLeftArea = inUpArea = inDownArea = false;
		}
		if (boxCollider.bounds.center.x > activeAreaLeft.bounds.max.x && boxCollider.bounds.center.x < activeAreaRight.bounds.min.x && boxCollider.bounds.center.y < activeAreaUp.bounds.min.y) {
			inDownArea = true;
			inLeftArea = inUpArea = inRightArea = false;
		}
		if (boxCollider.bounds.center.y > activeAreaUp.bounds.min.y) {
			inUpArea = true;
			inLeftArea = inDownArea = inRightArea = false;
		}
	}

	void DetectDeath() {
		if (dead) {
			deathSound.Play ();
			p2Score.gameScore++;
			if (p2Script.inLeftArea) {
				transform.position = respawnRight.transform.position;
				dead = false;
			}
			if (p2Script.inRightArea) {
				transform.position = respawnLeft.transform.position;
				dead = false;
			}
			if (p2Script.inUpArea) {
				transform.position = respawnDown.transform.position;
				dead = false;
			}
			if (p2Script.inDownArea) {
				transform.position = respawnUp.transform.position;
				dead = false;
			}
		}
	}

	void DetectDirectionalInputs() {
		if (Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D))
			input.x = -1;
		else if (Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.A))
			input.x = 1;
		else
			input.x = 0;
		if (Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.W))
			input.y = -1;
		else if (Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S))
			input.y = 1;
		else
			input.y = 0;
	}

	void ColPhysChecks() {
		grounded = controller.collisions.below;
		climbingSlope = controller.collisions.climbingSlope;
		descendingSlope = controller.collisions.descendingSlope;

		gravity = -fallSpeed * 2f;
		accelerationTimeGrounded = 1f / groundedTraction;
		accelerationTimeAirborne = 1f / airialTraction;

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
	}

	void DetectMovement() {
		if (input.x == 0 && input.y == 0)
			moving = false;
		else
			moving = true;

		if (input.x > 0) {
			movingRight = true;
		}

		if (input.x < 0) {
			movingLeft = true;
		}
	}

	void DetectDivekicking() {
		if (!divekicked && !slideAttacked && input.x > 0) {
			facingLeft = false;
			facingRight = true;
		}

		if (!divekicked && !slideAttacked && input.x < 0) {
			facingLeft = true;
			facingRight = false;
		}

		if (Input.GetKeyDown (actionKey) && !controller.collisions.below && !divekicked)
			divekicked = true;

		if (controller.collisions.below) {
			divekicked = false;
			doubleJumped = false;
		}
	}

	void DetectCrouching() {
		if (!guarded) {
			if (input.y == -1 && !controller.collisions.isAirborne () && !clinging && !crouching)
				crouching = true;
			if (input.y > -1 && !controller.collisions.isAirborne () && !clinging && crouching)
				crouching = false;
		}

		if (crouching && facingLeft)
			playerSprite.sprite = crouchLeftSprite;
		if (crouching && facingRight)
			playerSprite.sprite = crouchRightSprite;
	}

	void DetectSlideAttack() {
		if (Input.GetKeyDown (actionKey) && !controller.collisions.isAirborne () && crouching && canSlideAttack && !guarded) {
			slideAttacked = true;
			canSlideAttack = false;
		}

		if (slideAttackFrameCount >= slideAttackActiveFrames) {
			slideAttacked = false;
		}

		if (slideAttackFrameCount >= (slideAttackActiveFrames + slideAttackCooldownFrames)) {
			canSlideAttack = true;
			slideAttackFrameCount = 0;
		}

		if (!canSlideAttack) {
			slideAttackFrameCount++;
		}
	}

	void DetectDash() {
		if (moving && Input.GetKeyDown (actionKey) && controller.collisions.below && !dashed) {
			dashed = true;
			if (facingLeft)
				dashedLeft = true;
			if (facingRight)
				dashedRight = true;
		}

		if (dashedLeft && facingRight && !controller.collisions.isAirborne()) {
			dashed = false;
			dashedLeft = false;
		}
		if (dashedRight && facingLeft && !controller.collisions.isAirborne()) {
			dashed = false;
			dashedRight = false;
		}

		if (dashed && clinging)
			dashed = false;

		if (dashed) {
			dashFrameCount++;
		}

		if (dashed && dashFrameCount >= dashActiveFrames && controller.collisions.below) {
			dashed = false;
			dashFrameCount = 0;
		}
	}

	void DetectJumping() {
		if (Input.GetKeyDown (jumpKey) && canMove) {
			if (controller.collisions.below) {
				velocity.y = jumpPower;
			}
		}

		if (facingLeft && !clinging && !crouching && !lockFacing)
			playerSprite.sprite = runLeftSprite;

		if (facingRight && !clinging && !crouching && !lockFacing)
			playerSprite.sprite = runRightSprite;

		if (facingLeft && controller.collisions.isAirborne () && !clinging)
			playerSprite.sprite = jumpFallLeft;

		if (facingRight && controller.collisions.isAirborne () && !clinging)
			playerSprite.sprite = jumpFallRight;

		if (Input.GetKeyDown (jumpKey) && !controller.collisions.below && !doubleJumped && !clinging) {
			velocity.y = jumpPower;
			doubleJumped = true;
		}
	}

	void DetectClinging() {
		Vector2 clingNormal = controller.collisions.normal;

		if (controller.collisions.isAirborne () && !clinging)
			clingNormal = Vector2.zero;

		float clingAngle = Vector2.Angle (clingNormal, Vector2.up);

		bool upCling, upLeftCling, upRightCling, leftCling, rightCling;
		upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

		if (clingAngle == 180f) {
			upCling = true;
			if(facingLeft)
				playerSprite.sprite = upClingLeftSprite;
			if (facingRight)
				playerSprite.sprite = upClingRightSprite;
		}

		if (clingAngle == 135f) {
			if (clingNormal.x < 0) {
				upRightCling = true;
				playerSprite.sprite = upRightClingSprite;
			}
			if (clingNormal.x > 0) {
				upLeftCling = true;
				playerSprite.sprite = upLeftClingSprite;
			}
		}

		if (clingAngle > 88f && clingAngle < 92f) {
			if (clingNormal.x < 0) {
				rightCling = true;
				playerSprite.sprite = rightClingSprite;
			}
			if (clingNormal.x > 0) {
				leftCling = true;
				playerSprite.sprite = leftClingSprite;
			}
		}
			
		if (clingNormal.x < 0)
			print ("clingNormal.x = " + clingNormal.x);

		if (upCling || upLeftCling || upRightCling || leftCling || rightCling)
			clinging = true;

		if (clinging)
			clingFrameCount++;

		print ("cling angle = " + clingAngle);

		if (clinging && Input.GetKeyDown (jumpKey)) {
			divekicked = false;
			canPlayClingSound = true;
			velocity = clingNormal;
			if (upCling) {
				velocity.y = jumpPower * clingNormal.y / 2;
			}
			if (upRightCling || upLeftCling) {
				velocity.x = jumpPower * clingNormal.x / 2;
				velocity.y = jumpPower * clingNormal.y / 2;
			}
			if (rightCling) {
				facingLeft = true;
				facingRight = false;
				velocity.x = jumpPower * clingNormal.x / 2;
				velocity.y = jumpPower;
			}
			if (leftCling) {
				facingLeft = false;
				facingRight = true;
				velocity.x = jumpPower * clingNormal.x / 2;
				velocity.y = jumpPower;
			}

			clinging = upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

			if (facingLeft)
				playerSprite.sprite = jumpFallLeft;
			if (facingRight)
				playerSprite.sprite = jumpFallRight;
		}

		if (clinging && Input.GetKeyDown (actionKey)) {
			canPlayClingSound = true;
			if (upRightCling || rightCling) {
				facingLeft = true;
				facingRight = false;
				divekicked = true;
			}
			if (upLeftCling || leftCling) {
				facingLeft = false;
				facingRight = true;
				divekicked = true;
			}
			if (upCling)
				divekicked = true;

			clinging = upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

			if (facingLeft)
				playerSprite.sprite = divekickLeftSprite;
			if (facingRight)
				playerSprite.sprite = divekickRightSprite;
		}

		if (clinging && clingFrameCount >= clingActiveFrames) {
			
		}

		if (clinging) {
			velocity.x = 0;
			velocity.y = 0;
			controller.Move (velocity * Time.deltaTime);
			if (canPlayClingSound)
				clingSound.Play ();
			canPlayClingSound = false;
		}

		if (controller.collisions.below && canPlayClingSound) {
			clingSound.Play ();
			canPlayClingSound = false;
		}

		if (controller.collisions.isAirborne () && !clinging)
			canPlayClingSound = true;
	}

	void DetectGuard() {
		if (!guarded) {
			canMove = true;
			lockFacing = false;
		}

		if (Input.GetKeyDown (actionKey) && !controller.collisions.isAirborne() && !moving && !clinging)
			guarded = true;

		if (guardFrameCount >= guardActiveFrames) {
			guarded = false;
			guardFrameCount = 0;
		}

		if (guarded) {
			canMove = false;
			guardFrameCount++;
			if (facingLeft && !lockFacing) {
				playerSprite.sprite = guardLeftSprite;
				lockFacing = true;
			}
			if (facingRight && !lockFacing) {
				playerSprite.sprite = guardRightSprite;
				lockFacing = true;
			}
		}
	}

	void MovePlayer() {
		if (facingRight)
			print ("facing right");
		if (facingLeft)
			print ("facing left");

		if (slideAttacked)
			print ("slide attacked");

		if (canMove)
			print ("can move");
		else if (!canMove)
			print ("cannot move");
		
		if (facingRight && divekicked && controller.collisions.isAirborne() && !clinging && !guarded) {
			velocity.y = -divekickSpeed;
			velocity.x = divekickSpeed;
			playerSprite.sprite = divekickRightSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (facingLeft && divekicked && controller.collisions.isAirborne() && !clinging && !guarded) {
			velocity.y = -divekickSpeed;
			velocity.x = -divekickSpeed;
			playerSprite.sprite = divekickLeftSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (facingRight && slideAttacked && !clinging) {
			velocity.y += gravity * Time.deltaTime;
			velocity.x = slideAttackSpeed;
			playerSprite.sprite = slideAttackRightSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (facingLeft && slideAttacked && !clinging) {
			velocity.y += gravity * Time.deltaTime;
			velocity.x = -slideAttackSpeed;
			playerSprite.sprite = slideAttackLeftSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && dashed && !clinging && !crouching && !guarded && !slideAttacked) {
			float targetVelocityX = input.x * dashSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && !dashed && !clinging && !crouching && !guarded && !slideAttacked) {
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && !dashed && !clinging && crouching && !guarded && !slideAttacked) {
			float targetVelocityX = input.x * crouchSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}
	}

	/*void DetectClipping() {
		if (collidedObject != null) {
			if (boxCollider.bounds.min.y < collidedObject.bounds.max.y)
				clipping = true;
			else
				clipping = false;
		}
		if (clipping && velocity.y < 0) {
			clipAmount = collidedObject.bounds.max.y - boxCollider.bounds.min.y;
			transform.position = new Vector3 (transform.position.x, transform.position.y + clipAmount, transform.position.z);
			print (clipAmount);
		}

		if (clipping)
			print ("clipping");
	}*/
}
