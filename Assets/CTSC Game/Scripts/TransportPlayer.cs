using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/**
 * Tranportation Manager. 
 * When the given collider is hit, the player will
 * transport to the given scene location input as
 * a string of the name of the location in the scene.
 * 
 * Input must have an "Transport" axis, with the default value
 * being "e" to transport the player.
 * 
 * @author Shaun Howard
 */
[System.Serializable]
public class TransportPlayer : MonoBehaviour {
	
	public String location;
	public Transform playerStart;
	public float yawOnStart;
	public String currentLocation;
	public bool start;
	
	Vector3 currPlayerPosition;
	Quaternion currPlayerRotation;
	GameObject player, locations;
	EventHandler eventHandler;
	Vector3 targetPostition;
	bool loadNewScene = false;
	EventHandler.GameLocation transportLocation;
	GameObject interactionManager;
	GUIHandler guiHandler;
	
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		eventHandler = GameObject.Find("EventHandler").GetComponent<EventHandler>();
		currPlayerPosition = new Vector3 ();
		currPlayerRotation = new Quaternion ();
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnTriggerStay(Collider col){

		if(col.gameObject.tag.Equals("Player") &&
		   col.GetComponent<CharacterOurs>().canEnter &&
		   Input.GetKeyDown(KeyCode.E) && !start){
			//turn off all interaction gui, can enable this if desired
			guiHandler.reset();

			//Handles fade out.
			setTransportState();
			//Debug.Log("Into " + location);
			player = col.gameObject;

			//Store the current player position.
			currPlayerPosition = player.transform.position;
			currPlayerRotation = player.transform.rotation;

			//Transports player to given location.
			Transport();
		}
	}

	public void TransportNow(GameObject player){
		guiHandler.reset ();
		setTransportState ();
		this.player = player;
		currPlayerPosition = player.transform.position;
		currPlayerRotation = player.transform.rotation;
		Transport ();
	}

	void setTransportState(){
		eventHandler.setGameState (EventHandler.GameState.TRANSPORT);
	}

	void setPlayState(){
		eventHandler.setGameState (EventHandler.GameState.PLAY);
	}

	void HandleDestroy(){
		if (loadNewScene) {
			Destroy(gameObject);
		}

	}

	/**
	 * Transports the player to the location set 
	 * in the inspector for this door.
	 * Will disable the other locations and 
	 * enable the desired location of transport.
	 */
	void Transport(){
		eventHandler.Transport (true);
		EnableTransportLocation ();
		//MovePlayerToPosition never happens for some reason.
		StartCoroutine (waitForSecs (5F, MovePlayerToPosition));
	//	eventHandler.Transport (false);
    }

	void MovePlayerToPosition(){
		if (eventHandler.switchLocation (location).Equals (EventHandler.GameLocation.DUNGEON) &&
			eventHandler.switchLocation (currentLocation).Equals (EventHandler.GameLocation.DUNGEON)) {
			Vector3 transportLocation = playerStart.transform.position;
			player.transform.position = transportLocation;
		}
		else if (eventHandler.switchLocation (location).Equals (EventHandler.GameLocation.DUNGEON)){
			Vector3 transportLocation = GetPlayerStartPosition ();
			//Debug.Log(transportLocation.ToString());
			player.transform.position = transportLocation;
		} else if (eventHandler.switchLocation(location).Equals(EventHandler.GameLocation.TDC)
		           && eventHandler.switchLocation(currentLocation).Equals(EventHandler.GameLocation.DUNGEON)){
			Vector3 transportLocation = GetPlayerStartPosition ();
			//Debug.Log(transportLocation.ToString());
			player.transform.position = transportLocation;
		} else if (eventHandler.switchLocation(location).Equals(EventHandler.GameLocation.FOREST)
		           && eventHandler.switchLocation(currentLocation).Equals(EventHandler.GameLocation.HEALING_CAVE)){
			Vector3 transportLocation = GetPlayerStartPosition ();
			//Debug.Log(transportLocation.ToString());
			player.transform.position = transportLocation;
		}else if (playerStart != null) {
			Vector3 transportLocation = playerStart.transform.position;
			player.transform.position = transportLocation;
		} else {
			Debug.Log ("Failed to move player to correct position.");
		}
		Rotate ();
	}

	IEnumerator waitForSecs(float seconds, Action callAfter){
		yield return new WaitForSeconds (seconds);
		callAfter ();
	}

	Vector3 GetPlayerStartPosition(){
		loadNewScene = true;
		return eventHandler.FindPlayerStartPosition (location);
	}
	
	/**
	 * Rotates the player to the rotation set in the inspector.
	 */
	void Rotate(){
		player.GetComponent<PerfectController>().yaw = yawOnStart;
		setPlayState ();
		HandleDestroy ();
	}

	/**
	 * Enables the given transport location
	 * and disables the others in the game.
	 * Also sets the current level for the save game manager
	 * to load on a given save.
	 */
	void EnableTransportLocation(){
		eventHandler.EnableLocation(eventHandler.switchLocation(location));
    }
}
