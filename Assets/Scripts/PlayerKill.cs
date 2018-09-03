using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour {

	/*PolygonCollider2D playerCollider;

	Collider2D opponentCollider;*/

	Player1 p1Script;
	Player2 p2Script;

	GameObject player;
	GameObject opponent;

	bool divekicking;
	bool slideAttacking;

	PolygonCollider2D playerCollider;

	PolygonCollider2D opponentCollider;

	Vector3 colliderRadius;

	// Use this for initialization
	void Start () {
		p1Script = GameObject.FindGameObjectWithTag ("Player1").GetComponent<Player1> ();
		playerCollider = GetComponent<PolygonCollider2D> ();
		if (gameObject.tag == "Player1")
			opponent = GameObject.FindGameObjectWithTag ("Player2");
		if (gameObject.tag == "Player2")
			opponent = GameObject.FindGameObjectWithTag ("Player1");
		opponentCollider = opponent.GetComponent<PolygonCollider2D> ();
		p2Script = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player2> ();
		colliderRadius = playerCollider.bounds.size / 2;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (opponentCollider != null)
			print ("collided with opponent");*/
		if (gameObject.tag == "Player1") {
			divekicking = p1Script.divekicked;
			slideAttacking = p1Script.slideAttacked;
		}
		if (gameObject.tag == "Player2") {
			divekicking = p2Script.divekicked;
			slideAttacking = p2Script.slideAttacked;
		}
		//print ("divekicking = " + divekicking);
		/*if (playerCollider.bounds.max.x + 0.1f > opponentCollider.bounds.min.x - 0.1f && playerCollider.bounds.max.x - 0.1f < opponentCollider.bounds.min.x + 0.1f && playerCollider.bounds.min.y - 0.1f > opponentCollider.bounds.max.y + 0.1f && playerCollider.bounds.min.y + 0.1f < opponentCollider.bounds.max.y - 0.1f) {
			print ("collided with opponent");
			if (divekicking)
				print ("divekicked opponent");
		}*/
		if (playerCollider.bounds.max.x > opponentCollider.bounds.min.x && playerCollider.bounds.min.x < opponentCollider.bounds.max.x && playerCollider.bounds.max.y > opponentCollider.bounds.min.y && playerCollider.bounds.min.y < opponentCollider.bounds.max.y) {
			print ("inside");
			if (gameObject.tag == "Player1") {
				if ((((divekicking || slideAttacking) && !p2Script.guarded) ||
                    slideAttacking) && !p2Script.invincible) {
					print ("p2 killed");
					p2Script.dead = true;
				} else if (divekicking && p2Script.guarded) {
					print ("p2 guarded");
					p1Script.dead = true;
				}
			}
			if (gameObject.tag == "Player2") {
				if ((((divekicking || slideAttacking) && !p1Script.guarded) ||
                    slideAttacking) && !p1Script.invincible)
                {
					print ("p1 killed");
					p1Script.dead = true;
				} else if (divekicking && p1Script.guarded) {
					print ("p1 guarded");
					p2Script.dead = true;
				}
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
