using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitEffect : MonoBehaviour {

    public ParticleSystem effect; // Insert blank prefab

    public void SpawnEffect(float lifeTime, Vector2 spawnPosition)
    {
        ParticleSystem ps = Instantiate(effect, spawnPosition, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, lifeTime);
    }

	// Use this for initialization
	void Start () {
		
	}
}
