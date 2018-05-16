using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoundingBox : MonoBehaviour {

	BoxCollider2D boundingBox;
	PolygonCollider2D spriteCollider;

	Vector2 spriteBoundsSize;

	// Use this for initialization
	void Start () {
		boundingBox = GetComponent<BoxCollider2D> ();
		spriteCollider = GetComponentInChildren<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		spriteBoundsSize.x = spriteCollider.bounds.size.x;
		spriteBoundsSize.y = spriteCollider.bounds.size.y;
		boundingBox.size = spriteBoundsSize;
	}
}
