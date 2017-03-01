using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playish;

public class PlayerDriver : MonoBehaviour
{
	public bool debugMode;

	// An array containing as many different colors you need
	public Color[] playerColors;

	protected Rigidbody2D rb;
	protected PlayishInput playishInput;
	protected ParticleSystem playerParticleSystem;

	public float maxSpeed;
	public float currentSpeed;
	public float throttleSpeed;

	private Core core;

	// Use this for initialization (is called before Start())
	void Awake()
	{
		// Stores our references for further use
		rb = GetComponent<Rigidbody2D>();
		playishInput = GetComponent<PlayishInput>();
		playerParticleSystem = GetComponentInChildren<ParticleSystem>();

		// Makes sure every new player has a different color on it's particle system
		playerParticleSystem.startColor = playerColors[Random.Range(0, playerColors.Length)];
	}

	// Use this for initialization
	void Start()
	{
		// Gets a reference to the core
		core = Core.instance;
		// Makes sure that every player connected decreases the time after each enemy spawned
		core.spawnRate -= 0.2f;
	}

	public virtual void FixedUpdate()
	{
		PlayerInput();

		// Stores the player velocity in the currentSpeed variable each fixed update
		currentSpeed = rb.velocity.magnitude;
	}

	// Handles input and movement
	public virtual void PlayerInput()
	{
		if(playishInput != null)
		{
			if(playishInput.playerDeviceId != "")
			{
				// Adds force horizontally when using the joystick
				if(playishInput.joystick.x != 0f)
				{
					rb.AddForce(transform.right * (throttleSpeed * playishInput.joystick.x) * Time.deltaTime);
				}

				// Adds force vertically when using the joystick
				if(playishInput.joystick.y != 0f)
				{
					rb.AddForce(transform.up * (throttleSpeed * (playishInput.joystick.y * -1f)) * Time.deltaTime);
				}
			}

			// Enables keyboard input if debug mode is set to true in the inspector
			if(debugMode == true)
			{
				if(Input.GetAxis("Vertical") != 0)
				{
					if(currentSpeed <= maxSpeed)
					{
						// Gets the positive or negative input so that we will add force towards the correct direction
						float direction = Input.GetAxis("Vertical");
						// Adds force based on our throttleSpeed and direction we want to go
						rb.AddForce(transform.up * (throttleSpeed * direction) * Time.fixedDeltaTime);
					}
				}

				if(Input.GetAxis("Horizontal") != 0)
				{
					if(currentSpeed <= maxSpeed)
					{
						// Gets the positive or negative input so that we will add force towards the correct direction
						float direction = Input.GetAxis("Horizontal");
						// Adds force based on our throttleSpeed and direction we want to go
						rb.AddForce(transform.right * (throttleSpeed * direction) * Time.fixedDeltaTime);
					}
				}
			}
		}
	}
}