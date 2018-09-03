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

    Transform p1Trans, p1Respawn, p2Trans, p2Respawn;

    bool p1AlphaUp = false, p1AlphaDown = false, p1IncrementScore = true, p1Sounded = false;
    bool p2AlphaUp = false, p2AlphaDown = false, p2IncrementScore = true, p2Sounded = false;

    int p1RespawnLength = 60, p1RespawnCount = 0, p1DeathLength = 60, p1DeathCount = 0;
    int p2RespawnLength = 60, p2RespawnCount = 0, p2DeathLength = 60, p2DeathCount = 0;

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
        p2Respawn = GameObject.FindGameObjectWithTag("P2Respawn").
            GetComponent<Transform>();
        p1Sprite = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<SpriteRenderer>();
        p2Sprite = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<SpriteRenderer>();
        p1Score = GameObject.FindGameObjectWithTag("P1Score").
            GetComponent<P1Score>();
        p2Score = GameObject.FindGameObjectWithTag("P2Score").
            GetComponent<P2Score>();
        p1Color = p1Sprite.color;
        p2Color = p2Sprite.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (p1Script.dead)
        {
            print("p1 dead");
            p1Script.invincible = true;
            p1Sprite.enabled = false;
            if (!p1Sounded)
            {
                p2Script.killSound.Play();
                p1Sounded = true;
            }
            if (p1IncrementScore)
            {
                p2Score.gameScore++;
                p1IncrementScore = false;
            }
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
                print("p1 resetting respawn");
                p1Script.inRespawn = false;
                p1Script.invincible = false;
                p1Color.a = 1f;
                p1Sprite.color = p1Color;
                p1RespawnCount = 0;
                p1IncrementScore = true;
                p1Sounded = false;
            }
        }
        if (p1Script.invincible)
        {
            if (p1Color.a == 1)
            {
                p1Color.a = 0.8f;
                p1Sprite.color = p1Color;
            }
            if (p1Color.a > 0.7f)
            {
                p1AlphaUp = false;
                p1AlphaDown = true;
            }
            if (p1Color.a < 0.3f)
            {
                p1AlphaDown = false;
                p1AlphaUp = true;
            }
            if (p1AlphaUp)
            {
                p1Color.a = p1Color.a + 0.1f;
                p1Sprite.color = p1Color;
            }
            if (p1AlphaDown)
            {
                p1Color.a = p1Color.a - 0.1f;
                p1Sprite.color = p1Color;
            }
        }

        if (p2Script.dead)
        {
            print("p2 dead");
            p2Script.invincible = true;
            p2Sprite.enabled = false;
            if (!p2Sounded)
            {
                p2Script.killSound.Play();
                p2Sounded = true;
            }
            if (p2IncrementScore)
            {
                p1Score.gameScore++;
                p2IncrementScore = false;
            }
            p2DeathCount++;
            if (p2DeathCount > p2DeathLength)
            {
                p2Sprite.enabled = true;
                p2Script.inRespawn = true;
                p2DeathCount = 0;
            }
        }
        if (p2Script.inRespawn)
            print("p2 in respawn");
        if (!p2Script.inRespawn)
            print("p2 not in respawn");
        if (p2Script.inRespawn && p2Script.dead)
        {
            p2Trans.position = p2Respawn.position;
            p2Script.dead = false;
        }
        if (p2Script.inRespawn && !p2Script.dead)
        {
            p2RespawnCount++;
            print("p2 respawning");
            if (p2RespawnCount > p2RespawnLength)
            {
                print("p2 resetting respawn");
                p2Script.inRespawn = false;
                p2Script.invincible = false;
                p2Color.a = 1f;
                p2Sprite.color = p2Color;
                p2RespawnCount = 0;
                p2IncrementScore = true;
                p2Sounded = false;
            }
        }
        if (p2Script.invincible)
        {
            if (p2Color.a == 1)
            {
                p2Color.a = 0.8f;
                p2Sprite.color = p2Color;
            }
            if (p2Color.a > 0.7f)
            {
                p2AlphaUp = false;
                p2AlphaDown = true;
            }
            if (p2Color.a < 0.3f)
            {
                p2AlphaDown = false;
                p2AlphaUp = true;
            }
            if (p2AlphaUp)
            {
                p2Color.a = p2Color.a + 0.1f;
                p2Sprite.color = p2Color;
            }
            if (p2AlphaDown)
            {
                p2Color.a = p2Color.a - 0.1f;
                p2Sprite.color = p2Color;
            }
        }
    }
}
