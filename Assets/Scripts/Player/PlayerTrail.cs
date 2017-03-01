using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
	// Set reference in the inspector to the parent particle system
	public ParticleSystem parentSystem;

	private ParticleSystem ownSystem;

	private Color parentColor;

	// Use this for initialization (is called before Start())
	void Awake()
	{
		ownSystem = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start()
	{
		// Stores the parent particle color
		parentColor = parentSystem.startColor;
		// Sets own particle color to that same color
		ownSystem.startColor = parentColor;
	}
}