using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]

public class Player1 : MonoBehaviour
{

    [Range(1, 50)] public float moveSpeed = 20;
    [Range(1, 100)] public float jumpPower = 50;
    [Range(1, 300)] public float fallSpeed = 100;
    [Range(1, 100)] public float groundedTraction = 50;
    [Range(1, 100)] public float airialTraction = 20;
    [Range(1, 100)] public float divekickSpeed = 40;
    [Range(0, 100)] public float slideAttackSpeed = 20;
    [Range(1, 100)] public float dashSpeed = 40;
    [Range(0, 20)] public float crouchSpeed = 5;
    [Range(1, 30)] public float dashActiveFrames = 15;
    [Range(1, 120)] public float clingActiveFrames = 30;
    [Range(1, 30)] public float slideAttackActiveFrames = 6;
    [Range(1, 50)] public float slideAttackCooldownFrames = 5;
    [Range(1, 50)] public float projectileCooldownFrames = 5;
    [Range(1, 30)] public float guardActiveFrames = 15;
    [Range(1, 100)] public float pushBackSpeed = 50;
    [Range(1, 100)] public float pushBackActiveFrames = 10;
    [Range(1, 100)] public float stunActiveFrames = 60;

    float dashFrameCount = 0;
    float clingFrameCount = 0;
    float slideAttackFrameCount = 0;
    float projectileFrameCount = 0;
    float guardFrameCount = 0;
    float pushBackFrameCount = 0;
    float stunFrameCount = 0;

    bool doubleJumped = false;
    [HideInInspector] public bool divekicked = false;
    [HideInInspector] public bool slideAttacked = false;
    [HideInInspector] public bool guarded = false;
    [HideInInspector] public bool shotProjectile = false;
    [HideInInspector] public bool inPushBack = false;
    [HideInInspector] public bool wallSplat = false;
    [HideInInspector] public bool stunned = false;
    [HideInInspector] public bool continueStun = false;
    bool canWallSplat = true;
    bool canSlideAttack = true;
    [HideInInspector] public bool facingLeft = false;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool hitWithProjectile = false;
    bool dashed = false;
    bool dashedLeft = false;
    bool dashedRight = false;
    bool moving = false;
    public static bool crouching = false;
    bool movingLeft = false;
    bool movingRight = false;
    bool rotated = false;
    bool inRespawn = false;

    bool hitOnLeftSide = false;
    bool hitOnRightSide = false;

    bool splattedLeft = false, splattedRight = false;
    bool stunnedOnLeftWall = false, stunnedOnRightWall = false;

    bool grounded;
    bool climbingSlope;
    bool descendingSlope;

    float accelerationTimeGrounded;
    float accelerationTimeAirborne;

    float timeToJumpApex;
    float gravity;

    float velocityXSmoothing;

    Vector3 velocity;

    Controller2D controller;

    SpriteRenderer playerSprite;
    SpriteRenderer playerIcon;
    SpriteRenderer opponentSprite;
    SpriteRenderer opponentIcon;

    Vector3 standingPos;

    Vector2 input;

    bool clinging = false;

    public Sprite runLeftSprite, runRightSprite, upClingLeftSprite, upClingRightSprite, upLeftClingSprite,
    upRightClingSprite, leftClingSprite, rightClingSprite, jumpFallLeft, jumpFallRight, crouchLeftSprite,
    crouchRightSprite, divekickRightSprite, divekickLeftSprite, slideAttackLeftSprite, slideAttackRightSprite,
    guardRightSprite, guardLeftSprite, pushBackLeftSprite, pushBackRightSprite, wallSplatLeftSprite,
        wallSplatRightSprite, stunnedLeftSprite, stunnedRightSprite, dashLeftSprite, dashRightSprite;

    Collider2D collidedObject;

    [HideInInspector] public BoxCollider2D boxCollider;

    bool clipping;
    float clipAmount;

    bool leftCollision, rightCollision, bottomCollision, topCollision;

    [HideInInspector] public bool dead = false;

