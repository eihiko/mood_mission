using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//// The location the player will transfer to upon collision with door.
//public enum Location {City, Forest, OneStory_House, TwoStory_House, Library,
//	Bedroom, Small_Dungeon, Large_Dungeon}

/**
 * Door collision Manager. 
 * When the door collider is hit, the player will
 * transport to the given scene location input as
 * an enumerated type.
 * 
 * Choices for location are: 
 * City, Forest, OneStory_House, TwoStory_House, Library,
 * Bedroom, Small_Dungeon, Large_Dungeon
 * 
 * Input must have an "OpenDoor" axis, with the default value
 * being "e" to open a door.
 * 
 * @author Shaun Howard
 */
public class DoorCollision : MonoBehaviour {
	
	Vector3 currPlayerPosition;
	Quaternion currPlayerRotation;
	GameObject player, locations;
//	public Location location; 
	public String location;
	public Transform playerStart;

	//The set of locations to transport to.
	Dictionary<String, Transform> locationSet = new Dictionary<String, Transform>();
	GameObject transportLocation;

	// Use this for initialization
	void Start () {

		locations = GameObject.Find ("Locations");

		//Map all locations to their name in locations map.
		foreach (Transform location in locations.transform) {
			locationSet.Add (location.gameObject.name, location);
			Debug.Log(location.gameObject.name);
		}

		currPlayerPosition = new Vector3 ();
		currPlayerRotation = new Quaternion ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnTriggerStay(Collider col){
		Debug.Log("Entered trigger");
		if(col.gameObject.tag.Equals("Player") && Input.GetAxis("Transport") > 0){
			Debug.Log("Into transport area");
			player = col.gameObject;

			//Store the current player position.
			currPlayerPosition = player.GetComponent<Rigidbody>().transform.position;
			currPlayerRotation = player.GetComponent<Rigidbody>().transform.rotation;

			//Plays the door animation.
			//transform.FindChild("door").SendMessage("DoorCheck"); 

			//Transports player to given location.
			Transport(player);
		}
	}

	/**
	 * Transports the player to the location set 
	 * in the inspector for this door.
	 * Will disable the other locations and 
	 * enable the desired location of transport.
	 * 
	 * @param player - the player to transport
	 */
	void Transport(GameObject player){
		EnableTransportLocation ();
		Vector3 transportLocation = playerStart.transform.position;
		player.transform.position = transportLocation;
    }

	/**
	 * Enables the given transport location
	 * and disables the others in the game.
	 */
	void EnableTransportLocation(){
		GameObject currLocation;
		foreach (KeyValuePair<String, Transform> kvp in locationSet){
			currLocation = kvp.Value.gameObject;
			if (kvp.Key.Equals(location)){
				currLocation.SetActive(true);
			} else {
				currLocation.SetActive(false);
			}
		}
		
//		switch(location){
//			case "City":
//				opStatus = true;
//				break;
//			case "Forest":
//				opStatus = true;
//				break;
//			case "OneStory_House":
//				opStatus = true;
//				break;
//			case "TwoStory_House":
//				opStatus = true;
//				break;
//			case "Library":
//				opStatus = true;
//				break;
//			case "Small_Dungeon":
//				opStatus = true;
//				break;
//			case "Large_Dungeon":
//				opStatus = true;
//				break;
//			case "Bedroom":
//				opStatus = true;
//				break;
//	     }
    }
}
