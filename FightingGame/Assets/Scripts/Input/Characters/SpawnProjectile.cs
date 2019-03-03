using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour {


    public GameObject projectile;

    public void Shoot(float velocity)
    {
        GameObject p;
        if (gameObject.GetComponent<TestAnimInput>().Inverted())
        {
            p = Instantiate(projectile,
                new Vector3(this.gameObject.GetComponent<Renderer>().bounds.min.x,
                gameObject.GetComponent<Renderer>().bounds.max.y),
                transform.rotation);

            p.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0);
        }
        else
        {
            p = Instantiate(projectile,
                new Vector3(this.gameObject.GetComponent<Renderer>().bounds.max.x,
                gameObject.GetComponent<Renderer>().bounds.max.y),
                transform.rotation);

            p.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0);
        }
        Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
    }

	
}
