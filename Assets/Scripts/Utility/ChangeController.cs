using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playish;

// ----- INFO ----- //
// In order to have the playish app display your controller you need to have the game tell the app the name of your controller.
// In the playish manager you can call changeController() with a string to fetch your controller. This is the controller you tailor and name in the playish controller editor. 
// You could in practice also change the controller dynamically. You are not restricted to have one controller throughout your game. Just call the function again with a new name. 
// ----- END OF INFO ----- //

// Always have this component somewhere in your scene if you want the playish app to update the controller
public class ChangeController : MonoBehaviour
{
	PlayishManager playish;

	public string nameOfController; // Type your name of your controller in the unity inspector

	void Start()
	{
		playish = PlayishManager.getInstance(); 	// Gets the playishmanager instance in your scene
		playish.changeController(nameOfController); // This function tells the playishManager what name your custom controller has and then fetches it for the app
	}
}