using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]

public class PlayerOLD2 : MonoBehaviour {
	/*public float jumpHeight = 5;
	public float jumpDuration = 0.4f;*/
	[Range(1, 50)] public float moveSpeed = 15;
	[Range(1, 100)] public float jumpPower = 50;
	[Range(1, 300)] public float fallSpeed = 100;
	[Range(1, 100)] public float groundedTraction = 50;
	[Range(1, 100)] public float airialTraction = 20;
	[Range(1, 100)] public float divekickSpeed = 40;
	[Range(1, 100)] public float dashSpeed = 40;
	[Range(1, 30)] public float dashActiveFrames = 15;
	[Range(1, 20)] public float crouchSpeed = 5;

	float dashFrameCount = 0;

	bool doubleJumped = false;
	[HideInInspector] public bool divekicked = false;
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
	crouchRightSprite, divekickRightSprite, divekickLeftSprite, hitbox;

	Collider2D collidedObject;

	BoxCollider2D boxCollider;

	bool clipping;
	float clipAmount;

	bool leftCollision, rightCollision, bottomCollision, topCollision;

	[HideInInspector] public bool dead = false;

	GameObject respawnUp, respawnDown, respawnRight, respawnLeft, opponent;

	BoxCollider2D activeAreaUp, activeAreaDown, activeAreaRight, activeAreaLeft;

	[HideInInspector] public bool inUpArea, inDownArea, inLeftArea, inRightArea;

	Player opponentScript;

	P1Score p1Score;
	P2Score p2Score;

	AudioSource deathSound, clingSound;

	bool canPlayClingSound = true;

	public static bool p1Win = false;
	public static bool p2Win = false;

	/*Image p1WinButton, p2WinButton;
	Text p1WinText, p2WinText;*/

	Canvas p1WinCanvas, p2WinCanvas;

	/*public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;*/

	/*void OnValidate() {
		jumpHeight = Mathf.Clamp (jumpHeight, 0.1f, 20f);
		jumpDuration = Mathf.Clamp (jumpDuration, 0.1f, 5f);
		moveSpeed = Mathf.Clamp (moveSpeed, 0.1f, 50f);
		fallSpeed = Mathf.Clamp (fallSpeed, 0f, 1000f);
		jumpPower = Mathf.Clamp (jumpPower, 1f, 200f);
		dashSpeed = Mathf.Clamp (dashSpeed, 1f, 50f);
	}*/

