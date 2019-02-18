﻿using System.Collections;
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

    public float forwardSpeed;
    public float backwardSpeed;
    public float jumpPower;

    public bool doubleJumpReady;
    public bool justJumped = false;

    public string LowPunch;
    public string HighPunch;
    public string LowKick;
    public string HighKick;
    public string FireBallAttack;

    public SpecialInput FireBall;
    public SpecialInput FireBallInverted;


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
    int stunFrames = 0;
    int blockStunFrames = 0;

    public GameObject opponent;

    // Use this for initialization
    void Start()
    {
        input = Manager.instance.input[playerNum - 1];
        anim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();

        string[] fireBallMotion = new string[] { "down", "right", "HighPunch" };
        FireBall = new SpecialInput(fireBallMotion, playerNum);
        string[] fireBallMotionInverted = new string[] { "down", "left", "HighPunch" };
        FireBallInverted = new SpecialInput(fireBallMotionInverted, playerNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (stunFrames > 0)
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

            if (Inverted())
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }

            if (input.GetButtonDown("right"))
            {
                body.velocity = RightVelocity();
            }
            else if (input.GetButtonDown("left"))
            {
                body.velocity = LeftVelocity();
            }
            InAirState = !IsGrounded();

            if (!InAirState)
                GroundUpdate();
            else
                AirUpdate();
        }
    }
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

        if (Inverted() && FireBallInverted.Check() || !Inverted() && FireBall.Check())
        {
            StartCoroutine(Attack(FireBallAttack));
        }
        else if (input.GetButtonDown("HighPunch"))
        {
            StartCoroutine(Attack(HighPunch));
        }
        if (input.GetButtonDown("up"))
        {
            body.velocity = JumpVelocity();
            justJumped = true;
        }
        if (input.GetButtonDown("LowPunch"))
        {
            StartCoroutine(Attack(LowPunch));
        }
        if (input.GetButtonDown("LowKick"))
        {
            StartCoroutine(Attack(LowKick));
        }
        if (input.GetButtonDown("HighKick"))
        {
            StartCoroutine(Attack(HighKick));
        }

        if (!input.GetButtonDown("right") && !input.GetButtonDown("left"))
            body.velocity = new Vector2(0, body.velocity.y);
    }
    void AirUpdate()
    {
        if (!input.GetButtonDown("up"))
            justJumped = false; // Prevents double jump from triggering with only one button press
        if (input.GetButtonDown("up") && doubleJumpReady && !justJumped)
        {
            body.velocity = new Vector2(body.velocity.x, JumpVelocity().y);
            doubleJumpReady = false;
        }
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
    public void AttackHit(int attackDamage, int attackStun, int blockStun, float hurtVelocity)
    {
        Debug.Log(gameObject.name + " was hit!");
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Block") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("BlockHurt") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchBlock") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchBlockHurt"))
        {
            BlockHurt(blockStun, 0);
        }
        else
        {
            manager.currentHP[playerNum - 1] -= attackDamage;
            GetHurt(attackStun, hurtVelocity);
        }
    }
    
    bool Blocking()
    {
        // Returns true if moving away from opponent
        return (input.GetButtonDown("right") && Inverted()) || (input.GetButtonDown("left") && !Inverted());
    }

    Vector2 RightVelocity()
    {
        if (Inverted())
            return new Vector2(-backwardSpeed, body.velocity.y);
        else
            return new Vector2(forwardSpeed, body.velocity.y);
    }
    Vector2 LeftVelocity()
    {
        if (Inverted())
            return new Vector2(-forwardSpeed, body.velocity.y);
        else
            return new Vector2(backwardSpeed, body.velocity.y);

    }
    Vector2 JumpVelocity()
    {
        return new Vector2(body.velocity.x, jumpPower);
    }

    IEnumerator Attack(string atName)
    {
        anim.SetBool(atName, true);
        yield return new WaitForSeconds(10 * Time.deltaTime); // 10 Frames
        anim.SetBool(atName, false);
    }
    public void GetHurt(int attackStun, float hurtVelocity)
    {
        HurtingState = true;
        stunFrames = attackStun;
        if (!Inverted())
            body.velocity = new Vector2(hurtVelocity, body.velocity.y);
        else
            body.velocity = new Vector2(-hurtVelocity, body.velocity.y);
    }
    public void BlockHurt(int attackStun, float hurtVelocity)
    {
        BlockHurtingState = true;
        blockStunFrames = attackStun;
        if (!Inverted())
            body.velocity = new Vector2(hurtVelocity, body.velocity.y);
        else
            body.velocity = new Vector2(-hurtVelocity, body.velocity.y);
    }


}
