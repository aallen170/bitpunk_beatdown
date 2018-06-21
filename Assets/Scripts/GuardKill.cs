using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardKill : MonoBehaviour {

	Player playerScript;
	Player opponentScript;

	GameObject player;
	GameObject opponent;

	bool divekicking;
	bool slideAttacking;

	PolygonCollider2D playerCollider;

	PolygonCollider2D opponentCollider;

	Vector3 colliderRadius;

	void Start () {
		playerScript = GetComponent<Player> ();
		playerCollider = GetComponent<PolygonCollider2D> ();
		if (gameObject.tag == "Player1")
			opponent = GameObject.FindGameObjectWithTag ("Player2");
		if (gameObject.tag == "Player2")
			opponent = GameObject.FindGameObjectWithTag ("Player1");
		opponentCollider = opponent.GetComponent<PolygonCollider2D> ();
		opponentScript = opponent.GetComponent<Player> ();
		colliderRadius = playerCollider.bounds.size / 2;
	}

	void Update () {
		
	}
}
