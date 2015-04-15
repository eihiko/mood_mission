using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
[SerializeAll]
public class MissionManager : MonoBehaviour {

	public enum MissionType {
		GUIDES_HUT, FOREST, DOCTORS_OFFICE, CROSS_BRIDGE, ENTER_CITY_GATHER_SUPPLIES, 
		TALK_TO_SON, HELP_TP_1, HELP_TP_2, HELP_TP_3, HYDRA, SEWER_QUEST, 
	}

	//This should eventually be in its own class.
	public enum EventType {
		//First mission at Guide's Hut (11 events)
		INTRO, ENTER_GUIDES, CANDLE, ENTER_GUIDE_BASEMENT, DROP_KEY, RELIGHT_CANDLE, 
		FIND_KEY, OPEN_CHEST, GATHER_INITIAL_SUPPLIES, MEET_GUIDE, LEAVE_GUIDES, 
		//Second mission in Forest (6 events)
		//**Losing the map triggers minimap to disappear**
		FOLLOW_GUIDE, LOSE_MAP, ENCOUNTER_BEES, TOLERATE_BEES, RETRIEVE_MAP, 
		GO_TO_DOCTORS,
		//Third Mission at Doctor's Office (10 events)
		ENTER_DOCTORS, DOCTOR_INTRO, GUIDE_EXAM, REACH_GARDEN, TOLERATE_BEES_AGAIN,
		GATHER_HERBS, GIVE_DOCTOR_HERBS, PLAYER_EXAM, ATTAIN_AMULET, LEAVE_FOREST,
		//Fourth Mission leaving Forest (3 events)
		ENTER_BRIDGE, CROSS_BRIDGE, EXIT_BRIDGE, 
		//Fifth Mission entering city (7 events)
		//Must account for time taken to gather mt1 water
		//If too much time taken, must fetch medicine.. deal with this later..
		ENTER_CITY, TALK_TO_MT1, ENTER_TAVERN, GATHER_SELF_SUPPLIES, GATHER_MT1_WATER,
		//These will have to be checked for if player takes too long..
		NEEDS_MEDICINE,
		//bridge sequence = 28 to 30
		GATHER_MEDICINE, 
		//bridge sequence again..
		//Finish talking will have to be determined if MT1 needed medicine
		FINISH_TALKING_MT1, GO_TO_MT2_HOUSE,
		//Sixth Mission to Sewer Designer's son's house (2 events)
		CONFRONT_MT2, OFF_TO_HELP_PEOPLE,
		//Seventh Mission to help FT2 (8 events)
		//The player has the option of getting the photo album or just
		//handing FT2 the supplies. We must check if these events based on what
		//the player decides. 
		ENTER_FT2_HOUSE, TALK_TO_FT2, ENTER_FT2_BASEMENT, GRAB_SUPPLIES, 
		//Checking for these will be similar to the MT1 mission
		FIND_PHOTO_ALBUM, CLEAR_SPIDERS, FINISH_TALKING_FT2, EXIT_FT2_HOUSE,
		//Next two townspeople to help missions use these..
		//Use these to conclude helping missions as well.
		RETURN_TO_SON, TALK_TO_SON, EXIT_SONS_HOUSE,
		//Eight Mission to help MT3
		ENTER_MT3_HOUSE, TALK_TO_MT3, 
		//Ninth Mission to help FT3
		ENTER_FT3_HOUSE, TALK_TO_FT3,
		//Tenth Mission to go on quest for Hydra
		//use previous return talk and exit son for this mission also
		BEGIN_HYDRA_QUEST, BATTLE_HYDRA, DEFEAT_HYDRA,
		//Eleventh Mission to begin sewer quest
		//uses the previous values of return talk and exit son
		ATTAIN_SEWER_MAP,
		//
		NULL
	}

	public EventHandler eventHandler;
	public GameObject UISet;
	public GameObject[] missionObjects;
	public GameObject Player;
	public GameObject Torkana;

	public GameObject currentUI;

	//mission 1 objects
	public GameObject TorkanaHouse, TorkanaSitPos, inFrontTorkanaDoor,
	inFrontTorkanaHouse;
	public GameObject Candle;
	public GameObject MentorBasement, ChestClosed, ChestOpen;
	public GameObject leavingHouse;

	//UI indexed by name.
	//These should be unique to each mission in the future.
	private	Dictionary<string, GameObject> uiSet = new Dictionary<string, GameObject>(); 
	private bool firstPlay = true;

