using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/**
 * Handles game events like transitioning between
 * states of the game and controls.
 * 
 * @author Shaun Howard
 */
public class EventHandler: MonoBehaviour
{
	private static readonly string DungeonLocation = "/TransportToDungeon";
	private Color skyboxColor;
	
	public enum GameState
	{
		START,
		TRANSPORT,
		LOAD,
		SAVE,
		PLAY,
		PAUSE,
		GUI
	}

	public enum GameLocation
	{
		TDC, DUNGEON, HEALING_CAVE, FOREST, L_BASEMENT, 
		S_BASEMENT_1, S_BASEMENT_2, S_BASEMENT_3,
		COB_WEB_BASEMENT, ONE_ROOM_HOUSE_1,
		ONE_ROOM_HOUSE_2, ONE_ROOM_HOUSE_3, ONE_ROOM_HOUSE_4,
		SMALL_HOUSE_1, SMALL_HOUSE_2, TALL_HOUSE_1, TALL_HOUSE_2,
		TALL_HOUSE_3, TWO_STORY_HOUSE_1, TWO_STORY_HOUSE_2,
		UNKNOWN
	}

	public bool useUI = false;
	public GameObject[] uiList;
	public MissionManager missionManager;
	public float yawOnStart;
	public static EventHandler current;
	public GameObject saveManagerObject;
	public GameObject player;
	public GameObject playerCamera;
	public GameObject locations;
	public GameObject loginMenu;
	CurrentLevel currLevel;
	public float loadTime = 1f;
	
	//The set of locations to transport to.
	Dictionary<GameLocation, Transform> locationSet;
	
	PlayerScreenFade playerScreenFade;
	PerfectController controller;
	CharacterOurs playerChar;
	PauseMenu pauseMenu;
	
	private bool firstLoad = true;
	private GameState currState, lastState;
	private GameLocation currLocation, lastLocation;
	private int lastUIPlayed = 0;
	
