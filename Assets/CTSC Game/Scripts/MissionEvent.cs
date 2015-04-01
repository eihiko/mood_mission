using UnityEngine;
using System;
using System.Collections.Generic;
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
	public GameObject Player;
	public GameObject Torkana;

	//mission 1 objects
	public GameObject TorkanaHouse, TorkanaSitPos, inFrontTorkanaDoor,
					inFrontTorkanaHouse;
	public GameObject Candle, Key, Match;
	public GameObject MentorBasement, Chest, ChestOpen, Supplies;

	private AudioClip currentAudio;
	private GameObject currentUI;
	private string currentBrief;
	private MissionAction currentAction;

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
		//Determine what to do based on the current mission
		mission = missionManager.getCurrentMission ();
		Queue<MissionAction> actionQ = new Queue<MissionAction>();
		switch (eventType){
		//Mission one events
		case MissionManager.EventType.INTRO:
			//Do the next event if the previous one is completed
			//Player must FREEZE
			actionQ.Enqueue(new FreezeAction(Player, true));
			//Torkana must MOVE(currLoc, adjToPlayer, limp)
			actionQ.Enqueue(new MoveAction(Torkana, Player, AnimationEngine.Type.LIMP));  
			//Torkana must TALK(audio, guiToShow)
			actionQ.Enqueue(new TalkAction(Torkana, currentAudio, currentUI));
			//Torkana must TURN(faceHouse)
			actionQ.Enqueue(new TurnAction(Torkana, TorkanaHouse));
			//Torkana must MOVE(currLoc, adjToHouse)
			actionQ.Enqueue(new MoveAction(Torkana, TorkanaHouse, AnimationEngine.Type.LIMP));
			//Torkana must ENTER(mentorHouse)
			actionQ.Enqueue(new ActiveAction(Torkana, false));
			//Torkana must SIT(tableInHouse) before Player enters
			//actionQ.Enqueue(new MoveAction(Torkana, TorkanaSitPos)); //Pair this with a sit when sitting
			actionQ.Enqueue(new SitAction(Torkana, TorkanaSitPos));
			actionQ.Enqueue(new TurnAction(Torkana, inFrontTorkanaDoor));
			//Player must UNFREEZE
			actionQ.Enqueue(new FreezeAction(Player, false));
			//Player must ENTER(mentorHouse)
			actionQ.Enqueue(new EnterAction(Player, TorkanaHouse));
			break;
		case MissionManager.EventType.ENTER_GUIDES:
			//Player must FREEZE
			actionQ.Enqueue(new FreezeAction(Player, true));
			//Torkana must TALK(audio, guiToShow)
			actionQ.Enqueue(new TalkAction(Torkana, currentAudio, currentUI));
			break;
		case MissionManager.EventType.CANDLE:
			//Gui must ACTIVE(true, brief)
			actionQ.Enqueue(new ActiveAction(currentUI, true, currentBrief));
			//Player must GRAB(Candle)
			actionQ.Enqueue(new GrabAction(Player, Candle));
			//Player must GRAB(Key)
			actionQ.Enqueue(new GrabAction(Player, Key));
			//Gui must ACTIVE(true, brief)
			actionQ.Enqueue(new ActiveAction(currentUI, true, currentBrief));
			//Player must ENTER(mentorBasement)
			actionQ.Enqueue(new EnterAction(Player, MentorBasement));
			break;
		case MissionManager.EventType.ENTER_GUIDE_BASEMENT:
			//Candle must ACTIVE(true)
			actionQ.Enqueue(new ActiveAction(Candle, true));
			break;
		case MissionManager.EventType.DROP_KEY:
			//Player must DROP(Key)
			actionQ.Enqueue(new DropAction(Player, Key));
			//Candle must ACTIVE(false)
			actionQ.Enqueue(new ActiveAction(Candle, false));
			//Gui must ACTIVE(true, brief)
			actionQ.Enqueue(new ActiveAction(currentUI, true, currentBrief));
			break;
		case MissionManager.EventType.RELIGHT_CANDLE:
			//Player must FIND(Match) to light candle (GRAB?)
			actionQ.Enqueue(new GrabAction(Player, Match));
			actionQ.Enqueue(new ActiveAction(Candle, true));
			//Gui must ACTIVE(true, brief)
			actionQ.Enqueue(new ActiveAction(currentUI, true, currentBrief));
			break;
		case MissionManager.EventType.FIND_KEY:
			//Player must FIND(Key) to open chest (GRAB?)
			actionQ.Enqueue(new GrabAction(Player, Key));
			//Gui must ACTIVE(true, brief)
			actionQ.Enqueue(new ActiveAction(currentUI, true, currentBrief));
			break;
		case MissionManager.EventType.OPEN_CHEST:
			//Player must OPEN(Chest)
			//here we replace chest with open chest
			actionQ.Enqueue(new OpenAction(Player, Chest, ChestOpen));
			break;
		case MissionManager.EventType.GATHER_INITIAL_SUPPLIES:
			//Player must GRAB(Supplies)
			//Supplies is a game object of supplies
			actionQ.Enqueue(new GrabAction(Player, Supplies));
			//Torkana must STAND(inFrontOfDoor)
			//Stands in front of his front door inside
			actionQ.Enqueue(new StandAction(Torkana, inFrontTorkanaDoor));
			//Player must ENTER(mentorHouse)
			actionQ.Enqueue(new EnterAction(Player, TorkanaHouse));
			break;
		case MissionManager.EventType.MEET_GUIDE:
			//Player must MOVE(currLoc, TorkanaLoc)
			actionQ.Enqueue(new MoveAction(Player, Torkana));
			//Torkana must TALK(audio, guiToShow)
			actionQ.Enqueue(new TalkAction(Torkana, currentAudio, currentUI));
			//Torkana must ENTER(Forest)
			actionQ.Enqueue(new ActiveAction(Torkana, false));
			actionQ.Enqueue(new MoveAction(Torkana, inFrontTorkanaHouse));
			//Torkana must STAND(inFrontOfHouse)
			actionQ.Enqueue(new StandAction(Torkana, inFrontTorkanaHouse));
			//Player must ENTER(Forest)
			actionQ.Enqueue(new EnterAction(Player, inFrontTorkanaHouse));
			//Checkpoint to reflect with gui and input, write data to database
			break;
		//Mission Two events
		case MissionManager.EventType.LEAVE_GUIDES:
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
			//Checkpoint to reflect with gui and input, write data to database
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
			//Checkpoint to reflect with gui and input, write data to database
			break;
		//Mission 4 Actions
		case MissionManager.EventType.LEAVE_FOREST:
			//Player must MOVE(currLoc, adjToDoor)
			//Player must ENTER(Forest)
			break;
		//About 1/3 way through missions...
		}

		if (this.execute (actionQ)) {
			OnComplete ();
		} else {
			Debug.LogError("Mission execution did not go as planned...");
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
	
	//Executes actions provided the action queue
    //Each action queue represents a mission event
	bool execute(Queue<MissionAction> actionQ){
		MissionAction currAction;
		//begin the mission event with its first action
		if (actionQ.Count > 0) {
			currAction = actionQ.Dequeue ();
		} else {
			return false;
		}
				bool isComplete = false;
		while (actionQ.Count >= 0 && currentAction != null) {
			if (actionQ.Count == 0 && isComplete){
				currAction = null;
			}
			//Action runs its own loop until it's completed
			//then execute will return true if successfully completed
			if(currAction.execute()){
				currAction = actionQ.Dequeue();
			} else { //Otherwise, we can't continue the story.
			    //may want to change this later to keep trying action
				return false;
		    }
		}
		return true;
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
