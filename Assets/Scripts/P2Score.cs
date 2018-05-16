using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2Score : MonoBehaviour {

	[HideInInspector] public int gameScore;

	public Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text> ();
		gameScore = 0;
	}

	// Update is called once per frame
	void Update () {
		scoreText.text = "P2 KILLS: " + gameScore.ToString ();
	}
}
