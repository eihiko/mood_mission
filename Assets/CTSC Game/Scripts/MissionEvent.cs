using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class MissionEvent : MonoBehaviour {

	private MissionManager mm;
	public MissionManager.MissionType missionType;
	public MissionManager.EventType eventType;
	public GameObject interactionPerson;
	public GameObject[] eventWaypoints;
	public string[] eventBriefs;
	private Mission mission;
	private bool isComplete = false;
	private bool isBusy = false;
	MissionAction currAction;
	Queue<MissionAction> actionQ = new Queue<MissionAction>();

	public AudioClip currentAudio;

	private string currentBrief;
	private MissionAction currentAction;

	// Use this for initialization
	void Start () {
		isComplete = false;
		mm = GameObject.Find("MissionManager").GetComponent<MissionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		bool executionComplete = this.execute (actionQ);
		if (isBusy && executionComplete) {
			isBusy = false;
			Debug.Log ("done executing actions, event complete");
			//OnComplete ();
			isComplete = true;
		} else if (isBusy && !executionComplete) {
			//this.execute (actionQ);
			Debug.Log ("Still executing action");
			isComplete = false;
		} else if (!isBusy && !executionComplete) {
			//Debug.Log ("no actions received yet");
		}
			//			Debug.Log ("done executing actions, event complete");
			//			OnComplete ();
			//		} else {
			//			Debug.LogError("Mission execution did not go as planned...");
			//		}
		//Handles completion of this game event.
		if (isComplete &&
		    mm.getCurrentMission().getMissionType() 
		    == missionType) {
			OnComplete();
		}
	}

	//Just have a stack of actions and pop each action from the statck
	//when it is completed. Then call OnComplete() to complete this event.
	void OnTriggerEnter(Collider c){
		//Determine what to do based on the current mission
		mission = mm.getCurrentMission ();
		//isBusy = actionQ.Count > 0;
		Debug.Log ("queueing actions for next event");
		if (!isBusy) {
			Debug.Log("System is not busy");
			switch (eventType) {
			//Mission one events
			case MissionManager.EventType.INTRO:
				Debug.Log ("into the INTRO event");
			    //Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
//				//Gui must ACTIVE(true, brief)
//				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 0, 3));
//			    //Torkana must MOVE(currLoc, adjToPlayer, limp)
//				actionQ.Enqueue (new MoveAction (mm.Torkana, mm.Player, AnimationEngine.Type.LIMP));  
//			    //Torkana must TALK(audio, guiToShow)
//			    actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 3, 2));
//				//Torkana must TURN(faceHouse)
//				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.TorkanaHouse));
//				//Torkana must MOVE(currLoc, adjToHouse)
//				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaHouse, AnimationEngine.Type.LIMP));
//				//Torkana must ENTER(mentorHouse)
				actionQ.Enqueue(new ActiveAction(mm.Torkana, false));
				//Torkana must SIT(tableInHouse) before Player enters
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaSitPos)); //Pair this with a sit when sitting
				actionQ.Enqueue(new ActiveAction(mm.Torkana, true));
				actionQ.Enqueue(new SitAction(mm.Torkana, mm.TorkanaSitPos));
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.inFrontTorkanaDoor, false, 0));
				//Player must UNFREEZE
				actionQ.Enqueue(new FreezeAction(mm.Player, false));
				//actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.WOOD, "Go to the backyard and" +
				//	" gather wood by pressing G"));
				//Player must ENTER(mentorHouse)
				actionQ.Enqueue(new EnterAction(mm.Player, mm.inFrontTorkanaDoor,
				                                "Go to Torkana's front door and press E to enter"));
				isBusy = true;
				break;
			case MissionManager.EventType.ENTER_GUIDES:

				Debug.Log ("into the enter guides event");
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue(new TurnAction(mm.Player, mm.Torkana, true, 40));
				//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue (new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 5, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				//Player must drop wood
				actionQ.Enqueue(new DropAction(mm.Player, GrabMe.kind.WOOD));
				isBusy = true;
				break;
			case MissionManager.EventType.CANDLE:

				Debug.Log ("into the candle event");
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue (new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 6, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Gui must ACTIVE(true, brief)
			//	actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 7, 1));
			//Player must GRAB(Candle)
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.CANDLE, "Grab the candle on the table with G"));
			//Player must GRAB(Key)
		//		actionQ.Enqueue (new GrabAction (mm.Player, mm.Key, "Grab the key on the table with G"));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 7, 1));
			//Player must ENTER(mentorBasement)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.MentorBasement,
				                                  "Stand near Torkana's basement door and press E to enter"));
				isBusy = true;
				break;
			case MissionManager.EventType.ENTER_GUIDE_BASEMENT:

				Debug.Log ("into the guides basement event");
			//Candle must ACTIVE(true)
				//we use a headlamp here instead of a real candle
				actionQ.Enqueue (new ActiveAction (mm.Candle, true));
				isBusy = true;
				break;
			case MissionManager.EventType.DROP_KEY:
			//Player must DROP(Key)
			//	actionQ.Enqueue (new DropAction (mm.Player, GrabMe.kind.KEY));
			//Candle must ACTIVE(false)
				//we use a headlamp here instead of a real candle
				actionQ.Enqueue (new ActiveAction (mm.Candle, false));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 8, 1));
				isBusy = true;
				break;
			case MissionManager.EventType.RELIGHT_CANDLE:
			//Player must FIND(Match) to light candle (GRAB?)
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.MATCH, "Find some matches then grab them with G"));
				actionQ.Enqueue (new ActiveAction (mm.Candle, true));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 9, 1));
				isBusy = true;
				break;
			case MissionManager.EventType.FIND_KEY:
			//Player must FIND(Key) to open chest (GRAB?)
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.KEY, "Find the key then grab it with G"));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 10, 1));
				isBusy = true;
				break;
			case MissionManager.EventType.OPEN_CHEST:
			//Player must OPEN(Chest)
			//here we replace chest with open chest
				actionQ.Enqueue (new OpenAction (mm.Player, mm.ChestClosed, mm.ChestOpen));
				isBusy = true;
				break;
			case MissionManager.EventType.GATHER_INITIAL_SUPPLIES:
			//Player must GRAB(Supplies)
			//Supplies is a game object of supplies
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.SHIELD, "Gather the shield from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.KNIFE, "Gather the knife from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.TORCH, "Gather the torch from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.COMPASS, "Gather the compass from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.GOLD, "Gather the gold from the chest with G"));

			//Torkana must STAND(inFrontOfDoor)
			//Stands in front of his front door inside
				actionQ.Enqueue (new StandAction (mm.Torkana, mm.inFrontTorkanaDoor));
			//Player must ENTER(mentorHouse)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.leavingHouse,
				                                  "Stand near the ladder and press E to go upstairs"));
				actionQ.Enqueue (new ActiveAction (mm.Candle, false));
				isBusy = true;
				break;
			case MissionManager.EventType.MEET_GUIDE:
			//Player must MOVE(currLoc, TorkanaLoc)
			//	actionQ.Enqueue (new MoveAction (mm.Player, mm.Torkana));
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue (new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 11, 2));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				//Player must GRAB(Candle)
				actionQ.Enqueue (new DropAction (mm.Player, GrabMe.kind.CANDLE));
			//Torkana must ENTER(Forest)
				actionQ.Enqueue (new ActiveAction (mm.Torkana, false));
				actionQ.Enqueue (new MoveAction (mm.Torkana, mm.inFrontTorkanaHouse));
			//Torkana must STAND(inFrontOfHouse)
				actionQ.Enqueue (new StandAction (mm.Torkana, mm.inFrontTorkanaHouse));
			//Player must ENTER(Forest)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.inFrontTorkanaHouse,
				                                  "Stand near Torkana's front door and press E"));
			//Checkpoint to reflect with gui and input, write data to database
				isBusy = true;
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
		}

