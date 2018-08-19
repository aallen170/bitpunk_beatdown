using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour {

    Player1 p1Script;
    Player2 p2Script;

    P1Score p1Score;
    P2Score p2Score;

    SpriteRenderer sprite;
    Color color;

    Transform trans, respawn;

    bool alphaUp = false;
    bool alphaDown = false;
    bool incrementScore = true;

    int respawnLength = 60;
    int respawnCount = 0;
    int deathLength = 60;
    int deathCount = 0;

    // Use this for initialization
    void Start () {
        p1Script = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Player1>();
        p2Script = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Player2>();
        trans = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Transform>();
        respawn = GameObject.FindGameObjectWithTag("P1Respawn").
            GetComponent<Transform>();
        sprite = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<SpriteRenderer>();
        p1Score = GameObject.FindGameObjectWithTag("P1Score").
            GetComponent<P1Score>();
        p2Score = GameObject.FindGameObjectWithTag("P2Score").
            GetComponent<P2Score>();
        color = sprite.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (p1Script.dead)
        {
            print("p1 dead");
            p1Script.invincible = true;
            sprite.enabled = false;
            p2Script.killSound.Play();
            if (incrementScore)
            {
                p2Score.gameScore++;
                incrementScore = false;
            }
            deathCount++;
            if (deathCount > deathLength)
            {
                sprite.enabled = true;
                p1Script.inRespawn = true;
                deathCount = 0;
            }
        }
        if(p1Script.inRespawn && p1Script.dead)
        {
            trans.position = respawn.position;
            p1Script.dead = false;
        }
        if(p1Script.inRespawn && !p1Script.dead)
        {
            respawnCount++;
            if(respawnCount > respawnLength)
            {
                print("resetting respawn");
                p1Script.inRespawn = false;
                p1Script.invincible = false;
                color.a = 1f;
                sprite.color = color;
                respawnCount = 0;
                incrementScore = true;
            }
        }
        if (p1Script.invincible)
        {
            if (color.a == 1)
            {
                color.a = 0.8f;
                sprite.color = color;
            }
            if (color.a > 0.7f)
            {
                alphaUp = false;
                alphaDown = true;
            }
            if (color.a < 0.3f)
            {
                alphaDown = false;
                alphaUp = true;
            }
            if (alphaUp)
            {
                color.a = color.a + 0.1f;
                sprite.color = color;
            }
            if (alphaDown)
            {
                color.a = color.a - 0.1f;
                sprite.color = color;
            }
        }

        /*if (p2Script.dead)
        {
            print("p1 dead");
            p2Script.invincible = true;
            p2Sprite.enabled = false;
            p1Script.killSound.Play();
            if (p1IncrementScore)
            {
                p1Score.gameScore++;
                p1IncrementScore = false;
            }
            p2DeathCount++;
            if (p2DeathCount > p2DeathLength)
            {
                p2Sprite.enabled = true;
                p2Script.inRespawn = true;
                p2DeathCount = 0;
            }
        }
        if (p2Script.inRespawn && p2Script.dead)
        {
            p2Trans.position = p2Respawn.position;
            p2Script.dead = false;
        }
        if (p2Script.inRespawn)
            print("in respawn");
        if (p1Script.inRespawn && !p1Script.dead)
        {
            p1RespawnCount++;
            if (p1RespawnCount > p1RespawnLength)
            {
                print("resetting respawn");
                p1Script.inRespawn = false;
                p1Script.invincible = false;
                p1Color.a = 1f;
                p1Sprite.color = p1Color;
                p1RespawnCount = 0;
                p1IncrementScore = true;
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
                alphaUp = false;
                alphaDown = true;
            }
            if (p1Color.a < 0.3f)
            {
                alphaDown = false;
                alphaUp = true;
            }
            if (alphaUp)
            {
                p1Color.a = p1Color.a + 0.1f;
                p1Sprite.color = p1Color;
            }
            if (alphaDown)
            {
                p1Color.a = p1Color.a - 0.1f;
                p1Sprite.color = p1Color;
            }
        }*/
    }
}
