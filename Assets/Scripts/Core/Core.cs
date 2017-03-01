using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
	public static Core instance;

	[Header("Effects")]
	public ParticleSystem stageOne;
	public ParticleSystem stageTwo;
	public ParticleSystem stageThree;
	public ParticleSystem stageFour;
	public ParticleSystem stageFive;

	[Header("General Settings")]
	public int hitPoints;
	// GameObject containing a particle effect played when dying
	public GameObject hitEffect;
	// Set reference in the inpsector to the fade to black animation
	public Animator fadeToBlack;

	private CircleCollider2D col2D;
	// Time until the core dies after death has been triggered
	public float deathTimer = 5f;

	private float deathTimerReset;
	// Have the core started dying?
	private bool deathTriggered;
	// Stores the initial hitPoints value
	private int hitPointsReset;

	private bool resetModifier = true;

	private bool isDead = false;
	public bool destroyEnemies = false;

	public float spawnRate;
	public float killModifier;

	// Called when the component is initialized and enabled. Only happens before the first frame.
	void Awake()
	{
		instance = this;
		hitPointsReset = hitPoints;
		deathTimerReset = deathTimer;
		col2D = GetComponent<CircleCollider2D>();
	}

	void Start()
	{
		// Sets the core to stage one at the start of the game
		StageOne();
	}

	void Update()
	{
		// If we have 4 hitpoints we set our core to stage one
		if(hitPoints == 4)
		{
			StageOne();
		}

		// If we have 3 hitpoints we set our core to stage two
		if(hitPoints == 3)
		{
			StageTwo();
		}

		// If we have 2 hitpoints we set our core to stage three
		if(hitPoints == 2)
		{
			StageThree();
		}

		// If we have 1 hitpoint we set our to stage four
		if(hitPoints == 1)
		{
			StageFour();
		}

		// If we have 0 hitpoints we set our core to stage five
		if(hitPoints == 0)
		{
			StageFive();
		}

		// When the core is dead we count down to the level reset and disable the core collider
		if(isDead)
		{
			deathTimer -= Time.deltaTime;

			col2D.enabled = false;

			// When the timer reaches 0 we stop the stage five particle effect making any trace of the core disappear
			if(deathTimer <= 0f)
			{
				stageFive.Stop();

				// 5 seconds after the core disappears we fade the screen to black
				if(deathTimer <= -5f)
				{
					fadeToBlack.Play("Fade");
				}

				// 2 seconds later we destroy all currently alive enemies, reset the death timer, set the core to stage one again and set the core be alive again (isDead = false)
				if(deathTimer <= -7f)
				{
					deathTimer = deathTimerReset;
					StageOne();
					isDead = false;
					destroyEnemies = true;

					// We reset the spawn rate
					if(resetModifier)
					{
						spawnRate += killModifier;
						resetModifier = false;
					}
				}
			}
		}

		// Makes sure enemies doesn't spawn more often than once per second
		if(spawnRate < 1f)
		{
			spawnRate = 1f;
		}
	}

	// Is triggered when another object enters the cores trigger
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the entered gameobject is tagged an enemy.
		if(col.CompareTag("Enemy"))
		{
			hitPoints -= 1; // We remove one hit point

			// Spawns in the hitEffect so the players can see more clearly that the core took damage
			GameObject effect = (GameObject) Instantiate(hitEffect);
			Vector3 spawnPos =  new Vector3(transform.position.x, transform.position.y, transform.position.z);
			effect.transform.position = spawnPos;
			effect.transform.localEulerAngles = new Vector3(0, 0, 0);
		}
	}

	// Sets the core to stage one
	void StageOne()
	{
		hitPoints = hitPointsReset; // We set the core hitpoints to what it previously started with
		isDead = false; // Makes sure we're not set as isDead
		destroyEnemies = false; // Prevents further enemes to destroy themselves
		resetModifier = true; // Makes sure we reset enemy spawn rate modifiers
		col2D.enabled = true; // Enables the core collider

		if(stageOne.isStopped)
		{
			stageOne.Play();
		}

		if(stageFive.isPlaying)
		{
			stageFive.Stop();
		}
	}

	// Sets the core to stage two
	void StageTwo()
	{
		if(stageOne.isPlaying)
		{
			stageOne.Stop();
		}

		if(stageTwo.isStopped)
		{
			stageTwo.Play();
		}
	}

	// Sets the core to stage three
	void StageThree()
	{
		if(stageTwo.isPlaying)
		{
			stageTwo.Stop();
		}

		if(stageThree.isStopped)
		{
			stageThree.Play();
		}
	}

	// Sets the core to stage four
	void StageFour()
	{
		if(stageThree.isPlaying)
		{
			stageThree.Stop();
		}

		if(stageFour.isStopped)
		{
			stageFour.Play();
		}
	}

	// Sets the core to its last stage. Stage five
	void StageFive()
	{
		if(stageFour.isPlaying)
		{
			stageFour.Stop();
		}

		if(stageFive.isStopped)
		{
			stageFive.Play();
		}

		// Since stage five is the cores death stage we set isDead to true
		isDead = true; 
	}
}