//		Debug.Log ("done queueing actions");
//
//		if (this.execute (actionQ)) {
//			Debug.Log ("done executing actions, event complete");
//			OnComplete ();
//		} else {
//			Debug.LogError("Mission execution did not go as planned...");
//		}
		
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
		mm.getCurrentMission ().setEventComplete (eventType);
	}
	
	//Executes actions provided the action queue
    //Each action queue represents a mission event
	bool execute(Queue<MissionAction> actionQ){
		if (actionQ.Count > 0 && currAction == null && !isComplete) {
			Debug.Log ("we have at least one action in the queue!");
			Debug.Log ("there are this many actions in queue: " + actionQ.Count);
			currAction = actionQ.Dequeue ();
		}
		//begin the mission event with its first action
//		if (actionQ.Count > 0 && execComplete) {
//			Debug.Log ("we have at least one action in the queue!");
//			Debug.Log ("there are this many actions in queue: " + actionQ.Count);
//			currAction = actionQ.Dequeue ();
//			if (currAction == null)
//				Debug.Log ("but that action is null?");
//		} else {
//			Debug.Log ("no actions in the queue...");
//			return true;
//		}

	//	bool isComplete = false;
		if (actionQ.Count >= 0 && currAction != null) {
			Debug.Log ("Executing mission actions for this event");
//			if (actionQ.Count == 0 && isComplete){
//				Debug.Log ("event is complete...");
//				currAction = null;
//			}
			//Action runs its own loop until it's completed
			//then execute will return true if successfully completed
			if(currAction.execute()){
		    //return currAction.execute();
				if (actionQ.Count > 0){
					Debug.Log("Dequeueing next action, last was completed");
					currAction = actionQ.Dequeue();
					isBusy = true;
				} else {
					currAction = null;
					Debug.Log ("last action was completed, now event is complete...");
					return true;
				}
				//return true;
		     } else { //Otherwise, we can't continue the story.
//			    //may want to change this later to keep trying action

				Debug.Log ("Still executing action..please hold");
		    }
		}
		return false;
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
