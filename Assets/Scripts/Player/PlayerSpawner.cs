using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Playish;
using System;

public class PlayerSpawner : MonoBehaviour
{
	public static PlayerSpawner instance;

	public int maxPlayers;

	// Boat prefab
	public GameObject playerPrefab;
	public GameObject[] spawnPoints;

	// Playish references
	private DeviceManager deviceManager;
	private PlayishManager playishManager;

	// Players (Build with both got the Parameter name: Dictionary null error
	private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>(); // Build with this commented out got the Dictionary null error
	private Dictionary<string, PlayerInfo> playersInfo = new Dictionary<string, PlayerInfo>(); // Build with this commented out got the Dictionary null error
	private int lastPlayerNumber = 0;

	// Use this for initialization (is called before Start())
	void Awake()
	{
		//Make sure there is only on instance of the PlayerSpawner in the scene.
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
			name = "PlayerSpawner";
			if(transform.parent != null)
			{
				transform.SetParent(null);
			}
		}
		else
		{
			DestroyImmediate(this);
		}
	}

	// Use this for initialization
	void Start()
	{
		// Get references for further use
		deviceManager = DeviceManager.getInstance();
		playishManager = PlayishManager.getInstance();

		// Sync existing devices
		syncExistingDevices();

		// Set events
		deviceManager.deviceAddedEvent += onDeviceAdded;
		deviceManager.deviceRemovedEvent += onDeviceRemoved;
		deviceManager.deviceChangedEvent += onDevicesChanged;

		playishManager.playishPauseEvent += onBrowserPaused;
		playishManager.playishResumeEvent += onBrowserResumed;

		// Settings
		Application.runInBackground = true;
		DontDestroyOnLoad(transform.gameObject);
	}

	private void onDeviceAdded(DeviceManager.DeviceEventArgs e)
	{
		if(lastPlayerNumber < maxPlayers)
		{
			spawnPlayer(e.deviceId);
		}
	}

	private void onDeviceRemoved(DeviceManager.DeviceEventArgs e)
	{
		removePlayer(e.deviceId);
	}

	private void onDevicesChanged(DeviceManager.DeviceEventArgs e)
	{}

	private void syncExistingDevices()
	{
		var keys = deviceManager.devices.Keys.ToArray();

		for(int i = 0; i < keys.Length; i++)
		{
			var item = deviceManager.devices[keys [i]];
			spawnPlayer(item.getDeviceId());
		}
	}


	// ---- MARK: Browser pause events

	private void onBrowserPaused(EventArgs e)
	{
		//pausedText.spawnUIParts (ref canvas, ref font);
	}

	private void onBrowserResumed(EventArgs e)
	{
		// Keep in mind that your game has to be able to resume on it's own regardless
		// of this event. At the moment there are no actions on the website that triggers this.
		//pausedText.removeUIParts ();
	}

	// ---- MARK: Player management

	private void spawnPlayer(string playerDeviceId)
	{
		PlayerInfo playerInfo = null;
		if (playersInfo.ContainsKey(playerDeviceId))
		{
			playerInfo = playersInfo[playerDeviceId];
		}
		else
		{
			playerInfo = new PlayerInfo(lastPlayerNumber);
			playersInfo.Add(playerDeviceId, playerInfo);
		}
		int spawnPos = UnityEngine.Random.Range(0, spawnPoints.Length);

		var position = new Vector3(spawnPoints[spawnPos].transform.position.x, 
			spawnPoints[spawnPos].transform.position.y, 
			spawnPoints[spawnPos].transform.position.z);

		// Spawn and place new player
		var newPlayer = Instantiate<GameObject>(playerPrefab);
		newPlayer.transform.SetParent(transform);
		newPlayer.transform.position = position;

		var playerPlayishScript = newPlayer.GetComponent<PlayishInput>();
		// Sets playerDeviceId
		if(playerPlayishScript != null)
		{
			playerPlayishScript.playerDeviceId = playerDeviceId;
		}

		players.Add(playerDeviceId, newPlayer);
		lastPlayerNumber += 1;
	}

	private void removePlayer(string playerDeviceId)
	{
		if(players.ContainsKey(playerDeviceId))
		{
			var player = players[playerDeviceId];
			Destroy(player);
			players.Remove(playerDeviceId);
		}

		if(playersInfo.ContainsKey(playerDeviceId))
		{
			var playerInfo = playersInfo[playerDeviceId];
		}
	}

	private class PlayerInfo
	{
		public int playerNumber = -1;

		public PlayerInfo(int playerNumber)
		{
			this.playerNumber = playerNumber;
		}
	}
}