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

    public GameObject particlePoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player")) { 
            Time.timeScale = 0;

            gameObject.GetComponentInParent<TestAnimInput>().AttackHit(collision, attackDamage, attackStun, blockStun, velocity);
            GameObject.Find("EventSystem").GetComponent<HitEffect>().SpawnEffect(3, particlePoint.transform.position);

            counter = hitStopFrames;
        }
    }
    
    void Update()
    {
        if (Time.timeScale == 0 && counter == 0) Time.timeScale = 1;
        else counter--;

    }
}
