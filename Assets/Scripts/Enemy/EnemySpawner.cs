using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemyPrefab;

	public float spawnRate = 4f;
	public float distance;

	private bool canSpawn;

	private float spawnTimer;

	private Core core;

	// Use this for initialization (is called before Start())
	public void Awake()
	{
		spawnTimer = spawnRate;
	}

	// Use this for initialization 
	void Start()
	{
		// Gets reference to our core
		core = Core.instance;
	}

	// Is called every frame
	public void Update()
	{
		spawnTimer -= Time.deltaTime;

		// When the spawnTimer reaches 0 we instantiate an enemy prefab at a random location
		if(spawnTimer <= 0f)
		{
			Quaternion randomRotation = Random.rotation;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, randomRotation.eulerAngles.z));
			Vector3 spawnPos = transform.up * distance;
			spawnTimer = core.spawnRate;
			GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity) as GameObject;
		}
	}
}