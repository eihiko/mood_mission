using UnityEngine;
using System.Collections;

public class MissionEvent : MonoBehaviour {

	private MissionManager missionManager;
	public MissionManager.MissionType missionType;
	public MissionManager.EventType eventType;
	public GameObject interactionPerson;
	public GameObject[] eventWaypoints;
	public string[] eventBriefs;
	private Mission mission;
	private bool isComplete = false;

	// Use this for initialization
	void Start () {
		missionManager = GameObject.Find("MissionManager").GetComponent<MissionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		//Handles completion of this game event.
		if (isComplete &&
		    missionManager.getCurrentMission().getMissionType() 
		    == missionType) {
			OnComplete();
		}
	}

	//Just have a stack of actions and pop each action from the statck
	//when it is completed. Then call OnComplete() to complete this event.
	void OnTriggerEnter(Collider c){
		switch (eventType){
		//Mission one events
		case MissionManager.EventType.INTRO:
			//Do the next event if the previous one is completed
			//Player must FREEZE
			//Torkana must MOVE(currLoc, adjToPlayer, limp)
			//Torkana must TALK(audio, guiToShow)
			//Torkana must TURN(faceHouse)
			//Torkana must MOVE(currLoc, adjToHouse)
			//Torkana must ENTER(mentorHouse)
			//Torkana must SIT(tableInHouse) before Player enters
			//Player must UNFREEZE
			//Player must ENTER(mentorHouse)
			break;
		case MissionManager.EventType.ENTER_GUIDES:
			//Player must FREEZE
			//Torkana must TALK(audio, guiToShow)
			break;
		case MissionManager.EventType.CANDLE:
			//Gui must ACTIVE(true, brief)
			//Player must GRAB(Candle)
			//Player must GRAB(Key)
			//Gui must ACTIVE(true, brief)
			//Player must ENTER(mentorBasement)
			break;
		case MissionManager.EventType.ENTER_GUIDE_BASEMENT:
			//Candle must ACTIVE(true)
			break;
		case MissionManager.EventType.DROP_KEY:
			//Player must DROP(Key)
			//Candle must ACTIVE(false)
			//Gui must ACTIVE(true, brief)
			break;
		case MissionManager.EventType.RELIGHT_CANDLE:
			//Player must FIND(Match) to light candle (GRAB?)
			//Gui must ACTIVE(true, brief)
			break;
		case MissionManager.EventType.FIND_KEY:
			//Player must FIND(Key) to open chest (GRAB?)
			//Gui must ACTIVE(true, brief)
			break;
		case MissionManager.EventType.OPEN_CHEST:
			//Player must OPEN(Chest)
			break;
		case MissionManager.EventType.GATHER_INITIAL_SUPPLIES:
			//Player must GRAB(Supplies)
			//Torkana must STAND(inFrontOfDoor)
			//Player must ENTER(mentorHouse)
			break;
		case MissionManager.EventType.MEET_GUIDE:
			//Player must MOVE(currLoc, TorkanaLoc)
			//Torkana must TALK(audio, guiToShow)
			//Torkana must ENTER(Forest)
			//Torkana must STAND(inFrontOfHouse)
			break;
		//Mission Two events
		case MissionManager.EventType.LEAVE_GUIDES:
			//Player must ENTER(Forest)
			//Player must MOVE(currLoc, TorkanaLoc)
			break;
		case MissionManager.EventType.FOLLOW_GUIDE:
			//Torkana must TALK(audio, guiToShow)
			//Torkana must MOVE(currLoc, adjToBeeArea) iff IN_RANGE(Torkana, Player)
			//Player must MOVE(currLoc, adjToBeeArea)
			break;
		case MissionManager.EventType.LOSE_MAP:
			//Player must DROP(Map)
			//Torkana must TALK(audio, guiToShow)
			//Torkana must GIVE(Player, Amulet)
			//Player must MOVE(currLoc, BeeArea)
			break;
		case MissionManager.EventType.ENCOUNTER_BEES:
			//Gui must ACTIVE(true, brief)
			//Bees must MOVE(currLoc, Player)
			break;
		case MissionManager.EventType.TOLERATE_BEES:

			//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action) and
			//BEES must APPROVE(Action) 
			break;
		case MissionManager.EventType.RETRIEVE_MAP:
			//Player must FIND(Map) (GRAB?)
			break;
		case MissionManager.EventType.GO_TO_DOCTORS:
			//Torkana must TALK(audio, guiToShow)
			//Torkana must MOVE(currLoc, adjToDoctorsHouse) iff IN_RANGE(Torkana, Player)
			//Player must MOVE(currLoc, adjToDoctorsHouse)
			//Player must ENTER(DoctorsHouse)
			//Torkana must STAND(adjToDoctor) in the house
			break;
		//Mission Three events
		case MissionManager.EventType.ENTER_DOCTORS:
			//Torkana and Doctor must TALK(audio, noGui)
			//Player must MOVE(currLoc, adjToTorkana)
			break;
		case MissionManager.EventType.DOCTOR_INTRO:
			//Player and Doctor must TALK(audio, guiToShow)
			//Doctor must EXAMINE(Torkana)
			break;
		case MissionManager.EventType.GUIDE_EXAM:
			//Doctor and Torkana must TALK(audio, noGui)
			//Player must MOVE(currLoc, adjToDoor)
			//Player must ENTER(FOREST)
			//Gui must ACTIVE(true, brief)
			//Player must MOVE(currLoc, DoctorGarden)
			break;
		case MissionManager.EventType.TOLERATE_BEES_AGAIN:
			//Gui must ACTIVE(true, brief)
			//Bees must MOVE(currLoc, Player)
			//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action)
			//and BEES must APPROVE(Action)
		//	//**and Amulet must ACTIVE(true) and Amulet must REACT(Action, BEES) and
		//	//**Amulet must APPROVE(Action, Bees)
		//	//**Dont use these until later scenarios
			break;
		case MissionManager.EventType.GATHER_HERBS:
			//Gui must ACTIVE(true, brief)
			//Player must GRAB(Herb)
			//Player must MOVE(currLoc, adjToDoor)
			//Gui must ACTIVE(true, brief)
			//Player must ENTER(DoctorsHouse)
			//Torkana must STAND(adjToDoctor)
			break;
		case MissionManager.EventType.GIVE_DOCTOR_HERBS:
			//Torkana must TALK(audio, noGui)
			//Player must MOVE(currLoc, adjToDoctor)
			//Doctor must TALK(audio, guiToShow)
		    //Player must GIVE(Herb, Doctor)
			//Doctor must TALK(audio, guiToShow)
				break;
		case MissionManager.EventType.PLAYER_EXAM:
			//Player and Doctor must TALK(audio, guiToShow)
			//Doctor must EXAMINE(Player)
			//Doctor must TALK(audio, noGui)
			break;
		case MissionManager.EventType.ATTAIN_AMULET:
			//Torkana must GIVE(Amulet, Player)
			//Torkana must TALK(audio, guiToShow)
			//Torkana must MOVE(currLoc, adjToDoor)
			//Torkana must ENTER(Forest)
			//Torkana must ACTIVE(false)
			break;
		//Mission 4 Actions
		case MissionManager.EventType.LEAVE_FOREST:
			//Player must MOVE(currLoc, adjToDoor)
			//Player must ENTER(Forest)
			break;
		//About 1/3 way through missions...
		}

		//This is an easy example of how to complete this event
//		if(c.tag == "Player"){
//			setIsComplete (true);
//		}
	}

	/**
	 * Communicates with its own mission type in order to
	 * rectify that it is complete.
	 */
	void OnComplete(){
		missionManager.getCurrentMission ().setEventComplete (eventType);
	}

	public MissionManager.EventType getEventType(){
		return this.eventType;
	}

	public void setIsComplete(bool complete){
		this.isComplete = complete;
	}

	public bool getIsComplete(){
		return isComplete;
	}
}
