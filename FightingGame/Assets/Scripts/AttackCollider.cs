using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    public int attackDamage;
    public int attackStun;

	void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<TestAnimInput>().AttackHit(collision, attackDamage, attackStun);
    }
}
