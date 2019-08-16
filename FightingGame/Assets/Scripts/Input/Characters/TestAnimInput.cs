using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimInput : MonoBehaviour
{

    public int playerNum;
    private InputManager input;
    private Animator anim;

    public Animator Anim
    {
        get { return anim; }
    }


    private Rigidbody2D body;
    private Manager manager = Manager.instance;
    public BoxCollider2D ground;
    public BoxCollider2D[] walls;

    public float forwardSpeed;
    public float backwardSpeed;
    public float jumpPower;

    public bool doubleJumpReady;
    public bool justJumped = false;
    public bool knockDownLanded = false;
    public bool invulnerable = false;

    public string WeakAttackName;
    public string StrongAttackName;
    public string ThrowButtonAttackName;
    public string SpecialButtonAttackName;

    public string QuarterForwardAttackName;

    public SpecialInput FireBall;
    public SpecialInput FireBallInverted;
    public SpecialInput ThrowCommand;
    public SpecialInput ThrowCommandInvert;
    
    public GameObject ThrownObject;
    public GameObject ThrowStart;
    public GameObject ThrowEnd;
    public float ThrowVelocityX;
    public float ThrowVelocityY;
    private Vector2 ThrowVelocity;

    // AnimStateInfo
    bool HurtingState
    {
        set { anim.SetBool("Hurt", value); }
        get { return anim.GetBool("Hurt"); }
    }
    bool BlockingState
    {
        set { anim.SetBool("Block", value); }
        get { return anim.GetBool("Block"); }
    }
    bool BlockHurtingState
    {
        set { anim.SetBool("BlockHurt", value); }
        get { return anim.GetBool("BlockHurt"); }
    }
    bool CrouchingState
    {
        set { anim.SetBool("Crouch", value); }
        get { return anim.GetBool("Crouch"); }
    }
    bool InAirState
    {
        set { anim.SetBool("InAir", value); }
        get { return anim.GetBool("InAir"); }
    }
    bool KnockedDownState
    {
        set { anim.SetBool("Toppled", value); }
        get { return anim.GetBool("Toppled"); }
    }

    public bool CanWalk // Allowed to press left or right
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Block") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("InAir") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Landing");
        }
    }

    bool ThisPlayerBeingThrown
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName("GettingThrown") 
                || anim.GetCurrentAnimatorStateInfo(0).IsName("KnockedDown");
        }
    }
    bool PlayerBeingThrown
    {
        get
        {
            return ThisPlayerBeingThrown ||
                opponent.GetComponent<TestAnimInput>().ThisPlayerBeingThrown;
        }
    }

    bool OpponentTouchingWall
    {
        get { return opponent.GetComponent<Rigidbody2D>().IsTouching(walls[0]) 
                || opponent.GetComponent<Rigidbody2D>().IsTouching(walls[1]); }
    }


    int stunFrames = 0;
    int blockStunFrames = 0;

    public GameObject opponent;
    bool TooCloseToOpponent
    {
        get { return Physics2D.Distance(GetComponent<Collider2D>(), 
            opponent.GetComponent<Collider2D>()).distance < 0.2; }
    }
    float forceOfRepulsion = 100;
    float forceOfRepulsionInverted = -100;

    // Use this for initialization
    void Start()
    {
        input = Manager.instance.input[playerNum - 1];
        anim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();

        ThrowVelocity = new Vector2(ThrowVelocityX, ThrowVelocityY);

        string[] fireBallMotion = new string[] { "down", "right", "HighPunch" };
        FireBall = new SpecialInput(fireBallMotion, playerNum);
        string[] fireBallMotionInverted = new string[] { "down", "left", "HighPunch" };
        FireBallInverted = new SpecialInput(fireBallMotionInverted, playerNum);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), opponent.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {

        // Drop the character
        if (ThrownObject != null && CanWalk)
        {
            ThrownObject.GetComponent<Rigidbody2D>().simulated = true;
            ThrownObject.GetComponent<Animator>().Play("Idle");
            ThrownObject = null;
        }

        InAirState = !IsGrounded();

        if (KnockedDownState)
        {
            if(IsGrounded())
            {
                invulnerable = true;
                body.velocity = Vector2.zero;
                stunFrames = 0;
                HurtingState = false;
                if(!knockDownLanded)
                {
                    knockDownLanded = true;
                    StartCoroutine(Attack("Toppled", 120));
                }
            }
        }
        else if (stunFrames > 0)
        {
            HurtingState = true;
            stunFrames--;
        }
        else if (blockStunFrames > 0)
        {
            BlockHurtingState = true;
            blockStunFrames--;
        }
        else
        {
            HurtingState = false;
            BlockHurtingState = false;
            knockDownLanded = false;
            invulnerable = false;

            if (Inverted())
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }


            #region Movement

            // Normal Movement
            if (!TooCloseToOpponent)
            {
                if (input.GetButtonDown("right"))
                {
                    body.velocity = RightVelocity();
                }
                if (input.GetButtonDown("left"))
                {
                    body.velocity = LeftVelocity();
                }
            }
            // Touching the opponent
            else
            {

                // Touching at wall
                if (OpponentTouchingWall)
                {
                    if (input.GetButtonDown("right"))
                    {
                        if (Inverted()) //Inverted + right = retreat, normal
                        {
                            body.velocity = RightVelocity();
                        }
                        else // Not inverted + right = approaching intersect, nothing should happen
                        {
                            body.velocity = new Vector2(0, body.velocity.y);
                        }
                    }
                    if (input.GetButtonDown("left"))
                    {
                        if (Inverted()) //Inverted + left = advance, nothing should happen
                        {
                            body.velocity = new Vector2(0, body.velocity.y);
                        }
                        else // Not inverted + right = retreating, normal
                        {
                            body.velocity = LeftVelocity();
                        }
                    }
                }
                // Touching, but not at wall
                else
                {
                    if (input.GetButtonDown("right"))
                    {
                        if (Inverted()) //Inverted + right = retreat
                        {
                            body.velocity = RightVelocity();
                        }
                        else // Not inverted + right = approaching intersect, both move same direction
                        {
                            opponent.GetComponent<Rigidbody2D>().velocity = new Vector2(RightVelocity().x / 2,
                                opponent.GetComponent<TestAnimInput>().body.velocity.y);
                            body.velocity = new Vector2(RightVelocity().x / 2, body.velocity.y);
                        }
                    }
                    if (input.GetButtonDown("left"))
                    {
                        if (Inverted()) //Inverted + left = advance
                        {
                            opponent.GetComponent<Rigidbody2D>().velocity = new Vector2(LeftVelocity().x / 2,
                                opponent.GetComponent<TestAnimInput>().body.velocity.y);
                            body.velocity = new Vector2(LeftVelocity().x / 2, body.velocity.y);
                        }
                        else // Not inverted + left = retreat
                        {
                            body.velocity = LeftVelocity();
                        }
                    }
                }
            }

            #endregion

            if (!InAirState)
                GroundUpdate();
            else
                AirUpdate();
        }
    }

    // Reset double jump, handle ground inputs (attacks, walking, crouching)
    void GroundUpdate()
    {
        doubleJumpReady = true;

        if (input.GetButtonDown("down"))
        {
            body.velocity = Vector2.zero;
            CrouchingState = true;
        }
        else
        {
            CrouchingState = false;
        }
        if (Blocking())
        {
            BlockingState = true;
        }
        else
        {
            BlockingState = false;
        }

        /*
        if (Inverted() && FireBallInverted.Check() || !Inverted() && FireBall.Check())
        {
            StartCoroutine(Attack(QuarterForwardAttackName));
        }
         */


        if (input.GetButtonDown("up"))
        {
            StartCoroutine(Attack("Jump", 1));
        }  
        if (input.GetButtonDown("WeakAttack"))
        {
            StartCoroutine(Attack(WeakAttackName));
        }
        else if (input.GetButtonDown("StrongAttack"))
        {
            StartCoroutine(Attack(StrongAttackName));
        }
        if (input.GetButtonDown("Throw"))
        {
            StartCoroutine(Attack(ThrowButtonAttackName));
        }
        if (input.GetButtonDown("Special"))
        {
            StartCoroutine(Attack(SpecialButtonAttackName));
        }

        // Instant stop if you can walk and aren't violating the space (in case of jumping in?)
        if (!input.GetButtonDown("right") && !input.GetButtonDown("left") && !TooCloseToOpponent && CanWalk)
            body.velocity = new Vector2(0, body.velocity.y);
        // Instant ground stop if you can't walk
        if (!CanWalk)
            body.velocity = new Vector2(0, body.velocity.y);

        if (TooCloseToOpponent && !input.GetButtonDown("left")
            && !input.GetButtonDown("right")
            && !opponent.GetComponent<TestAnimInput>().input.GetButtonDown("left")
            && !opponent.GetComponent<TestAnimInput>().input.GetButtonDown("right")
            && !PlayerBeingThrown)
        {
            Debug.Log("Repulsion!");
            if (Inverted())
            {
                opponent.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceOfRepulsionInverted, 0));
            }
            else
            {
                opponent.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceOfRepulsion, 0));
            }
        }
    }
    void AirUpdate()
    {
        if (!input.GetButtonDown("up"))
            justJumped = false; // Prevents double jump from triggering with only one button press
        if (input.GetButtonDown("up") && doubleJumpReady && !justJumped)
        {
            body.velocity = new Vector2(0, JumpVelocity().y);
            doubleJumpReady = false;
        }
        if (!input.GetButtonDown("right") && !input.GetButtonDown("left") && !TooCloseToOpponent && !PlayerBeingThrown)
            body.velocity = new Vector2(0, body.velocity.y);
    }


    public bool Inverted()
    {
        // If opponent is to the left, then inverted
        if (opponent.transform.position.x < gameObject.transform.position.x)
            return true;
        return false;
    }

    public bool IsGrounded()
    {
        return gameObject.GetComponent<BoxCollider2D>().IsTouching(ground);
    }

    // AttackHit should be called by script on the attack hitbox
    // This is called on the player that gets hit's testAnimInput
    public void AttackHit(int attackDamage, int attackStun, int blockStun, Vector2 hurtVelocity,
        bool KnockDownAttack, bool ReStandAttack)
    {
        Debug.Log(gameObject.name + " was hit!");


        if (!invulnerable)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Block") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("BlockHurt") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchBlock") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchBlockHurt"))
            {
                BlockHurt(blockStun, 0);
            }
            else
            {
                if (KnockDownAttack)
                {
                    knockDownLanded = false; // He's up there now!
                    KnockedDownState = true;
                }
                if (ReStandAttack)
                    KnockedDownState = false;
                manager.currentHP[playerNum - 1] -= attackDamage;
                if (manager.currentHP[playerNum - 1] <= 0)
                {
                    PlayerDie();
                }
                GetHurt(attackStun, hurtVelocity);
            }
        }
    }

    // Called by the throw collider, can be used as a way to do damage w/o stuns and velocity (i.e. poison)
    public void AttackHit(int attackDamage)
    {
        manager.currentHP[playerNum - 1] -= attackDamage;
        if (manager.currentHP[playerNum - 1] <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        gameObject.SetActive(false);
    }
    
    bool Blocking()
    {
        // Returns true if moving away from opponent
        return (input.GetButtonDown("right") && Inverted()) || (input.GetButtonDown("left") && !Inverted());
    }

    Vector2 RightVelocity()
    {
        if(ThisPlayerBeingThrown)
        {
            return body.velocity;
        }
        if (Inverted())
            return new Vector2(-backwardSpeed, body.velocity.y);
        else
            return new Vector2(forwardSpeed, body.velocity.y);
    }
    Vector2 LeftVelocity()
    {
        if(ThisPlayerBeingThrown)
        {
            return body.velocity;
        }
        if (Inverted())
            return new Vector2(-forwardSpeed, body.velocity.y);
        else
            return new Vector2(backwardSpeed, body.velocity.y);

    }

    public void Jump() // Called by animator
    {
        body.velocity = JumpVelocity();
        justJumped = true;
    }
    Vector2 JumpVelocity()
    {
        return new Vector2(body.velocity.x, jumpPower);
    }

    IEnumerator Attack(string atName, int frames = 4)
    {
        anim.SetBool(atName, true);
        yield return new WaitForSeconds(frames * Time.deltaTime);
        anim.SetBool(atName, false);
    }

    public void ThrowAttack(GameObject thrownOpponent) // Called by the character that hit the throw's attack collider
    {
        Debug.Log("Throw hit " + thrownOpponent.name);

        // Saves it for ThrowHold, which is called by the animation, not the hitbox.
        ThrownObject = thrownOpponent;
        ThrownObject.transform.position = ThrowStart.transform.position;

        // This is what causes the character to not go into throw whiff frames
        StartCoroutine(Attack("ThrowSuccess"));

        // Handles opponent processing
        StartCoroutine(ThrownObject.GetComponent<TestAnimInput>().Attack("GettingThrown"));
        ThrownObject.GetComponent<TestAnimInput>().doubleJumpReady = false; // Being put in air, feelsbad.

        ThrownObject.GetComponent<Rigidbody2D>().simulated = false;
    }

    public void ThrowHold() // Called by animation, not a script
    {
        // Should NEVER happen
        if (ThrownObject == null)
        {
            Debug.Log("ThrowHold fails; no target");
        }
        // Always happens, unless terrible bugs lol
        // TODO: Add capability for backthrow
        else
        {
            ThrownObject.transform.position = ThrowEnd.transform.position;
            ThrownObject.GetComponent<Rigidbody2D>().simulated = true;
            ThrownObject.GetComponent<TestAnimInput>().KnockedDownState = true;
            if (Inverted())
            {                
                ThrownObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-ThrowVelocity.x, ThrowVelocityY);
                if(input.GetButtonDown("Right"))
                {
                    ThrownObject.GetComponent<Rigidbody2D>().velocity = ThrowVelocity;
                }
            }
            else
            {
                ThrownObject.GetComponent<Rigidbody2D>().velocity = ThrowVelocity;
                if(input.GetButtonDown("Left"))
                {
                    ThrownObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-ThrowVelocity.x, ThrowVelocityY);
                }
            }
            ThrownObject = null;
        }
    }

    public void GetHurt(int attackStun, Vector2 hurtVelocity)
    {
        HurtingState = true;
        stunFrames = attackStun;
        if (Inverted()) // Inverted means +, because away from the center
            body.velocity = new Vector2(hurtVelocity.x, hurtVelocity.y);
        else
            body.velocity = new Vector2(-hurtVelocity.x, hurtVelocity.y);
    }
    public void BlockHurt(int attackStun, float hurtVelocity)
    {
        BlockHurtingState = true;
        blockStunFrames = attackStun;
        if (Inverted())
            body.velocity = new Vector2(hurtVelocity, body.velocity.y);
        else
            body.velocity = new Vector2(-hurtVelocity, body.velocity.y);
    }

    public void ToggleContinuousCollisionDetection(float onOrOff)
    {
        if (onOrOff == 1)
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        else if (onOrOff == 0)
            body.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
    }
}