    GameObject respawnUp, respawnDown, respawnRight, respawnLeft, opponent;

    BoxCollider2D activeAreaUp, activeAreaDown, activeAreaRight, activeAreaLeft;

    [HideInInspector] public bool inUpArea, inDownArea, inLeftArea, inRightArea;

    Player1 p1Script;
    Player2 p2Script;

    P1Score p1Score;
    P2Score p2Score;

    Projectile playerProjectileScript, opponentProjectileScript;

    AudioSource deathSound, clingSound;

    bool canPlayClingSound = true;

    public static bool p1Win = false;
    public static bool p2Win = false;

    Canvas p1WinCanvas, p2WinCanvas;

    bool lockFacing = false;

    PolygonCollider2D p1Hurtbox, p2Hurtbox;

    P1GameManager gm;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerIcon = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        respawnUp = GameObject.FindGameObjectWithTag("RespawnUp");
        respawnDown = GameObject.FindGameObjectWithTag("RespawnDown");
        respawnLeft = GameObject.FindGameObjectWithTag("RespawnLeft");
        respawnRight = GameObject.FindGameObjectWithTag("RespawnRight");
        if (gameObject.tag == "Player1")
        {
            opponent = GameObject.FindGameObjectWithTag("Player2");
        }
        if (gameObject.tag == "Player2")
        {
            opponent = GameObject.FindGameObjectWithTag("Player1");
        }
        activeAreaUp = GameObject.FindGameObjectWithTag("ActiveAreaUp").
            GetComponent<BoxCollider2D>();
        activeAreaDown = GameObject.FindGameObjectWithTag("ActiveAreaDown").
            GetComponent<BoxCollider2D>();
        activeAreaLeft = GameObject.FindGameObjectWithTag("ActiveAreaLeft").
            GetComponent<BoxCollider2D>();
        activeAreaRight = GameObject.FindGameObjectWithTag("ActiveAreaRight").
            GetComponent<BoxCollider2D>();
        p1Script = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<Player1>();
        p2Script = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<Player2>();
        opponentSprite = opponent.GetComponent<SpriteRenderer>();
        opponentIcon = opponent.GetComponentInChildren<SpriteRenderer>();
        p1Score = GameObject.FindGameObjectWithTag("P1Score").
            GetComponent<P1Score>();
        p2Score = GameObject.FindGameObjectWithTag("P2Score").
            GetComponent<P2Score>();
        p1Win = p2Win = false;
        opponentSprite.enabled = true;
        opponentIcon.enabled = true;
        p1WinCanvas = GameObject.FindGameObjectWithTag("P1Win").
            GetComponent<Canvas>();
        p2WinCanvas = GameObject.FindGameObjectWithTag("P2Win").
            GetComponent<Canvas>();
        p1WinCanvas.enabled = p2WinCanvas.enabled = false;
        deathSound = GetComponents<AudioSource>()[0];
        clingSound = GetComponents<AudioSource>()[1];
        p1Hurtbox = GameObject.FindGameObjectWithTag("Player1").
            GetComponent<PolygonCollider2D>();
        p2Hurtbox = GameObject.FindGameObjectWithTag("Player2").
            GetComponent<PolygonCollider2D>();
        playerProjectileScript = GameObject.FindGameObjectWithTag("P1Projectile").
            GetComponent<Projectile>();
        opponentProjectileScript = GameObject.FindGameObjectWithTag("P2Projectile").
            GetComponent<Projectile>();
        gm = P1GameManager.GM;
    }

    void Update()
    {
        UpdateScore();

        CheckActiveArea();

        DetectDeath();

        DetectDirectionalInputs();

        ColPhysChecks();

        DetectMovement();

        DetectDivekicking();

        DetectCrouching();

        DetectSlideAttack();

        DetectDash();

        DetectJumping();

        DetectClinging();

        DetectGuard();

        MovePlayer();

        DetectProjectile();

        DetectWallsplat();

        //DetectSideHit();

        if (guarded)
            print("guarding");
    }

    void UpdateScore()
    {
        if (p1Score.gameScore == 5)
        {
            p1Win = true;
            p1WinCanvas.enabled = true;
        }
        if (p2Score.gameScore == 5)
        {
            p2Win = true;
            p2WinCanvas.enabled = true;
        }

        if (p1Win && gameObject.tag == "Player1")
            Destroy(opponent);
        if (p2Win && gameObject.tag == "Player2")
            Destroy(opponent);
    }

    void CheckActiveArea()
    {
        if (boxCollider.bounds.center.x < activeAreaLeft.bounds.max.x && boxCollider.bounds.center.y < activeAreaLeft.bounds.max.y)
        {
            inLeftArea = true;
            inRightArea = inUpArea = inDownArea = false;
        }
        if (boxCollider.bounds.center.x > activeAreaRight.bounds.min.x && boxCollider.bounds.center.y < activeAreaRight.bounds.max.y)
        {
            inRightArea = true;
            inLeftArea = inUpArea = inDownArea = false;
        }
        if (boxCollider.bounds.center.x > activeAreaLeft.bounds.max.x && boxCollider.bounds.center.x < activeAreaRight.bounds.min.x && boxCollider.bounds.center.y < activeAreaUp.bounds.min.y)
        {
            inDownArea = true;
            inLeftArea = inUpArea = inRightArea = false;
        }
        if (boxCollider.bounds.center.y > activeAreaUp.bounds.min.y)
        {
            inUpArea = true;
            inLeftArea = inDownArea = inRightArea = false;
        }
    }

    void DetectDeath()
    {
        if (dead)
        {
            inRespawn = true;
            deathSound.Play();
            p2Score.gameScore++;
            if (p2Script.inLeftArea)
            {
                transform.position = respawnRight.transform.position;
                dead = false;
            }
            if (p2Script.inRightArea)
            {
                transform.position = respawnLeft.transform.position;
                dead = false;
            }
            if (p2Script.inUpArea)
            {
                transform.position = respawnDown.transform.position;
                dead = false;
            }
            if (p2Script.inDownArea)
            {
                transform.position = respawnUp.transform.position;
                dead = false;
            }
        }
    }

    void DetectDirectionalInputs()
    {
        if (Input.GetKey(gm.left))
            input.x = -1;
        else if (Input.GetKey(gm.right))
            input.x = 1;
        else
            input.x = 0;
        if (Input.GetKey(gm.down))
            input.y = -1;
        else if (Input.GetKey(gm.up))
            input.y = 1;
        else
            input.y = 0;
    }

    void ColPhysChecks()
    {
        grounded = controller.collisions.below;
        climbingSlope = controller.collisions.climbingSlope;
        descendingSlope = controller.collisions.descendingSlope;

        gravity = -fallSpeed * 2f;
        accelerationTimeGrounded = 1f / groundedTraction;
        accelerationTimeAirborne = 1f / airialTraction;

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    void DetectMovement()
    {
        if (input.x == 0 && input.y == 0)
            moving = false;
        else
            moving = true;

        if (input.x > 0)
        {
            movingRight = true;
        }

        if (input.x < 0)
        {
            movingLeft = true;
        }
    }

    void DetectDivekicking()
    {
        if (!divekicked && !slideAttacked && input.x > 0)
        {
            facingLeft = false;
            facingRight = true;
        }

        if (!divekicked && !slideAttacked && input.x < 0)
        {
            facingLeft = true;
            facingRight = false;
        }

        if (Input.GetKeyDown(gm.attack) && !controller.collisions.below && !divekicked)
            divekicked = true;

        if (controller.collisions.below)
        {
            divekicked = false;
            doubleJumped = false;
        }
    }

    void DetectCrouching()
    {
        if (!guarded &&
            !slideAttacked)
        {
            if (input.y == -1 &&
                !clinging &&
                !crouching &&
                !controller.collisions.isAirborne())
            {
                crouching = true;
            }
            if (input.y > -1 &&
                !controller.collisions.isAirborne() &&
                !clinging &&
                crouching ||
                controller.collisions.isAirborne())
            {
                crouching = false;
            }
        }

        if (crouching && facingLeft)
            playerSprite.sprite = crouchLeftSprite;
        if (crouching && facingRight)
            playerSprite.sprite = crouchRightSprite;
    }

    void DetectSlideAttack()
    {
        if (Input.GetKeyDown(gm.attack) && !controller.collisions.isAirborne() && crouching && canSlideAttack && !guarded)
        {
            slideAttacked = true;
            crouching = false;
            canSlideAttack = false;
        }

        if (slideAttacked && controller.collisions.isAirborne())
            slideAttacked = false;

        if (slideAttackFrameCount >= slideAttackActiveFrames)
        {
            slideAttacked = false;
        }

        if (slideAttackFrameCount >= (slideAttackActiveFrames + slideAttackCooldownFrames))
        {
            canSlideAttack = true;
            slideAttackFrameCount = 0;
        }

        if (!canSlideAttack)
        {
            slideAttackFrameCount++;
        }
    }

    void DetectDash()
    {
        if (moving &&
            Input.GetKeyDown(gm.attack) &&
            controller.collisions.below &&
            !dashed &&
            !slideAttacked)
        {
            dashed = true;
            if (facingLeft)
                dashedLeft = true;
            if (facingRight)
                dashedRight = true;
        }

        if (dashedLeft && facingRight && !controller.collisions.isAirborne())
        {
            dashed = false;
            dashedLeft = false;
        }
        if (dashedRight && facingLeft && !controller.collisions.isAirborne())
        {
            dashed = false;
            dashedRight = false;
        }

        if (dashed && clinging)
            dashed = false;

        if (dashed)
        {
            dashFrameCount++;
        }

        if (dashed && dashFrameCount >= dashActiveFrames && controller.collisions.below)
        {
            dashed = false;
            dashFrameCount = 0;
        }
    }

    void DetectJumping()
    {
        if (Input.GetKeyDown(gm.jump) && !inPushBack)
        {
            guarded = false;
            if (controller.collisions.below)
            {
                velocity.y = jumpPower;
            }
        }

        if (facingLeft && !clinging && !crouching && !guarded && !stunned)
            playerSprite.sprite = runLeftSprite;

        if (facingRight && !clinging && !crouching && !guarded && !stunned)
            playerSprite.sprite = runRightSprite;

        if (facingLeft && controller.collisions.isAirborne() && !clinging)
            playerSprite.sprite = jumpFallLeft;

        if (facingRight && controller.collisions.isAirborne() && !clinging)
            playerSprite.sprite = jumpFallRight;

        if (Input.GetKeyDown(gm.jump) &&
            !controller.collisions.below &&
            !doubleJumped &&
            !clinging)
        {
            velocity.y = jumpPower;
            doubleJumped = true;
        }
    }

    void DetectClinging()
    {
        Vector2 clingNormal = controller.collisions.normal;

        if (controller.collisions.isAirborne() && !clinging)
            clingNormal = Vector2.zero;

        float clingAngle = Vector2.Angle(clingNormal, Vector2.up);

        bool upCling, upLeftCling, upRightCling, leftCling, rightCling;
        upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

        if (clingAngle == 180f)
        {
            upCling = true;
            if (facingLeft)
                playerSprite.sprite = upClingLeftSprite;
            if (facingRight)
                playerSprite.sprite = upClingRightSprite;
        }

        if (clingAngle == 135f)
        {
            if (clingNormal.x < 0)
            {
                upRightCling = true;
                playerSprite.sprite = upRightClingSprite;
            }
            if (clingNormal.x > 0)
            {
                upLeftCling = true;
                playerSprite.sprite = upLeftClingSprite;
            }
        }

        if (clingAngle > 88f && clingAngle < 92f)
        {
            if (clingNormal.x < 0)
            {
                rightCling = true;
                playerSprite.sprite = rightClingSprite;
            }
            if (clingNormal.x > 0)
            {
                leftCling = true;
                playerSprite.sprite = leftClingSprite;
            }
        }

        if (clingNormal.x < 0)
            print("clingNormal.x = " + clingNormal.x);

        if (!inPushBack &&
            (upCling || upLeftCling || upRightCling || leftCling || rightCling))
            clinging = true;

        if (clinging)
            clingFrameCount++;

        print("cling angle = " + clingAngle);

        if (clinging && Input.GetKeyDown(gm.jump))
        {
            divekicked = false;
            canPlayClingSound = true;
            velocity = clingNormal;
            if (upCling)
            {
                velocity.y = jumpPower * clingNormal.y / 2;
            }
            if (upRightCling || upLeftCling)
            {
                velocity.x = jumpPower * clingNormal.x / 2;
                velocity.y = jumpPower * clingNormal.y / 2;
            }
            if (rightCling)
            {
                facingLeft = true;
                facingRight = false;
                velocity.x = jumpPower * clingNormal.x / 2;
                velocity.y = jumpPower;
            }
            if (leftCling)
            {
                facingLeft = false;
                facingRight = true;
                velocity.x = jumpPower * clingNormal.x / 2;
                velocity.y = jumpPower;
            }

            clinging = upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

            if (facingLeft)
                playerSprite.sprite = jumpFallLeft;
            if (facingRight)
                playerSprite.sprite = jumpFallRight;
        }

        if (clinging && Input.GetKeyDown(gm.attack))
        {
            canPlayClingSound = true;
            if (upRightCling || rightCling)
            {
                facingLeft = true;
                facingRight = false;
                divekicked = true;
            }
            if (upLeftCling || leftCling)
            {
                facingLeft = false;
                facingRight = true;
                divekicked = true;
            }
            if (upCling)
                divekicked = true;

            clinging = upCling = upLeftCling = upRightCling = leftCling = rightCling = false;

            if (facingLeft)
                playerSprite.sprite = divekickLeftSprite;
            if (facingRight)
                playerSprite.sprite = divekickRightSprite;
        }

        if (clinging && clingFrameCount >= clingActiveFrames)
        {

        }

        if (clinging)
        {
            print("clinging");
            velocity.x = 0;
            velocity.y = 0;
            controller.Move(velocity * Time.deltaTime);
            if (canPlayClingSound)
                clingSound.Play();
            canPlayClingSound = false;
            if (inPushBack || wallSplat)
                clinging = false;
        }

        if (controller.collisions.below && canPlayClingSound)
        {
            clingSound.Play();
            canPlayClingSound = false;
        }

        if (controller.collisions.isAirborne() && !clinging)
            canPlayClingSound = true;
    }

    void DetectGuard()
    {
        if (!guarded)
            lockFacing = false;

        if (Input.GetKey(gm.guard) &&
            !controller.collisions.isAirborne() &&
            !inRespawn)
            guarded = true;
        else
            guarded = false;

        if (inRespawn &&
            controller.collisions.isAirborne())
            inRespawn = false;

        if (guarded && controller.collisions.isAirborne())
            print("p1 guarding in air");

        if (controller.collisions.isAirborne())
            print("p1 airborne");

        /*if (guardFrameCount >= guardActiveFrames) {
			guarded = false;
			guardFrameCount = 0;
		}*/

        if (guarded)
        {
            //guardFrameCount++;
            if (facingLeft && !lockFacing)
            {
                playerSprite.sprite = guardLeftSprite;
                lockFacing = true;
            }
            if (facingRight && !lockFacing)
            {
                playerSprite.sprite = guardRightSprite;
                lockFacing = true;
            }
        }

        if (Input.GetKeyDown(gm.jump))
            guarded = false;
    }

    void DetectProjectile()
    {
        if (Input.GetKeyDown(gm.projectile) &&
            !divekicked &&
            !slideAttacked &&
            !guarded &&
            !clinging &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
            shotProjectile = true;

        if (shotProjectile)
            projectileFrameCount++;

        if (projectileFrameCount >= projectileCooldownFrames)
        {
            shotProjectile = false;
            projectileFrameCount = 0;
        }

        if (hitWithProjectile && !inPushBack)
        {
            inPushBack = true;
            hitWithProjectile = false;
        }
    }

    void DetectWallsplat()
    {
        if (inPushBack &&
            (controller.collisions.left || controller.collisions.right || clinging) &&
            canWallSplat &&
            !stunned)
        {
            print("activate wallsplat");
            inPushBack = false;
            wallSplat = true;
            velocity.y = 0;
            if (controller.collisions.left)
                splattedLeft = true;
            if (controller.collisions.right)
                splattedRight = true;
            canWallSplat = false;
        }

        if (wallSplat)
        {
            inPushBack = false;
            //playerSprite.sprite = wallSplatLeftSprite;
            /* if (controller.collisions.below)
             {
                 wallSplat = false;
                 stunned = true;
             }*/
        }

        if (wallSplat && controller.collisions.below)
        {
            inPushBack = false;
            wallSplat = false;
            stunned = true;
            if (splattedLeft)
                stunnedOnLeftWall = true;
            if (splattedRight)
                stunnedOnRightWall = true;
        }

        if (continueStun)
        {
            stunFrameCount = 0;
            continueStun = false;
        }

        if (stunned)
        {
            stunFrameCount++;
            if (stunnedOnRightWall)
            {
                playerSprite.sprite = stunnedLeftSprite;
            }
            if (stunnedOnLeftWall)
            {
                playerSprite.sprite = stunnedRightSprite;
            }
        }

        if ((stunFrameCount >= stunActiveFrames) || inRespawn)
        {
            print("stun over");
            stunned = false;
            stunFrameCount = 0;
            canWallSplat = true;
            if (inPushBack)
                print("inpushback right after stun is over");
        }
    }

    /*void DetectSideHit()
    {
        hitOnLeftSide = false;
        hitOnRightSide = false;
        if (p1Hurtbox.bounds.max.x > p2Hurtbox.bounds.min.x &&
            p1Hurtbox.bounds.min.x < p2Hurtbox.bounds.max.x &&
            p1Hurtbox.bounds.max.y > p2Hurtbox.bounds.min.y &&
            p1Hurtbox.bounds.min.y < p2Hurtbox.bounds.max.y)
        {
            print("p1 inside p2");
            if (p1Script.facingRight && p1Script.divekicked)
            {
                print("divekicked from left");
                hitOnLeftSide = true;
            }
            if (p1Script.facingLeft && p1Script.divekicked)
            {
                print("divekicked from right");
                hitOnRightSide = true;
            }
        }
    }*/

    void MovePlayer()
    {
        //right divekick movement
        if (facingRight &&
            divekicked &&
            controller.collisions.isAirborne() &&
            !clinging &&
            !guarded &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            velocity.y = -divekickSpeed;
            velocity.x = divekickSpeed;
            playerSprite.sprite = divekickRightSprite;
            controller.Move(velocity * Time.deltaTime);
        }

        //left divekick movement
        if (facingLeft &&
            divekicked &&
            controller.collisions.isAirborne() &&
            !clinging &&
            !guarded &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            velocity.y = -divekickSpeed;
            velocity.x = -divekickSpeed;
            playerSprite.sprite = divekickLeftSprite;
            controller.Move(velocity * Time.deltaTime);
        }

        if (facingRight)
            print("facing right");
        if (facingLeft)
            print("facing left");

        //right slide attack movement
        if (facingRight &&
            slideAttacked &&
            !clinging &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.x = slideAttackSpeed;
            playerSprite.sprite = slideAttackRightSprite;
            controller.Move(velocity * Time.deltaTime);
        }

        //left slide attack moevement
        if (facingLeft &&
            slideAttacked &&
            !clinging &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.x = -slideAttackSpeed;
            playerSprite.sprite = slideAttackLeftSprite;
            controller.Move(velocity * Time.deltaTime);
        }

        //dash movement
        if (!divekicked &&
            dashed &&
            !clinging &&
            !crouching &&
            !guarded &&
            !slideAttacked &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            float targetVelocityX = input.x * dashSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            if (facingLeft && !controller.collisions.isAirborne())
                playerSprite.sprite = dashLeftSprite;
            if (facingRight && !controller.collisions.isAirborne())
                playerSprite.sprite = dashRightSprite;
            controller.Move(velocity * Time.deltaTime);
        }

        //run movement
        if (!divekicked &&
            !dashed &&
            !clinging &&
            !crouching &&
            !guarded &&
            !slideAttacked &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        //crouch movement
        if (!divekicked &&
            !dashed &&
            !clinging &&
            crouching &&
            !guarded &&
            !slideAttacked &&
            !inPushBack &&
            !wallSplat &&
            !stunned)
        {
            float targetVelocityX = input.x * crouchSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        if (opponentProjectileScript.shotRight)
            print("p2 shot right");
        if (opponentProjectileScript.shotLeft)
            print("p2 shot left");

        if (inPushBack &&
            !stunned &&
            opponentProjectileScript.shotRight)
        {
            hitOnLeftSide = true;
        }

        if (inPushBack &&
            !stunned &&
            opponentProjectileScript.shotLeft)
        {
            hitOnRightSide = true;
        }

        //pushback right movement
        if (inPushBack &&
            !stunned &&
            hitOnLeftSide)
        {
            hitOnLeftSide = true;
            hitWithProjectile = false;
            print("inpushback");
            velocity.y += gravity * Time.deltaTime;
            velocity.x = pushBackSpeed;
            pushBackFrameCount++;
            playerSprite.sprite = pushBackLeftSprite;
            controller.Move(velocity * Time.deltaTime);
            if (pushBackFrameCount > pushBackActiveFrames)
                inPushBack = false;
        }

        //pushback left movement
        if (inPushBack &&
            !stunned &&
            hitOnRightSide)
        {
            hitOnRightSide = true;
            hitWithProjectile = false;
            print("inpushback");
            velocity.y += gravity * Time.deltaTime;
            velocity.x = -pushBackSpeed;
            pushBackFrameCount++;
            playerSprite.sprite = pushBackRightSprite;
            controller.Move(velocity * Time.deltaTime);
            if (pushBackFrameCount > pushBackActiveFrames)
                inPushBack = false;
        }

        if (!inPushBack || wallSplat || stunned)
        {
            hitOnLeftSide = false;
            hitOnRightSide = false;
        }

        if (inPushBack && stunned)
            inPushBack = false;

        if (!inPushBack &&
            !stunned)
        {
            pushBackFrameCount = 0;
        }

        //wallsplat movement
        if (wallSplat &&
            !stunned)
        {
            velocity.x = 0;
            velocity.y += gravity * Time.deltaTime;
            if (splattedLeft)
            {
                playerSprite.sprite = wallSplatRightSprite;
                facingRight = true;
                facingLeft = false;
            }
            if (splattedRight)
            {
                playerSprite.sprite = wallSplatLeftSprite;
                facingLeft = true;
                facingRight = false;
            }
            controller.Move(velocity * Time.deltaTime);
        }

        if (!wallSplat)
        {
            splattedLeft = false;
            splattedRight = false;
        }
    }

    /*void DetectClipping() {
		if (collidedObject != null) {
			if (boxCollider.bounds.min.y < collidedObject.bounds.max.y)
				clipping = true;
			else
				clipping = false;
		}
		if (clipping && velocity.y < 0) {
			clipAmount = collidedObject.bounds.max.y - boxCollider.bounds.min.y;
			transform.position = new Vector3 (transform.position.x, transform.position.y + clipAmount, transform.position.z);
			print (clipAmount);
		}

		if (clipping)
			print ("clipping");
	}*/
}
