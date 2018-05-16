using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5;

	Vector2 input;
	Vector3 move;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		input.x = Input.GetAxisRaw ("Horizontal");
		if (Input.GetKeyDown (KeyCode.Z)) {
			move.y = moveSpeed * 5;
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			move.y = 0;
		}

		move.x = Input.GetAxis ("Horizontal") * moveSpeed;
		transform.position += move * Time.deltaTime;
	}
}
