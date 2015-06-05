using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
[SerializeAll]
public class MissionManager : MonoBehaviour {

	public enum MissionType {
		GUIDES_HUT, FOREST, DOCTORS_OFFICE, ENTER_CITY_GATHER_SUPPLIES, 
		TALK_TO_SON, HELP_TP_1, HELP_TP_2, HELP_TP_3, CYCLOPS, HYDRA, SEWER_QUEST, 
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
		GATHER_HERBS, GATHER_HEALING_WATER, GIVE_DOCTOR_HERBS, PLAYER_EXAM, ATTAIN_AMULET, 
		//Fourth Mission entering city (9 events)
		//Must account for time taken to gather mt1 water
		//If too much time taken, must fetch medicine.. deal with this later..
		LEAVE_FOREST, ENTER_CITY, TALK_TO_MT1, ENTER_TAVERN, GATHER_SELF_SUPPLIES, GATHER_MT1_WATER,
		//These will have to be checked for if player takes too long to get the water..
		NEEDS_MEDICINE,
		//go back to Torkana's for medicine..
		GIVE_MEDICINE, 
		//come back
		//Finish talking will have to be determined if MT1 needed medicine
		FINISH_TALKING_MT1,

		//Fifth Mission to Sewer Designer's son's house (3 events)
		GO_TO_MT2_HOUSE, CONFRONT_MT2, OFF_TO_HELP_PEOPLE,

		//Sixth Mission - Old man and the blacksmith (9 events)
		ENTER_MT3_HOUSE, TALK_TO_MT3, GO_TO_BLACKSMITH, TALK_TO_BLACKSMITH,
		//Tool creation: include failure loops
		CREATE_TOOL_1, CREATE_TOOL_2, CREATE_TOOL_3, 
		RETURN_TO_MT3, FINISH_TALKING_MT3,

		//Seventh Mission - Letter delivery (8 events)
		ENTER_FT1_HOUSE, TALK_TO_FT1, EXIT_FT1_HOUSE, 
		//These must play thunder sounds and start particle effect(?) at certain points
		THUNDER_BEGINS, RAIN_STARTS,
		//These must check the choice of the player during the storm
		RETURN_TO_FT1, WAIT_FOR_DRIZZLE, WAIT_FOR_END, 
		
		//Eighth Mission - Locket and end of Son part (12 events)
		HEAD_BACK_MT2, TALK_TO_FT2, ENTER_FT2_HOUSE, FIND_DRAWINGS, SHOW_DRAWINGS, FIND_PICTURES, SHOW_PICTURES, FIND_LOCKET, GIVE_LOCKET,
		RETURN_TO_SON, TALK_TO_SON, EXIT_SONS_HOUSE,

		//Ninth Mission to deal with cyclops (12 events)
		TOWARD_TOWN_CENTER, TALK_TO_FT3, APPLES_FROM_FT3, 
		TALK_TO_MT4, CLOSER_TOWN_CENTER, 
		//Need to play sound file here
		HEAR_LAUGHING, 
		//Finally at the town center where the mission truly begins
		REACH_TOWN_CENTER, TALK_TO_GUARD,
		GIVE_CYCLOPS_APPLES,
		//These must check the dialogue choice of cyclops, with some sort of loop until the correct one is selected
		CYCLOPS_CHOICE,
		//If the cyclops chooses wrong
		GET_MORE_APPLES, FEED_CYCLOPS_MORE,


		//Tenth Mission to go on quest for Hydra
		//use previous return talk and exit son for this mission also
		BEGIN_HYDRA_QUEST, BATTLE_HYDRA, DEFEAT_HYDRA,
		//Tenth Mission to begin sewer quest
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

	//mission objects
	public GameObject TorkanaHouse, TorkanaSitPos, inFrontTorkanaDoor,
	inFrontTorkanaHouse;
	public GameObject Candle;
	public GameObject MentorBasement, ChestClosed, ChestOpen;
	public GameObject TorkanaStandPos, leavingHouse;
	public GameObject atBeeArea, DoctorGardenBees;
	public GameObject Map, Bees, DoctorsHouse, PlayerEnterDoctors, TorkanaEnterDoctors;
	public GameObject nearDoctor, TorkanaNearDoctor, Doctor, GoingToBees, DoctorGarden;
	public GameObject CityEntrance;
	public GameObject InjuredPerson; //Note, is also classified as injuredPerson
	public GameObject nearInjuredPerson;
	public GameObject TavernEntrance, insideTavern, nearTavernKeeper;
	public GameObject HealingCaveEntrance, HealingCaveExit, healthPotion, CaveSwitch, CaveGateOpened, CaveGateClosed;
	public GameObject TDC;
	public GameObject Son;
	public GameObject InsideSonHouse, OutsideSonHouse, leavingSonsHouse;
	public GameObject MT3, InsideMT3House, OutsideMT3House, GoingToBlacksmith;
	public GameObject Blacksmith, AtBlacksmith, leavingBlacksmith, BackWithTools;
	public GameObject Tools;
	
