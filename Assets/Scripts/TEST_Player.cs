using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (TEST_Controller2D))]
public class TEST_Player : MonoBehaviour {



	TEST_Controller2D controller;

	void Start() {
		controller = GetComponent<TEST_Controller2D> ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Z)) {
			
		}
	}
}
