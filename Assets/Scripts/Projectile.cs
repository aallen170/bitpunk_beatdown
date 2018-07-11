using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour {

    [Range(0, 30)] public float activeFrames = 10;
    [Range(0, 200)] public float speed = 100;
    float activeFrameCount = 0;

    [HideInInspector] public bool shotLeft = false;
    [HideInInspector] public bool shotRight = false;
    bool canShoot = true;
    bool lockProjectile = false;

    bool p1ProjIsVisible = false, p2ProjIsVisible = false;

    BoxCollider2D p1Hitbox, p2Hitbox;

    SpriteRenderer p1ProjSprite, p2ProjSprite;

    Player1 p1Script;
    Player2 p2Script;

    Transform p1Trans, p2Trans;

    PolygonCollider2D p1Hurtbox, p2Hurtbox;

	void Start () {
        p1Hitbox = GameObject.FindGameObjectWithTag("P1Projectile").
            GetComponent<BoxCollider2D>();
        p2Hitbox = GameObject.FindGameObjectWithTag("P2Projectile").
            GetComponent<BoxCollider2D>();
        p1Script = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Player1>();
        p2Script = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Player2>();
        p1Trans = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Transform>();
        p2Trans = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Transform>();
        p1Hurtbox = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<PolygonCollider2D>();
        p2Hurtbox = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<PolygonCollider2D>();
        p1ProjSprite = GameObject.FindGameObjectWithTag("P1Projectile").
            GetComponent<SpriteRenderer>();
        p2ProjSprite = GameObject.FindGameObjectWithTag("P2Projectile").
            GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        if (gameObject.tag == "P1Projectile")
        {
            if (!p1Script.shotProjectile &&
                (activeFrameCount >= activeFrames))
            {
                p1ProjIsVisible = false;
                shotRight = false;
                shotLeft = false;
                canShoot = true;
            }
            if (p1Script.shotProjectile &&
                (activeFrameCount < activeFrames))
                p1ProjIsVisible = true;
            if (!p1ProjIsVisible)
            {
                transform.position = p1Trans.position;
                activeFrameCount = 0;
                p1ProjSprite.enabled = false;
                p1Hitbox.enabled = false;
            }
            if (p1ProjIsVisible)
            {
                activeFrameCount++;
                if (p1Script.facingRight && canShoot)
                {
                    shotRight = true;
                    canShoot = false;
                }
                if (p1Script.facingLeft && canShoot)
                {
                    shotLeft = true;
                    canShoot = false;
                }
                if (shotRight)
                {
                    transform.position += new Vector3(speed/100, 0, 0);
                }
                if (shotLeft)
                {
                    transform.position += new Vector3(-speed/100, 0, 0);
                }
                p1ProjSprite.enabled = true;
                p1Hitbox.enabled = true;
                if (p1Hitbox.bounds.max.x > p2Hurtbox.bounds.min.x &&
                    p1Hitbox.bounds.min.x < p2Hurtbox.bounds.max.x &&
                    p1Hitbox.bounds.max.y > p2Hurtbox.bounds.min.y &&
                    p1Hitbox.bounds.min.y < p2Hurtbox.bounds.max.y)
                {
                    print("p1 projectile hit p2 hurtbox");
                    p2Script.inPushBack = true;
                    p2Script.hitWithProjectile = true;
                    p1ProjIsVisible = false;
                    if (p2Script.stunned)
                        p2Script.continueStun = true;
                }
            }
        }
        if (gameObject.tag == "P2Projectile")
        {
            if (!p2Script.shotProjectile &&
                (activeFrameCount >= activeFrames))
            {
                p2ProjIsVisible = false;
                shotRight = false;
                shotLeft = false;
                canShoot = true;
            }
            if (p2Script.shotProjectile &&
                (activeFrameCount < activeFrames))
                p2ProjIsVisible = true;
            if (!p2ProjIsVisible)
            {
                transform.position = p2Trans.position;
                activeFrameCount = 0;
                p2ProjSprite.enabled = false;
                p2Hitbox.enabled = false;
            }
            if (p2ProjIsVisible)
            {
                activeFrameCount++;
                if (p2Script.facingRight && canShoot)
                {
                    shotRight = true;
                    canShoot = false;
                }
                if (p2Script.facingLeft && canShoot)
                {
                    shotLeft = true;
                    canShoot = false;
                }
                if (shotRight)
                {
                    transform.position += new Vector3(speed / 100, 0, 0);
                }
                if (shotLeft)
                {
                    transform.position += new Vector3(-speed / 100, 0, 0);
                }
                p2ProjSprite.enabled = true;
                p2Hitbox.enabled = true;
                if (p2Hitbox.bounds.max.x > p1Hurtbox.bounds.min.x &&
                    p2Hitbox.bounds.min.x < p1Hurtbox.bounds.max.x &&
                    p2Hitbox.bounds.max.y > p1Hurtbox.bounds.min.y &&
                    p2Hitbox.bounds.min.y < p1Hurtbox.bounds.max.y)
                {
                    print("p2 projectile hit p1 hurtbox");
                    p1Script.inPushBack = true;
                    p1Script.hitWithProjectile = true;
                    p2ProjIsVisible = false;
                    if (p1Script.stunned)
                        p1Script.continueStun = true;
                }
            }
        }
    }
}
