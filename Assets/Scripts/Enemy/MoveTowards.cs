using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
	public float speed; // Set the enemy speed in the inspector

	public void Start()
	{
		// Makes sure the enemy is looking at the core
		transform.LookAt(Core.instance.transform);
	}

	public void Update()
	{
		// Moves towards the core
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}
}