	public MissionManager.MissionType currMissionType;

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
//			Debug.Log ("Initialized and disabled all missions.");
		}
	}

	// Use this for initialization
	void Start () {
		test (5);
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
		currMissionType = currMission.getMissionType ();
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

	public void test(int missionsCompleted){
		for (int i=0; i<missionsCompleted; i++) {
			Mission currMission = getCurrentMission();
			missionHistory[currMission] = true;
		}
	}

	public void playUI(string name){
		Cursor.visible = true;
		Screen.lockCursor = false;
		eventHandler.setGameState (EventHandler.GameState.GUI);
		GameObject currUI;
		uiSet.TryGetValue(name, out currUI) ;
		if (currUI != null) {
			currUI.SetActive(true);
		}
	}

	public void removeThisUI(GameObject ui){
		Cursor.visible = false;
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
		Debug.Log ("Couldnt find current mission, game done or error");
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
		int currMissionNum = 0;
		foreach (MissionType type in Enum.GetValues(typeof(MissionType))) {
			//Add events specific to the given mission
			switch (type){
				case MissionType.GUIDES_HUT:
					for(int i = 0; i < 11; i++) {
						events.Add((MissionManager.EventType)i);
					//Debug.Log (((MissionManager.EventType)i).ToString ());
					}
					events.Add (MissionManager.EventType.NULL);
					currTransform = missionObjects[(int)type].transform;
				    //Add all the event transforms for this mission to its event dictionary
					foreach (Transform child in currTransform){
//						Debug.Log ("Adding event to dict: " + child.GetComponent<MissionEvent>().eventType.ToString());
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
				case MissionType.FOREST:
				// i = 11 but changed it for testing !
					for(int i = 11; i < 17; i++) {
						events.Add((EventType)i);
					}
					currTransform = missionObjects[(int)type].transform;
					foreach (Transform child in currTransform){
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
				case MissionType.DOCTORS_OFFICE:
					for(int i = 17; i < 27; i++) {
						events.Add((EventType)i);
					}
					currTransform = missionObjects[(int)type].transform;
					foreach (Transform child in currTransform){
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
				case MissionType.ENTER_CITY_GATHER_SUPPLIES:
					for(int i = 27; i < 36; i++) {
						events.Add((EventType)i);
					}
					currTransform = missionObjects[(int)type].transform;
					foreach (Transform child in currTransform){
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
				case MissionType.TALK_TO_SON:
					for(int i = 36; i < 39; i++) {
						events.Add((EventType)i);
					}
					currTransform = missionObjects[(int)type].transform;
					//Add all the event transforms for this mission to its event dictionary
					foreach (Transform child in currTransform){
						//						Debug.Log ("Adding event to dict: " + child.GetComponent<MissionEvent>().eventType.ToString());
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
				case MissionType.HELP_TP_1:
					for(int i = 39; i < 48; i++) {
						events.Add((EventType)i);
					}
					currTransform = missionObjects[(int)type].transform;
					//Add all the event transforms for this mission to its event dictionary
					foreach (Transform child in currTransform){
						//						Debug.Log ("Adding event to dict: " + child.GetComponent<MissionEvent>().eventType.ToString());
						eventDict.Add (child.GetComponent<MissionEvent>().eventType, child);
					}
					break;
//				case MissionType.HELP_TP_2:
//					for(int i = 48; i < 56; i++) {
//						events.Add((EventType)i);
//					}
//					break;
//				case MissionType.HELP_TP_3:
//					for(int i = 56; i < 68; i++) {
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
			//if (currMissionNum > 1){
				missionHistory.Add(currMission, false);
			//}
			events.Clear();
			currMissionNum++;
		}
	} 
}