	/**
	 * A mission is a level in the game.
	 * The boolean is whether the level was completed yet.
	 * This is essentially a truth table to determine what missions the player
	 * has completed.
	 */
	private SortedList<Mission, bool> missionHistory = new SortedList<Mission, bool>();

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
		if (missionHistory.Count == 0){
			initializeMissions();
			//Player will not be able to complete any missions until story begins
			foreach (GameObject mission in missionObjects){
				mission.SetActive(false);
				foreach(Transform child in mission.transform){
					child.gameObject.SetActive(false);
				}
			}
			Debug.Log ("Initialized and disabled all missions.");
		}
	}

	// Use this for initialization
	void Start () {
		getCurrentMission ();
		UISet.SetActive (true);
		if (firstPlay == true){
			foreach(Transform ui in UISet.transform){
				uiSet.Add(ui.name, ui.gameObject);
			}
			firstPlay = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Mission currMission = getCurrentMission();
		if (currMission == null) {
			Debug.Log ("Game is completed!");
		} else if (!currMission.getIsComplete ()) {
			//Debug.Log ("Executing Mission.");
			currMission.execute();
		} else {
			Debug.Log ("Player has completed: " + currMission.ToString ());
			missionHistory[currMission] = true;
		}
	}

	public void playUI(string name){
		Screen.showCursor = true;
		Screen.lockCursor = false;
		eventHandler.setGameState (EventHandler.GameState.GUI);
		GameObject currUI;
		uiSet.TryGetValue(name, out currUI) ;
		if (currUI != null) {
			currUI.SetActive(true);
		}
	}

	public void removeThisUI(GameObject ui){
		Screen.showCursor = false;
		Screen.lockCursor = true;
		eventHandler.setGameState (EventHandler.GameState.PLAY);
		ui.SetActive(false);
		uiSet.Remove(ui.name);
	}

	public Mission getCurrentMission(){
		/** 
		 * Since missions are sorted by order of occurrence in game,
		 * we can iterate through list of missions and find the first
		 * that is not complete. That will be the next mission to complete.
		 */
		foreach(KeyValuePair<Mission, bool> mission in missionHistory){
			//Debug.Log("Current mission id is: " + mission.Key.getID());
			GameObject currMission = missionObjects[mission.Key.getID()];
			if (!mission.Value) {
				if(currMission.activeSelf == false){
					currMission.SetActive(true);
				}
				mission.Key.setIsCurrentMission(true);
				return mission.Key;
			} else {
				if(currMission.activeSelf == true){
					mission.Key.setIsCurrentMission(false);
					currMission.SetActive(false);
				}
			}
		}
		//Otherwise, the game is probably complete.
		//Will check for null's later on in game dev process..
		return null;
	}

	/**
	 * Sets up all the missions for the game that are known thus far.
	 * A mission consists of an ID and a mission type for unique identification.
	 * All created missions are added to the mission history list with a value
	 * of false since they are not completed upon initialization.
	 * Currently, the missions are indexed by their placement in the enum 
	 * MissionType. The first element has a 0 index and hence, it is the first mission.
	 */
	private void initializeMissions(){
		Mission currMission;
		//List<Transform> eventTransforms = new List<Transform>();
		List<MissionManager.EventType> events = new List<MissionManager.EventType> ();
		Dictionary<MissionManager.EventType, Transform> eventDict = new Dictionary<MissionManager.EventType, Transform> ();
		Transform currTransform;
		foreach (MissionType type in Enum.GetValues(typeof(MissionType))) {
			//Add events specific to the given mission
			switch (type){
				case MissionType.GUIDES_HUT:
					for(int i = 0; i < 11; i++) {
						events.Add((MissionManager.EventType)i);
					//Debug.Log (((MissionManager.EventType)i).ToString ());
					}
					currTransform = missionObjects[(int)type].transform;
				    //Add all the event transforms for this mission to its event dictionary
					foreach (Transform child in currTransform){
						Debug.Log ("Adding event to dict: " + child.GetComponent<MissionEvent>().eventType.ToString());
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
//				case MissionType.FOREST:
//					for(int i = 11; i < 17; i++) {
//						events.Add((EventType)i);
//					}
//					currTransform = missionObjects[(int)type].transform;
//					foreach (Transform child in currTransform){
//						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
//					}
//					break;
//				case MissionType.DOCTORS_OFFICE:
//					for(int i = 17; i < 27; i++) {
//						events.Add((EventType)i);
//					}
//					currTransform = missionObjects[(int)type].transform;
//					foreach (Transform child in currTransform){
//						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
//					}
//					break;
//				case MissionType.CROSS_BRIDGE:
//					for(int i = 27; i < 30; i++) {
//						events.Add((EventType)i);
//					}
//					currTransform = missionObjects[(int)type].transform;
//					foreach (Transform child in currTransform){
//						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
//					}
//					break;
//				case MissionType.ENTER_CITY_GATHER_SUPPLIES:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.TALK_TO_SON:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.HELP_TP_1:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.HELP_TP_2:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.HELP_TP_3:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.HYDRA:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.SEWER_QUEST:
//					for(int i = 12; i < 18; i++) {
				//						events.Add((EventType)i);
//					}
//					break;
			}
			currMission = new Mission((int)type, type, events, eventDict);
			missionHistory.Add(currMission, false);
			events.Clear();
		}
	} 
}
