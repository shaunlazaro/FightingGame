using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    public int attackDamage;
    public int attackStun;
    public int blockStun = 0;
    public float velocity;

    public int hitStopFrames;
    public int counter;

    public bool projectile = false; // If true, the gameobject will self destruct after hitstop ends.
    public bool throwAttack = false;
    public GameObject throwObject;

    public GameObject particlePoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            if (!throwAttack)
            {
                Time.timeScale = 0;

                collision.GetComponentInParent<TestAnimInput>().AttackHit(attackDamage, attackStun, blockStun, velocity);
                GameObject.Find("EventSystem").GetComponent<HitEffect>().SpawnEffect(3, particlePoint.transform.position);

                counter = hitStopFrames;
            }
            else
            {
                // Throw attack = true!
                throwObject = collision.gameObject;
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
