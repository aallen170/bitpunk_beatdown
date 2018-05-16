using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RB_Controller2D : MonoBehaviour {

	/*PolygonCollider2D collider;

	// Use this for initialization
	void Start () {
		collider = GetComponentInChildren<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move(Vector3 velocity) {
		VerticalCollisions (ref velocity);

		transform.Translate (velocity);
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);

	}

	void OnCollisionEnter2D(Collision2D col) {
		Collider2D col2d = col.collider;

		Vector3 contactPoint = col.contacts [0].point;
		Vector3 center = col2d.bounds.center;

		bool right = contactPoint.x > center.x;
		bool left = contactPoint.x < center.x;
		bool top = contactPoint.y > center.y;
		bool bottom = contactPoint.y < center.y;

		if (bottom)
			print ("bottom collision");
	}*/
}
