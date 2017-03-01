using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject deathEffect;
	public GameObject attackEffect;

	public CircleCollider2D col2D;
		
	private ParticleSystem ps;

	private float deathTimer = 2f;

	private bool isDead = false;

	private Transform target;

	private Core core;

	void Start()
	{
		// Collects references to our components for further use
		ps = GetComponentInChildren<ParticleSystem>();
		col2D = GetComponent<CircleCollider2D>();
		core = Core.instance;
	}
	
	// Update is called once per frame
	void Update()
	{
		// If the object is called dead we count down to it's removal from the scene
		if(isDead)
		{
			deathTimer -= Time.deltaTime;
			if(deathTimer <= 0f)
			{
				Destroy(this.gameObject); // Destroys the object which this component is attached to
			}
		}

		// If the core is set to destroy all enemies. 
		if(core.destroyEnemies)
		{
			spawnDeathEffect();
			ps.Stop(); // Stops emiting particles

			isDead = true;
			col2D.enabled = false; // Disables the collider so that it can't do any damage while set to isDead
		}
	}

	// Is called when something triggers our collider
	void OnTriggerEnter2D(Collider2D col)
	{
		// If what triggered us is a player we stop the particle emition and call that the object dead
		if(col.CompareTag("Player") || col.CompareTag("Core"))
		{
			spawnDeathEffect();
			ps.Stop(); // Stops emiting particles
			
			isDead = true;
			col2D.enabled = false; // Disables the collider so that it can't do any damage while set to isDead
			core.killModifier += 0.05f;
			core.spawnRate -= 0.05f;
		}
	}

	// Spawns the death effect. 
	void spawnDeathEffect()
	{
		GameObject effect = (GameObject) Instantiate(deathEffect);
		Vector3 spawnPos =  new Vector3(transform.position.x, transform.position.y, transform.position.z);
		effect.transform.position = spawnPos;
		effect.transform.localEulerAngles = new Vector3(0, 0, 0);
	}

	// This will be called when the enemy triggers with the core. The effect will look like it's being absorbed by the core and therefore inflicting further damage
	void spawnAttackEffect()
	{
		GameObject effect = (GameObject) Instantiate(attackEffect);
		Vector3 spawnPos =  new Vector3(transform.position.x, transform.position.y, transform.position.z);
		effect.transform.position = spawnPos;
		effect.transform.localEulerAngles = new Vector3(0, 0, 0);
	}
}