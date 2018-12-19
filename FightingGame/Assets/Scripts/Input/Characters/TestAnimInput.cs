using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimInput : MonoBehaviour
{

    public int playerNum;
    private InputManager input;
    private Animator anim;
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

    public GameObject opponent;
    public float distToGround;

    // Use this for initialization
    void Start()
    {
        input = Manager.instance.input[playerNum - 1];
        anim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();

        distToGround = gameObject.GetComponent<BoxCollider2D>().bounds.extents.y;
        Debug.Log(distToGround);
    }

    // Update is called once per frame
    void Update()
    {
        // Flip code
        if (Inverted())
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (Right())
        {
            body.velocity = RightVelocity();
        }
        else if (Left())
        {
            body.velocity = LeftVelocity();
        }
        if (IsGrounded())
            GroundUpdate();
        else
            AirUpdate();
    }
    void GroundUpdate()
    {
        justJumped = false;
        
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
        doubleJumpReady = true;
        if (HurtingState)
            Debug.Log("Pain");
        else if (!Right() &&
            !Left())
            body.velocity = new Vector2(0, body.velocity.y);
    }
    void AirUpdate()
    {
        if (!input.GetButtonDown("up"))
            justJumped = false;
        if (input.GetButtonDown("up") && doubleJumpReady && !justJumped)
        {
            body.velocity += new Vector2(0, JumpVelocity().y);
            doubleJumpReady = false;
        }
        Debug.Log("In the air");
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
    public void AttackHit(Collider2D collision, int attackDamage, int attackStun)
    {
        Debug.Log("Hit " + collision.gameObject.name);
        if (collision.gameObject.name == "Player1")
        {
            manager.currentHP[0] -= attackDamage;
            collision.GetComponent<TestAnimInput>().Hurt(attackStun);
        }
        else if (collision.gameObject.name == "Player2")
        {
            manager.currentHP[1] -= attackDamage;
            collision.GetComponent<TestAnimInput>().Hurt(attackStun);
        }
    }

    bool Left()
    {
        return input.GetButtonDown("left") && !HurtingState;
    }
    bool Right()
    {
        return input.GetButtonDown("right") && !HurtingState;
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
        yield return new WaitForEndOfFrame();
        anim.SetBool(atName, false);
    }

    IEnumerator Hurt(int stunFrames)
    {
        if (Inverted())
            StartCoroutine(Hurt(stunFrames, new Vector2(-1, body.velocity.y)));
        else
            StartCoroutine(Hurt(stunFrames, new Vector2(1, body.velocity.y)));
        return null;
    }
    IEnumerator Hurt(int stunFrames, Vector2 hurtVelocity)
    {
        if (HurtingState)
            Debug.Log("Chain!");
        HurtingState = true;
        yield return new WaitForEndOfFrame();
        for(int i = 1; i < stunFrames; i++)
            yield return new WaitForEndOfFrame();
        HurtingState = false;
    }
}