	// Use this for initialization
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
		opponentScript = opponent.GetComponent<Player> ();
		opponentSprite = opponent.GetComponent<SpriteRenderer> ();
		opponentIcon = opponent.GetComponentInChildren<SpriteRenderer> ();
		p1Score = GameObject.FindGameObjectWithTag ("P1Score").GetComponent<P1Score> ();
		p2Score = GameObject.FindGameObjectWithTag ("P2Score").GetComponent<P2Score> ();
		p1Win = p2Win = false;
		opponentSprite.enabled = true;
		opponentIcon.enabled = true;
		/*p1WinButton = GameObject.FindGameObjectWithTag ("P1WinButton").GetComponent<Image> ();
		p2WinButton = GameObject.FindGameObjectWithTag ("P2WinButton").GetComponent<Image> ();
		p1WinText = GameObject.FindGameObjectWithTag ("P1WinText").GetComponent<Text> ();
		p2WinText = GameObject.FindGameObjectWithTag ("P2WinText").GetComponent<Text> ();
		p1WinButton.enabled = p2WinButton.enabled = p1WinText.enabled = p2WinText.enabled = false;*/
		print ("opponent icon = " + opponentIcon);
		p1WinCanvas = GameObject.FindGameObjectWithTag ("P1Win").GetComponent<Canvas> ();
		p2WinCanvas = GameObject.FindGameObjectWithTag ("P2Win").GetComponent<Canvas> ();
		p1WinCanvas.enabled = p2WinCanvas.enabled = false;
		print ("p1 win canvas = " + p1WinCanvas);
		print ("p2 win canvas = " + p2WinCanvas);
		deathSound = GetComponents<AudioSource> () [0];
		clingSound = GetComponents<AudioSource> () [1];
	}
	
	// Update is called once per frame
	void Update () {

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

		CheckActiveArea ();

		if (inLeftArea)
			print ("in left area");
		if (inRightArea)
			print ("in right area");
		if (inUpArea)
			print ("in up area");
		if (inDownArea)
			print ("in down area");

		if (dead) {
			deathSound.Play ();
			if (gameObject.tag == "Player1" && !opponentScript.divekicked) {
				print ("P1 died");
				p2Score.gameScore++;
				if (opponentScript.inLeftArea) {
					transform.position = respawnRight.transform.position;
					dead = false;
				}
				if (opponentScript.inRightArea) {
					transform.position = respawnLeft.transform.position;
					dead = false;
				}
				if (opponentScript.inUpArea) {
					transform.position = respawnDown.transform.position;
					dead = false;
				}
				if (opponentScript.inDownArea) {
					transform.position = respawnUp.transform.position;
					dead = false;
				}
			}
			if (gameObject.tag == "Player2") {
				print ("P2 died");
				p1Score.gameScore++;
				if (opponentScript.inLeftArea) {
					transform.position = respawnRight.transform.position;
					dead = false;
				}
				if (opponentScript.inRightArea) {
					transform.position = respawnLeft.transform.position;
					dead = false;
				}
				if (opponentScript.inUpArea) {
					transform.position = respawnDown.transform.position;
					dead = false;
				}
				if (opponentScript.inDownArea) {
					transform.position = respawnUp.transform.position;
					dead = false;
				}
				//transform.position = respawn.transform.position;
				dead = false;
			}
		}

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

		if (gameObject.tag == "Player1") {
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

		if (gameObject.tag == "Player2") {
			if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow))
				input.x = -1;
			else if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow))
				input.x = 1;
			else
				input.x = 0;
			if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.UpArrow))
				input.y = -1;
			else if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.UpArrow))
				input.y = 1;
			else
				input.y = 0;
		}
		//int wallDirX = (controller.collisions.left) ? -1 : 1;

		/*bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			} else {
				timeToWallUnstick = wallStickTime;
			}
		}*/

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

		if (input.x == 0 && input.y == 0)
			moving = false;
		else
			moving = true;

		if (input.x > 0) {
			movingRight = true;
			print ("moving right");
		}

		if (input.x < 0) {
			movingLeft = true;
			print ("moving left");
		}

		if (!divekicked && input.x > 0) {
			facingLeft = false;
			facingRight = true;
		}

		if (!divekicked && input.x < 0) {
			facingLeft = true;
			facingRight = false;
		}

		if (gameObject.tag == "Player1") {
		if (input.y == -1 && !controller.collisions.isAirborne () && !clinging && !crouching) {
				standingPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
				transform.position = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
				crouching = true;
				print ("crouched");
			}
			standingPos.x = transform.position.x;
			//print ("standing position = " + standingPos);
		if (input.y > 1 && !controller.collisions.isAirborne () && !clinging && crouching) {
				transform.position = standingPos;
				crouching = false;
			}
		}

		if (gameObject.tag == "Player2") {
			if (Input.GetKeyDown (KeyCode.DownArrow) && !controller.collisions.isAirborne () && !clinging && !crouching) {
				standingPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
				transform.position = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
				crouching = true;
				print ("crouched");
			}
			standingPos.x = transform.position.x;
			//print ("standing position = " + standingPos);
			if (Input.GetKeyUp (KeyCode.DownArrow) && !controller.collisions.isAirborne () && !clinging && crouching) {
				transform.position = standingPos;
				crouching = false;
			}
		}

		if (crouching && facingLeft)
			playerSprite.sprite = crouchLeftSprite;
		if (crouching && facingRight)
			playerSprite.sprite = crouchRightSprite;

		//print ("crouching = " + crouching);
		//print ("player sprite = " + playerSprite.sprite);

		/*if (Input.GetKeyDown (KeyCode.DownArrow) && facingRight && !climbingSlope && !descendingSlope && !crouching) {
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
		}*/

		if (Input.GetKeyDown (actionKey) && !controller.collisions.below && !divekicked)
			divekicked = true;

		if (controller.collisions.below) {
			divekicked = false;
			doubleJumped = false;
		}

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

		if (dashed)
			print ("dashing");

		if (!dashed)
			print ("not dashing");

		if (facingLeft)
			print ("facing left");

		if (facingRight)
			print ("facing right");

		if (Input.GetKeyDown (jumpKey)) {
			/*if (wallSliding) {
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if (input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}*/
			if (controller.collisions.below) {
				velocity.y = jumpPower;
			}
		}

		if (facingLeft && !clinging && !crouching)
			playerSprite.sprite = runLeftSprite;

		if (facingRight && !clinging && !crouching)
			playerSprite.sprite = runRightSprite;

		if (facingLeft && controller.collisions.isAirborne () && !clinging)
			playerSprite.sprite = jumpFallLeft;

		if (facingRight && controller.collisions.isAirborne () && !clinging)
			playerSprite.sprite = jumpFallRight;

		if (Input.GetKeyDown (jumpKey) && !controller.collisions.below && !doubleJumped && !clinging) {
			velocity.y = jumpPower;
			doubleJumped = true;
		}

		DetectClinging ();

		print ("divekicked = " + divekicked);

		if (controller.collisions.below && canPlayClingSound) {
			clingSound.Play ();
			canPlayClingSound = false;
		}

		if (controller.collisions.isAirborne () && !clinging)
			canPlayClingSound = true;

		if (facingRight && divekicked && !controller.collisions.below && !clinging) {
			velocity.y = -divekickSpeed;
			velocity.x = divekickSpeed;
			playerSprite.sprite = divekickRightSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (facingLeft && divekicked && !controller.collisions.below && !clinging) {
			velocity.y = -divekickSpeed;
			velocity.x = -divekickSpeed;
			playerSprite.sprite = divekickLeftSprite;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && dashed && !clinging && !crouching) {
			float targetVelocityX = input.x * dashSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && !dashed && !clinging && !crouching) {
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
		}

		if (!divekicked && !dashed && !clinging && crouching) {
			float targetVelocityX = input.x * crouchSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move (velocity * Time.deltaTime);
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
				print ("clung");
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
		

		print ("up right cling = " + upRightCling);
		if (clingNormal.x < 0)
			print ("clingNormal.x = " + clingNormal.x);

		if (upCling || upLeftCling || upRightCling || leftCling || rightCling)
			clinging = true;

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

		if (clinging) {
			print ("clinging");
		} else
			print ("not clinging");

		print ("cling normal = " + clingNormal);

		if (clinging) {
			velocity.x = 0;
			velocity.y = 0;
			controller.Move (velocity * Time.deltaTime);
			if (canPlayClingSound)
				clingSound.Play ();
			canPlayClingSound = false;
		}
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

	/*void OnCollisionEnter2D(Collision2D col) {
		collidedObject = col.collider;

		Vector3 contactPoint = col.contacts [0].point;
		Vector3 spriteCenter = boxCollider.bounds.center;
		Vector3 colCenter = collidedObject.bounds.center;

		rightCollision = contactPoint.x > spriteCenter.x;
		leftCollision = contactPoint.x < spriteCenter.x;
		topCollision = contactPoint.y > spriteCenter.y;
		bottomCollision = contactPoint.y < spriteCenter.y;
	}*/
}
