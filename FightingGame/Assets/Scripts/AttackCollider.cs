using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    public int attackDamage;
    public int attackStun;
    public int blockStun = 0;
    public Vector2 velocity;

    public int hitStopFrames;
    public int counter;

    public bool projectile = false; // If true, the gameobject will self destruct after hitstop ends.
    public bool throwAttack = false;
    public bool knockDownAttack = false;
    public bool reStandAttack = false; // Untopples character on hit

    public GameObject particlePoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player") && !collision.gameObject.GetComponent<TestAnimInput>().invulnerable)
        {
            if (!throwAttack)
            {
                Time.timeScale = 0;

                collision.gameObject.GetComponent<TestAnimInput>().AttackHit(attackDamage,
                    attackStun, blockStun, velocity, knockDownAttack, reStandAttack);
                GameObject.Find("EventSystem").GetComponent<HitEffect>().SpawnEffect(3, particlePoint.transform.position);

                counter = hitStopFrames;
            }
            else
            {
                // Attacks beat throws, can't rethrow downed opponents
                if (collision.gameObject.GetComponent<TestAnimInput>().CanWalk)
                {
                    collision.gameObject.GetComponent<TestAnimInput>().AttackHit(attackDamage);
                    gameObject.GetComponentInParent<TestAnimInput>().ThrowAttack(collision.gameObject);
                }
            }
        }
        if(collision.gameObject.name.Contains("Border"))
        {
            if(projectile)
                Destroy(gameObject);
        }
    }
    

    void Update()
    {
        if (Time.timeScale == 0 && counter == 0)
        {
            Time.timeScale = 1;
            if (projectile) Destroy(gameObject);
        }
        else counter--;


    }
}
