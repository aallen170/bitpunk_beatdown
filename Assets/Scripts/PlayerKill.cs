using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour {

	/*PolygonCollider2D playerCollider;

	Collider2D opponentCollider;*/

	Player playerScript;
	Player opponentScript;

	GameObject player;
	GameObject opponent;

	bool divekicking;

	BoxCollider2D playerCollider;

	BoxCollider2D opponentCollider;

	Vector3 colliderRadius;

	// Use this for initialization
	void Start () {
		playerScript = GetComponent<Player> ();
		playerCollider = GetComponent<BoxCollider2D> ();
		if (gameObject.tag == "Player1")
			opponent = GameObject.FindGameObjectWithTag ("Player2");
		if (gameObject.tag == "Player2")
			opponent = GameObject.FindGameObjectWithTag ("Player1");
		opponentCollider = opponent.GetComponent<BoxCollider2D> ();
		opponentScript = opponent.GetComponent<Player> ();
		colliderRadius = playerCollider.bounds.size / 2;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (opponentCollider != null)
			print ("collided with opponent");*/
		divekicking = playerScript.divekicked;
		print ("divekicking = " + divekicking);
		/*if (playerCollider.bounds.max.x + 0.1f > opponentCollider.bounds.min.x - 0.1f && playerCollider.bounds.max.x - 0.1f < opponentCollider.bounds.min.x + 0.1f && playerCollider.bounds.min.y - 0.1f > opponentCollider.bounds.max.y + 0.1f && playerCollider.bounds.min.y + 0.1f < opponentCollider.bounds.max.y - 0.1f) {
			print ("collided with opponent");
			if (divekicking)
				print ("divekicked opponent");
		}*/
		if (playerCollider.bounds.max.x > opponentCollider.bounds.min.x && playerCollider.bounds.min.x < opponentCollider.bounds.max.x && playerCollider.bounds.max.y > opponentCollider.bounds.min.y && playerCollider.bounds.min.y < opponentCollider.bounds.max.y) {
			print ("inside");
			if (divekicking) {
				print ("divekicked opponent");
				opponentScript.dead = true;
			}
		}
		/*print ("player bounds max = " + playerCollider.bounds.max);
		print ("player bounds min = " + playerCollider.bounds.min);
		print ("opponent bounds max = " + opponentCollider.bounds.max);
		print ("opponent bounds min = " + opponentCollider.bounds.min);*/
		/*if (opponentCollider != null && divekicking)
			opponentScript.dead = true;
		else
			opponentScript.dead = false;*/
	}

	void OnTriggerEnter2D(Collider2D col) {
		//if (col.collider != null) {
			//opponentCollider = col.collider;
		//opponentScript.dead = true;
		//}
	}

	void OnTriggerExit2D(Collider2D col) {
		print ("outside opponent");
			//opponentCollider = null;
	}
}
