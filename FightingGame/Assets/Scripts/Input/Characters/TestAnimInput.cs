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

    public float forwardSpeed;
    public float backwardSpeed;
    public float jumpPower;

    public bool doubleJumpReady;
    public bool justJumped = false;

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
    int stunFrames = 0;
    int blockStunFrames = 0;

    public GameObject opponent;
    public float distToGround;

    // Use this for initialization
    void Start()
    {
        input = Manager.instance.input[playerNum - 1];
        anim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();

        distToGround = gameObject.GetComponent<BoxCollider2D>().bounds.extents.y;
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
            if (IsGrounded())
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
            Debug.Log("Crouched");
            body.velocity = Vector2.zero;
        }
        if (Blocking())
        {
            BlockingState = true;
        }
        else
        {
            BlockingState = false;
        }
        if (input.GetButtonDown("up"))
        {
            body.velocity = JumpVelocity();
            justJumped = true;
        }
        if (input.GetButtonDown("LowPunch"))
        {
            StartCoroutine(Attack("Jab"));
        }
        if (input.GetButtonDown("HighPunch"))
        {
            StartCoroutine(Attack("Cross"));
        }
        
        if (!input.GetButtonDown("right") && !input.GetButtonDown("left"))
            body.velocity = Vector2.zero;
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
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.25f);
    }

    // AttackHit should be called by script on the attack hitbox
    // This is called when you hit someone
    public void AttackHit(Collider2D collision, int attackDamage, int attackStun, int blockStun, float hurtVelocity)
    {
        Debug.Log("Hit " + collision.gameObject.name + this.gameObject.name);

        // Blocked
        if (collision.GetComponent<TestAnimInput>().anim.GetCurrentAnimatorStateInfo(0).IsName("Block") ||
            collision.GetComponent<TestAnimInput>().anim.GetCurrentAnimatorStateInfo(0).IsName("BlockHurt"))
        {
            collision.GetComponent<TestAnimInput>().BlockHurt(blockStun, 0);
        }
        else
        {
            manager.currentHP[collision.gameObject.GetComponent<TestAnimInput>().playerNum - 1] -= attackDamage;
            collision.GetComponent<TestAnimInput>().GetHurt(attackStun, hurtVelocity);
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
        yield return new WaitForSeconds(0.1f); // 6 Frames
        anim.SetBool(atName, false);
    }
    public void GetHurt(int attackStun, float hurtVelocity)
    {
        HurtingState = true;
        stunFrames = attackStun;
        body.velocity = new Vector2(hurtVelocity, body.velocity.y);
    }
    public void BlockHurt(int attackStun, float hurtVelocity)
    {
        BlockHurtingState = true;
        blockStunFrames = attackStun;
        body.velocity = new Vector2(hurtVelocity, body.velocity.y);
    }


}
