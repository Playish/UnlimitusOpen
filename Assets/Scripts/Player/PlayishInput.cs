using System;
using UnityEngine;
using System.Collections;

using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using Playish;

/**
 * Singleton handling the input from Playish unique to this game.
 */
public class PlayishInput : MonoBehaviour
{
	public Vector2 joystick = Vector2.zero;
	public Vector2 wheel = Vector2.zero;
	public bool buttona = false;

	public String playerDeviceId = "";

	DeviceManager deviceManager;
	// PlayishManager playishManager;

	protected Vector2 _primaryMovement = Vector2.zero;
	protected string _axisHorizontal;
	protected string _axisVertical;

	// This decides how much of the middle of the joystick works as a deadzone
	// This zone in other words will give out x and y as 0 as long as we're not going beyond 0.3 with our finger
	private const float deadzone = 0.3f;

	// Use this for initialization
	private void Awake()
	{
		Application.runInBackground = true;

		deviceManager = DeviceManager.getInstance();
	}

	// Is called every frame
	void Update()
	{
		// string[] deviceKeys = deviceManager.devices.Keys.ToArray();
		var device = deviceManager.getDevice(playerDeviceId);
		if(device == null || !device.hasController())
		{
			return;
		}

		if(playerDeviceId == "")
		{
			playerDeviceId = device.getDeviceId();
		}

		// buttona is set to an attack button
		// If you want to use this button you have to make a custom playish controller and name a button: attack
		// Simply check if buttona is true or false (buttona is true if pressed) if you want to do anything when you press the button
		buttona = device.getBoolInput("attack");

		if(device.getDeviceId() == playerDeviceId)
		{
			float x = device.getIntInput("joystickX") / 99f;
			float y = device.getIntInput("joystickY") / 99f;

			// Applies the deadzone
			if(x < deadzone && x > -deadzone){x = 0f;}
			if(y < deadzone && y > -deadzone){y = 0f;}

			joystick = new Vector2(x, y);
			Debug.Log("deviceId: " + playerDeviceId); // Is called and prints the device Id.
		}
	}
}