	// Keeps the manager alive throughout the game
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
//		startLocation.activeSelf = true;
	}

	// Use this for initialization
	void Start()
	{
		if (useUI) {
			setGameState (GameState.GUI);
			missionManager.playUI (uiList[lastUIPlayed].name);
			lastUIPlayed++;
		}
		controller = player.GetComponent<PerfectController> ();
		controller.yaw = yawOnStart;
		currLevel = player.GetComponent<CurrentLevel>();
		lastLocation = currLevel.getCurrLocation();
		locationSet = new Dictionary<GameLocation, Transform>();

		//Map all locations to their enum in locations map.
		foreach (Transform location in locations.transform) {
			if (location.gameObject.name.Equals("TopDownInteriors")){
				//Debug.Log("In TDI");
				foreach (Transform room in location){
					if (!room.gameObject.name.Equals("Transports_TDI")){
						GameLocation enumLoc = switchLocation(room.gameObject.name);
						locationSet.Add(enumLoc, room);
						//Debug.Log(room.gameObject.name);
						if (room.gameObject.activeSelf && firstLoad) {
							currLevel.setCurrLocation(enumLoc);
						}
					}
				}
			} else {
				GameLocation enumLoc = switchLocation(location.gameObject.name);
				locationSet.Add(enumLoc, location);
				//Debug.Log(location.gameObject.name);
				if (location.gameObject.activeSelf && firstLoad) {
					currLevel.setCurrLocation(enumLoc);
				}
			}
		}
		
//		Screen.showCursor = true;
//		Screen.lockCursor = true;
		pauseMenu = saveManagerObject.GetComponent<PauseMenu>();
		playerScreenFade = playerCamera.GetComponent<PlayerScreenFade>();
		controller = player.GetComponent<PerfectController>();
		playerChar = player.GetComponent<CharacterOurs>();
		currState = GameState.START;
		lastState = GameState.START;
		firstLoad = false;
		if (!useUI) {
			StartGame ();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		//Determine the currently loaded location.
		//currLocation = currLevel.getCurrLocation();
//		if (lastLocation != currLocation) {
//			//EnableLocation(currLocation);
//			lastLocation = currLocation;
//		}
		
		//Pause/unpause the game.
		if (Input.GetKeyUp(KeyCode.Escape) && currState != GameState.PAUSE) {
			pauseMenu.setPaused(true);
			Time.timeScale = 0;
			currState = GameState.PAUSE;
		} else if (Input.GetKeyUp(KeyCode.Escape) && currState == GameState.PAUSE) {
			pauseMenu.setPaused(false);
			Time.timeScale = 1;
			currState = GameState.PLAY;
		}

//		//Transport the player.
//		if (Input.GetKeyDown("E")) {
//			currState = GameState.TRANSPORT;
//			Debug.Log("Transport key pressed.");
//		}
		
		updateGame(currState);
	}
	
	public void updateGame(GameState state)
	{
		switch (currState) {
			case GameState.START:
//				Start();
				lastState = GameState.START;
				break;
			case GameState.LOAD:
//				Load();
				lastState = GameState.LOAD;
				break;
			case GameState.TRANSPORT:
				//Transport();
				lastState = GameState.TRANSPORT;
				break;
			case GameState.PLAY:
				Play();
				lastState = GameState.PLAY;
				break;
			case GameState.PAUSE:
				Pause();
				lastState = GameState.PAUSE;
				break;
			case GameState.GUI:
				Gui();
				lastState = GameState.GUI;
				break;
		}
	}

	IEnumerator waitForSecs(float seconds){
		yield return new WaitForSeconds (seconds);
	}

	public void EnableLocation(GameLocation nextLocation)
	{
		if (nextLocation == GameLocation.DUNGEON) {
			Application.LoadLevel (1);
			lastLocation = currLocation;
			currLocation = nextLocation;
			currLevel.setCurrLocation (nextLocation);
			playerCamera.GetComponent<Skybox> ().enabled = false;
		} else if (currLocation == GameLocation.DUNGEON && nextLocation == GameLocation.TDC) {
			Application.LoadLevel (0);
			lastLocation = currLocation;
			currLocation = nextLocation;
			currLevel.setCurrLocation (nextLocation);
			playerCamera.GetComponent<Skybox> ().enabled = true;
		} 
		//------Note------
		//This code correctly loads the forest AND city, but throws a lot of Missing Reference Exceptions.
		//Also the fade back in never happens and must be fixed by going into the Pause menu briefly.

		//else if (nextLocation == GameLocation.FOREST || nextLocation == GameLocation.TDC) {
			//Application.LoadLevel (0);
			//lastLocation = currLocation;
			//currLocation = nextLocation;
			//currLevel.setCurrLocation (nextLocation);
			//playerCamera.GetComponent<Skybox> ().enabled = true;
		//}
		//This code does not load the city when exiting a house, but doesn't have the other problems.
			else if (currLocation != GameLocation.DUNGEON){
			GameObject nextLocationObj;
			foreach (KeyValuePair<GameLocation, Transform> kvp in locationSet) {
				nextLocationObj = kvp.Value.gameObject;

				if (kvp.Key == nextLocation) {
					if (kvp.Value.parent.name.Equals("TopDownInteriors")){
						kvp.Value.parent.gameObject.SetActive(true);
						kvp.Value.parent.Find("Transports_TDI").gameObject.SetActive(true);
						//Debug.Log("Set TDI parent to active.");
						playerCamera.GetComponent<Skybox>().enabled = false;
					}
					else {
						playerCamera.GetComponent<Skybox>().enabled = true;
					}
					//Debug.Log("Set "  + nextLocationObj.name + " to active.");
					nextLocationObj.SetActive(true);
					lastLocation = currLocation;
					currLocation = nextLocation;
					currLevel.setCurrLocation(nextLocation);
				} else {
					//Getting rid of this line makes nothing disappear ever, which means everything loads right,
					//but there are also rocks in houses and you can see the house interiors in the distance from the city.
					nextLocationObj.SetActive(false);
				}
			}
		}
	}

	/**
	 * Starts a new game.
	 */
	public void StartNewGame(){
		player.SetActive(true);
		loginMenu.SetActive(false);
		missionManager.playUI (uiList[lastUIPlayed].name);
		lastUIPlayed++;
	}

	public void Gui(){
		Time.timeScale = 0.0f;
	}

	/**
	 * Assumes the game is already loaded
	 * then disables the menu ui and enables
	 * the player to begin gameplay.
	 */
	public void StartGame(){
		player.SetActive(true);
		currState = GameState.PLAY;
		Cursor.visible = false;
		Screen.lockCursor = true;
		loginMenu.SetActive(false);
	}
	
	/**
	 * Loads the serialized game data,
	 * then starts the game.
	 */
	public void LoadGame(string serializedData)
	{
		currState = GameState.LOAD;
		LevelSerializer.LoadNow (serializedData);
		StartGame ();
	}
	
	/**
	 * Handles the transport of the player.
	 * @param fadeOut - fading in or out?
	 */
	public void Transport(bool fadeOut)
	{ 
		if (fadeOut) {
		   StartCoroutine ("Fade");
	    } else {
		   StopAllCoroutines();
		}
	}

	/**
	 * Fades the player camera to black, waits 
	 * for the given load time, and fades back
	 * to play.
	 */
	IEnumerator Fade()
	{
		//Debug.Log("in fade coroutine");
		playerScreenFade.Fade(false, .001f);
		while (currState == GameState.TRANSPORT){
			yield return null;
		}
		playerScreenFade.Fade(true, .001f);
		DestroyObject(GameObject.Find("Screen Fade"));
		yield break;
	}
	
	/**
	 * Handles play tasks in the game.
	 */
	void Play()
	{
		if (player.activeSelf){
		    controller.UpdatePlayer(currState);
		}
	}
	
	/**
	 * Handles pause tasks in the game.
	 */
	void Pause()
	{
		Time.timeScale = 0;
	}
	
	/**
	 * Loads the scene with the specified name.
	 * @param scene - the scene name to load
	 */
	void LoadScene(string scene)
	{
		Application.LoadLevel(scene);
	}
	
	/**
	 * Sets the state of the game.
	 * 
	 * @param state - the state to put the game in
	 */
	public void setGameState(GameState state)
	{
		currState = state;
	}
	
	public void rotatePlayer(Vector3 angle)
	{
		playerCamera.transform.localRotation.Set(angle.x, angle.y, angle.z, playerCamera.transform.rotation.w);
	}
	
	public void reinstantiatePlayer(Vector3 position, Quaternion rotation)
	{
		Instantiate(player, position, rotation);
	}
	
	/**
	 * Gets the enum of the location given.
	 * Returns TDC by default.
	 * @param location - the location to get the enum of
	 * @return the enum of the given location
	 */
	public GameLocation switchLocation(string location)
	{
		switch (location) {
			case "Forest":
				return EventHandler.GameLocation.FOREST;
			case "TopDownCity":
				return EventHandler.GameLocation.TDC;
			case "Dungeon":
				return EventHandler.GameLocation.DUNGEON;
			case "HealingCave":
				return EventHandler.GameLocation.HEALING_CAVE;
			case "LargeBasement":
				return EventHandler.GameLocation.L_BASEMENT;
			case "CobWebBasement":
				return EventHandler.GameLocation.COB_WEB_BASEMENT;
			case "SmallBasement1":
				return EventHandler.GameLocation.S_BASEMENT_1;
			case "SmallBasement2":
				return EventHandler.GameLocation.S_BASEMENT_2;
			case "SmallBasement3":
				return EventHandler.GameLocation.S_BASEMENT_3;
			case "TallHouse1":
				return EventHandler.GameLocation.TALL_HOUSE_1;
			case "TallHouse2":
				return EventHandler.GameLocation.TALL_HOUSE_2;
			case "TallHouse3":
				return EventHandler.GameLocation.TALL_HOUSE_3;
			case "OneRoomHouse1":
				return EventHandler.GameLocation.ONE_ROOM_HOUSE_1;
			case "OneRoomHouse2":
				return EventHandler.GameLocation.ONE_ROOM_HOUSE_2;
			case "OneRoomHouse3":
				return EventHandler.GameLocation.ONE_ROOM_HOUSE_3;
			case "OneRoomHouse4":
				return EventHandler.GameLocation.ONE_ROOM_HOUSE_4;
			case "SmallHouse1":
				return EventHandler.GameLocation.SMALL_HOUSE_1;
			case "SmallHouse2":
				return EventHandler.GameLocation.SMALL_HOUSE_2;
			case "TwoStoryHouse1":
				return EventHandler.GameLocation.TWO_STORY_HOUSE_1;
			case "TwoStoryHouse2":
				return EventHandler.GameLocation.TWO_STORY_HOUSE_2;
		}

		return EventHandler.GameLocation.UNKNOWN;
	}

	public Vector3 FindPlayerStartPosition(string location){
		Vector3 playerStartPosition = new Vector3 (0, 0, 0);
		switch (location) {
			case "Dungeon":
				playerStartPosition = GameObject.Find("TransportToTDC").transform.position;
				break;
			case "TopDownCity":
				playerStartPosition = GameObject.Find("TransportToDungeon").transform.position;
				break;
			case "HealingCave":
				playerStartPosition = GameObject.Find ("HealingCaveEntrance").transform.position;
				break;
			case "Forest":
			playerStartPosition = GameObject.Find("TransportToHealingCave").transform.position;
			break;
//			case "Dungeon":
//				return EventHandler.GameLocation.DUNGEON;

			//	return EventHandler.GameLocation.HEALING_CAVE;
//			case "LargeBasement":
//				return EventHandler.GameLocation.L_BASEMENT;
//			case "CobWebBasement":
//				return EventHandler.GameLocation.COB_WEB_BASEMENT;
//			case "SmallBasement1":
//				return EventHandler.GameLocation.S_BASEMENT_1;
//			case "SmallBasement2":
//				return EventHandler.GameLocation.S_BASEMENT_2;
//			case "SmallBasement3":
//				return EventHandler.GameLocation.S_BASEMENT_3;
//			case "TallHouse1":
//				return EventHandler.GameLocation.TALL_HOUSE_1;
//			case "TallHouse2":
//				return EventHandler.GameLocation.TALL_HOUSE_2;
//			case "TallHouse3":
//				return EventHandler.GameLocation.TALL_HOUSE_3;
//			case "OneRoomHouse1":
//				return EventHandler.GameLocation.ONE_ROOM_HOUSE_1;
//			case "OneRoomHouse2":
//				return EventHandler.GameLocation.ONE_ROOM_HOUSE_2;
//			case "OneRoomHouse3":
//				return EventHandler.GameLocation.ONE_ROOM_HOUSE_3;
//			case "OneRoomHouse4":
//				return EventHandler.GameLocation.ONE_ROOM_HOUSE_4;
//			case "SmallHouse1":
//				return EventHandler.GameLocation.SMALL_HOUSE_1;
//			case "SmallHouse2":
//				return EventHandler.GameLocation.SMALL_HOUSE_2;
//			case "TwoStoryHouse1":
//				return EventHandler.GameLocation.TWO_STORY_HOUSE_1;
//			case "TwoStoryHouse2":
//				return EventHandler.GameLocation.TWO_STORY_HOUSE_2;
			}
		return playerStartPosition;
	}
}
