using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour {

    Player1 p1Script;
    Player2 p2Script;

    P1Score p1Score;
    P2Score p2Score;

    SpriteRenderer p1Sprite, p2Sprite;
    Color p1Color, p2Color;

    Transform p1Trans, p2Trans, p1Respawn;

    int p1RespawnLength = 60;
    int p1RespawnCount = 0;
    int p1DeathLength = 60;
    int p1DeathCount = 0;

    int p2RespawnLength = 60;
    int p2RespawnCount = 0;
    int p2DeathLength = 60;
    int p2DeathCount = 0;

    // Use this for initialization
    void Start () {
        p1Script = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Player1>();
        p2Script = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Player2>();
        p1Trans = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Transform>();
        p2Trans = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Transform>();
        p1Respawn = GameObject.FindGameObjectWithTag("P1Respawn").
            GetComponent<Transform>();
        p1Sprite = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<SpriteRenderer>();
        p2Sprite = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (p1Script.dead)
        {
            p1Script.invincible = true;
            p1Sprite.enabled = false;
            p2Script.killSound.Play();
            p2Score.gameScore++;
            p1DeathCount++;
            if (p1DeathCount > p1DeathLength)
            {
                p1Sprite.enabled = true;
                p1Script.inRespawn = true;
                p1DeathCount = 0;
            }
        }
        if(p1Script.inRespawn && p1Script.dead)
        {
            p1Trans.position = p1Respawn.position;
            p1Script.dead = false;
        }
        if(p1Script.inRespawn && !p1Script.dead)
        {
            p1RespawnCount++;
            if(p1RespawnCount > p1RespawnLength)
            {
                p1Script.inRespawn = false;
                p1Script.invincible = false;
                p1Color.a = 1;
                p1RespawnCount = 0;
            }
        }
        if (p1Script.invincible)
        {
            if (p1Color.a == 1)
                p1Color.a = 0.8f;
            if (p1Color.a > 0.7f)
                p1Color.a = p1Color.a - 0.1f;
            else if (p1Color.a < .3f)
                p1Color.a = p1Color.a + 0.1f;
        }
    }
}
