using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(RB_Controller2D))]
public class RB_Player : MonoBehaviour {

	RB_Controller2D controller;
	Rigidbody2D rb;
	//PolygonCollider2D spriteCollider;
	BoxCollider2D boxCollider;

	[Range(-100, -1)] public float gravity = -20;
	Vector3 velocity;
	[Range(1, 50)] public float moveSpeed = 10;
	[Range(1, 50)] public float jumpPower = 20;

	Collider2D collidedObject;

	bool clipping;
	float clipAmount;

	bool leftCollision, rightCollision, bottomCollision, topCollision, grounded;

	// Use this for initialization
	void Start () {
		controller = GetComponent<RB_Controller2D> ();
		rb = GetComponent<Rigidbody2D> ();
		//spriteCollider = GetComponentInChildren<PolygonCollider2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		leftCollision = rightCollision = bottomCollision = topCollision = grounded = false;
	}
	
	// Update is called once per frame
	void Update () {
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
		if (grounded) {
			velocity.y = 0;
			print ("grounded");
		} else {
			velocity.y += gravity * Time.deltaTime;
			print ("not grounded");
		}
		velocity.x = Input.GetAxisRaw ("Horizontal") * moveSpeed;
		if (Input.GetKeyDown (KeyCode.Z))
			velocity.y = jumpPower;
		transform.Translate (velocity * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D col) {
		collidedObject = col.collider;

		Vector3 contactPoint = col.contacts [0].point;
		Vector3 spriteCenter = boxCollider.bounds.center;
		Vector3 colCenter = collidedObject.bounds.center;

		rightCollision = contactPoint.x > spriteCenter.x;
		leftCollision = contactPoint.x < spriteCenter.x;
		topCollision = contactPoint.y > spriteCenter.y;
		bottomCollision = contactPoint.y < spriteCenter.y;

		if (col.gameObject.layer == 9 && velocity.y < 0)
			grounded = true;
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.layer == 9)
			grounded = false;
	}
}
