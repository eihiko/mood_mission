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
	private bool isTest = false;
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
		if ((isBusy && executionComplete) || isTest) {
			isBusy = false;
			isTest = false;

		//	Debug.Log ("done executing actions, event complete");
			//OnComplete ();
			isComplete = true;
		} else if (isBusy && !executionComplete) {
			//this.execute (actionQ);
		//	Debug.Log ("Still executing action");
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
	//	Debug.Log ("queueing actions for next event");
		if (!isBusy) {
	//		Debug.Log("System is not busy");
			switch (eventType) {
			//Mission one events
			case MissionManager.EventType.INTRO:
				if(mission.getCurrentMissionEvent()==eventType) {
	//			Debug.Log ("into the INTRO event");
			    //Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 0, 3));
			    //Torkana must MOVE(currLoc, adjToPlayer, limp)
				actionQ.Enqueue (new MoveAction (mm.Torkana, mm.Player, AnimationEngine.Type.LIMP));  
			    //Torkana must TALK(audio, guiToShow)
			    actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 3, 2));
				//Torkana must TURN(faceHouse)
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.TorkanaHouse, false, 0));
				//Torkana must MOVE(currLoc, adjToHouse)
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaHouse, AnimationEngine.Type.LIMP));
				//Player must UNFREEZE
				actionQ.Enqueue(new FreezeAction(mm.Player, false));
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.WOOD, "Go to the backyard and" +
					" gather wood by pressing G"));
				//Torkana must ENTER(mentorHouse)
				actionQ.Enqueue(new ActiveAction(mm.Torkana, false));
				//Torkana must SIT(tableInHouse) before Player enters
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaSitPos)); //Pair this with a sit when sitting
				actionQ.Enqueue(new ActiveAction(mm.Torkana, true));
				actionQ.Enqueue(new SitAction(mm.Torkana, mm.TorkanaSitPos));
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.inFrontTorkanaDoor, false, 0));
				//Player must ENTER(mentorHouse)
				actionQ.Enqueue(new EnterAction(mm.Player, mm.inFrontTorkanaDoor,
				                                "Go to Torkana's front door and press E to enter"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_GUIDES:
				if(mission.getCurrentMissionEvent()==eventType){
	//			Debug.Log ("into the enter guides event");
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
				}
				break;
			case MissionManager.EventType.CANDLE:
				if(mission.getCurrentMissionEvent()==eventType) {
	//			Debug.Log ("into the candle event");
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
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 7, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Player must ENTER(mentorBasement)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.MentorBasement,
				                                  "Stand near Torkana's basement door and press E to enter"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_GUIDE_BASEMENT:
				if(mission.getCurrentMissionEvent()==eventType) {
	//			Debug.Log ("into the guides basement event");
			//Candle must ACTIVE(true)
				//we use a headlamp here instead of a real candle
				actionQ.Enqueue (new ActiveAction (mm.Candle, true));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.DROP_KEY:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must DROP(Key)
			//	actionQ.Enqueue (new DropAction (mm.Player, GrabMe.kind.KEY));
			//Candle must ACTIVE(false)
				//we use a headlamp here instead of a real candle
				actionQ.Enqueue (new ActiveAction (mm.Candle, false));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 8, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.RELIGHT_CANDLE:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must FIND(Match) to light candle (GRAB?)
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.MATCH, "Find some matches then grab them with G"));
				actionQ.Enqueue (new ActiveAction (mm.Candle, true));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 9, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_KEY:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must FIND(Key) to open chest (GRAB?)
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.KEY, "Find the key then grab it with G"));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 10, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.OPEN_CHEST:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must OPEN(Chest)
			//here we replace chest with open chest
				actionQ.Enqueue (new OpenAction (mm.Player, mm.ChestClosed, mm.ChestOpen));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GATHER_INITIAL_SUPPLIES:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must GRAB(Supplies)
			//Supplies is a game object of supplies
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.SHIELD, "Gather the shield from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.KNIFE, "Gather the knife from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.TORCH, "Gather the torch from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.COMPASS, "Gather the compass from the chest with G"));
				actionQ.Enqueue (new GrabAction (mm.Player, GrabMe.kind.GOLD, "Gather the gold from the chest with G"));

			//Torkana must STAND(inFrontOfDoor)
			//Stands in front of his front door inside
				actionQ.Enqueue (new StandAction (mm.Torkana, mm.TorkanaStandPos));
			//Player must ENTER(mentorHouse)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.leavingHouse,
				                                  "Stand near the ladder and press E to go upstairs"));
				actionQ.Enqueue (new ActiveAction (mm.Candle, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.MEET_GUIDE:
				if(mission.getCurrentMissionEvent()==eventType) {
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
				actionQ.Enqueue (new ActiveAction (mm.Torkana, true));
				actionQ.Enqueue (new StandAction (mm.Torkana, mm.inFrontTorkanaHouse));
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.Player, false, 0));
			//Player must ENTER(Forest)
				actionQ.Enqueue (new EnterAction (mm.Player, mm.inFrontTorkanaHouse,
				                                  "Stand near Torkana's front door and press E"));
			//Checkpoint to reflect with gui and input, write data to database
				actionQ.Enqueue (new CheckpointAction());
				isBusy = true;
				}
				break;
			//Mission Two events
			case MissionManager.EventType.LEAVE_GUIDES:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must MOVE(currLoc, TorkanaLoc)
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 13, 2));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.FOLLOW_GUIDE:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Torkana must MOVE(currLoc, adjToBeeArea) iff IN_RANGE(Torkana, Player)
				actionQ.Enqueue(new FollowAction(0, 12, mm.Torkana));
				//actionQ.Enqueue (new MoveAction (mm.Torkana, mm.adjToBeeArea));
			//Player must MOVE(currLoc, adjToBeeArea)
				//this automatically happens b.c. follow action requires it!
				isBusy = true;
			//	isTest = true;
				}
				break;
			case MissionManager.EventType.LOSE_MAP:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must DROP(Map)
				actionQ.Enqueue(new DropAction(mm.Player, GrabMe.kind.MAP));
				//move map to ground near bees
				//actionQ.Enqueue(new MoveAction(mm.Map, mm.mapLocation));
				//activate map on ground near bees
				actionQ.Enqueue(new ActiveAction(mm.Map, true));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 15, 2));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Torkana must GIVE(Player, Amulet)
				//don't think this is happening until later
			//Player must MOVE(currLoc, BeeArea)
				actionQ.Enqueue(new EnterAction(mm.Player, mm.atBeeArea, "Go search for the map down the hill"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENCOUNTER_BEES:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Gui must ACTIVE(true, brief)
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 17, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				//actionQ.Enqueue(new MoveAction(mm.Bees, mm.Player));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TOLERATE_BEES:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Bees must MOVE(currLoc, Player)
				actionQ.Enqueue(new ActiveAction(mm.Bees, true));
			//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action) and
			//BEES must APPROVE(Action) 
				actionQ.Enqueue(new PrintAction("Hold C while you move for courage\r\n" +
				                                "Hold E while you move for compassion\r\n" +
				                                "Hold Q while you move for health\r\n", 20));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.RETRIEVE_MAP:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must FIND(Map) (GRAB?)
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.MAP, "Press G to grab map"));
			//Bees fly off to the doctor's garden
				//set the focus of the bees to the doctor's garden area
				mm.Bees.GetComponent<Swarm>().swarmFocus = mm.DoctorGardenBees.transform;
				//move the actual position to the doctor's garden area
				actionQ.Enqueue(new MoveAction(mm.Bees, mm.DoctorGardenBees));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GO_TO_DOCTORS:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue (new TalkAction (mm.Torkana, currentAudio, mm.currentUI, 18, 2));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Torkana must MOVE(currLoc, adjToDoctorsHouse) iff IN_RANGE(Torkana, Player)
			//Player must MOVE(currLoc, adjToDoctorsHouse)
			//note that Torkana moves to the doctor's house so the player also must
				actionQ.Enqueue(new FollowAction(13, 29, mm.Torkana));
			
			//Player must ENTER(DoctorsHouse)
				actionQ.Enqueue(new EnterAction(mm.Player, mm.DoctorsHouse, ""));

			//Torkana must STAND(adjToDoctor) in the house
			//Checkpoint to reflect with gui and input, write data to database
				actionQ.Enqueue (new CheckpointAction());
				isBusy = true;
				}
				break;
			//Mission Three events
			case MissionManager.EventType.ENTER_DOCTORS:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player, mm.PlayerEnterDoctors, ""));
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaEnterDoctors));
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.Doctor, false, 0));
				actionQ.Enqueue(new EnterAction(mm.Player, mm.nearDoctor, ""));
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaNearDoctor));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Torkana and Doctor must TALK(audio, noGui)
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 20, 2));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.DOCTOR_INTRO:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Player and Doctor must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction(mm.Doctor, currentAudio, mm.currentUI, 22, 2));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Doctor must EXAMINE(Torkana)
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GUIDE_EXAM:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
			//Doctor and Torkana must TALK(audio, noGui)
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 24, 2));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Player must MOVE(currLoc, adjToDoor)
				//Player must ENTER(FOREST)
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player, mm.GoingToBees, "Go straight to the garden for the herbs"));
				mm.Player.GetComponent<CharacterOurs>().canEnter = false;
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 26, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
			//Player must MOVE(currLoc, DoctorGarden)
				isBusy = true;
				}
				break;
			case MissionManager.EventType.REACH_GARDEN:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 27, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TOLERATE_BEES_AGAIN:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new MoveAction(mm.Bees, mm.DoctorGardenBees));
				//Bees must MOVE(currLoc, Player)
				actionQ.Enqueue(new ActiveAction(mm.Bees, true));
				//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action) and
				//BEES must APPROVE(Action) 
				actionQ.Enqueue(new PrintAction("Hold C while you move for courage\r\n" +
				                                "Hold E while you move for compassion\r\n" +
				                                "Hold Q while you move for health\r\n", 20));

				//Bees fly off to the doctor's garden
				//set the focus of the bees to the doctor's garden area
				mm.Bees.GetComponent<Swarm>().swarmFocus = mm.Player.transform;
				isBusy = true;
				}
				//Gui must ACTIVE(true, brief)
			//Bees must MOVE(currLoc, Player)
			//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action)
			//and BEES must APPROVE(Action)
		//	//**and Amulet must ACTIVE(true) and Amulet must REACT(Action, BEES) and
		//	//**Amulet must APPROVE(Action, Bees)
		//	//**Dont use these until later scenarios
				break;
			case MissionManager.EventType.GATHER_HERBS:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 28, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				//Player must GRAB(Herb)
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.HERB, "Press G to grab herb"));

				//set the focus of the bees to the doctor's garden area
				//mm.Bees.GetComponent<Swarm>().swarmFocus = mm.DoctorGardenBees.transform;
				actionQ.Enqueue(new MoveAction(mm.Bees, mm.TorkanaEnterDoctors));
				actionQ.Enqueue(new ActiveAction(mm.Bees, false));//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//print message to go into cave
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 29, 1));

				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));

				//mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				//actionQ.Enqueue(new EnterAction(mm.Player, mm.HealingCaveInside, ""));
			//Player must MOVE(currLoc, adjToDoor)
			//Gui must ACTIVE(true, brief)
			//Player must ENTER(DoctorsHouse)
			//Torkana must STAND(adjToDoctor)

				//set the focus of the bees to the doctor's garden area
				//mm.Bees.GetComponent<Swarm>().swarmFocus = mm.DoctorGardenBees.transform;
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GATHER_HEALING_WATER:
				if(mission.getCurrentMissionEvent()==eventType) {
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				//player must enter healing cave
				actionQ.Enqueue(new EnterAction(mm.Player, mm.HealingCaveEntrance, "Press E near the cave entrance to enter"));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 30, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				actionQ.Enqueue(new PressAction(mm.CaveSwitch));
				actionQ.Enqueue (new ActiveAction(mm.CaveGateClosed, false));
				actionQ.Enqueue (new ActiveAction(mm.CaveGateOpened, true));
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.HEALING_WATER,"Jump into the water and grab some healing water with G"));
				//player must find the health potion to get out alive
				if(mm.Player.GetComponent<CharacterOurs>().health < 50){
					actionQ.Enqueue (new FreezeAction (mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 31, 1));
					actionQ.Enqueue (new FreezeAction (mm.Player, false));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.HEALTH_POTION, "Grab the health potion with G"));
					actionQ.Enqueue (new FreezeAction (mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 32, 1));
					actionQ.Enqueue (new FreezeAction (mm.Player, false));
					actionQ.Enqueue(new ApplyAction(mm.Player, mm.Player, GrabMe.kind.HEALTH_POTION));
				}
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 33, 1));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				//player must exit healing cave
				actionQ.Enqueue(new EnterAction(mm.Player, mm.HealingCaveExit, "Press E near the cave entrance to exit"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_DOCTOR_HERBS:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must MOVE(currLoc, adjToDoctor)
				actionQ.Enqueue(new EnterAction(mm.Player, mm.nearDoctor, "Give the herbs and water to the doctor"));
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Doctor must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction(mm.Doctor, currentAudio, mm.currentUI, 34, 2));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				//Player must GIVE(Herb, Doctor)
				actionQ.Enqueue(new DropAction(mm.Player, GrabMe.kind.HERB, "Give the herb to the Doctor"));
				actionQ.Enqueue(new DropAction(mm.Player, GrabMe.kind.HEALING_WATER, "Now, give the healing water to the Doctor"));
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 36, 2));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.PLAYER_EXAM:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must SIT(Chair)
				//Doctor must heal torkana
				//actionQ.Enqueue(new AnimateAction(mm.Doctor, AnimateAction.type.Heal));
				//Doctor must heal player (fade screen and change colors)
				//actionQ.Enqueue(new AnimateAction(mm.Doctor, AnimateAction.type.Heal));
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Torkana must TALK(audio, noGui)
				//Doctor must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction(mm.Doctor, currentAudio, mm.currentUI, 38, 1));
				//Torkana must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 39, 2));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				isBusy = true;
				}

			//Player and Doctor must TALK(audio, guiToShow)
			//Doctor must EXAMINE(Player)
			//Doctor must TALK(audio, noGui)
				break;
			case MissionManager.EventType.ATTAIN_AMULET:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new GiveAction(mm.Torkana, mm.Player, GrabMe.kind.AMULET));
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.AMULET, "Grab the amulet by pressing G"));
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 41, 1));
				actionQ.Enqueue(new EnterAction(mm.Player, mm.DoctorsHouse, "Leave the Doctor's Office and head out to Merami"));
			//Torkana must GIVE(Amulet, Player)
			//Torkana must TALK(audio, guiToShow)
			//Torkana must MOVE(currLoc, adjToDoor)
			//Torkana must ENTER(Forest)
			//Torkana must ACTIVE(false)
				actionQ.Enqueue(new ActiveAction(mm.Torkana, false));
				//Doctor must ACTIVE(false)
				actionQ.Enqueue(new ActiveAction(mm.Doctor, false));
			//Checkpoint to reflect with gui and input, write data to database
				actionQ.Enqueue (new CheckpointAction());
				isBusy = true;
				}
				break;
			//Mission 4 Actions
			case MissionManager.EventType.LEAVE_FOREST:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must MOVE(currLoc, adjToDoor)
				actionQ.Enqueue(new EnterAction(mm.Player,mm.DoctorsHouse, "Head outside to start your journey to Merami"));
			//Player must ENTER(Forest)
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 42, 1));
				isBusy = true;
				}
				break;
			//Fourth mission begins
				//All EnterAction places need EnterScripts
			case MissionManager.EventType.ENTER_CITY:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player, mm.CityEntrance, "You did it!  You made it all the way to Merami on your own!"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_MT1:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player,mm.nearInjuredPerson, "That person looks hurt.  You should see if they need help"));
				//Player and Townsperson must TALK(audio, guiToShow)
				actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 43, 2));
				actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 45, 1));
				actionQ.Enqueue(new EnterAction(mm.Player,mm.TavernEntrance, "Search for the tavern.  It should be nearby.  Look for the sign with a mug"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_TAVERN:
				if(mission.getCurrentMissionEvent()==eventType) {
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.insideTavern, "There's the tavern.  They should have the supplies and healing water you need."));
				isBusy = true;
				}
				break;
//			case MissionManager.EventType.GATHER_SELF_SUPPLIES:
				//if(mission.getMissionType==eventType) {
//				actionQ.Enqueue(new EnterAction(mm.Player,mm.nearTavernKeeper, "Talk to the tavern keeper about your supplies and the water for the townsperson"));
//				isBusy = true;
				//}
//				break;
//			case MissionManager.EventType.GATHER_MT1_WATER:
				//if(mission.getMissionType==eventType) {
//				isBusy = true;
				//}
//				break;
//			case MissionManager.EventType.NEEDS_MEDICINE:
				//if(mission.getMissionType==eventType) {
//				isBusy = true;
				//}
//				break;
//			case MissionManager.EventType.GIVE_MEDICINE:
				//if(mission.getMissionType==eventType) {
//				isBusy = true;
				//}
//				break;
			case MissionManager.EventType.FINISH_TALKING_MT1:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 46, 1));
				if (mm.InjuredPerson.GetComponent<injuredPerson>().needsMedicine)
					actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 47, 1));
				else
					actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 48, 6));
				isBusy = true;
				}
				break;

			//Fifth Mission Events
			case MissionManager.EventType.GO_TO_MT2_HOUSE:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 54, 1));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CONFRONT_MT2:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideSonHouse, ""));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI,55,1));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideSonHouse, "Enter the house by pressing E"));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI, 56, 1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 57, 1));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio, mm.currentUI, 58, 4));
				//Player then exits the house
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingSonsHouse, "Leave the house to go help the townspeople"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.OFF_TO_HELP_PEOPLE:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 62, 1));
				isBusy = true;
				}
				break;

			//Sixth Mission Events
			case MissionManager.EventType.ENTER_MT3_HOUSE:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue (new ActiveAction(mm.currentUI, true, 63, 1));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideMT3House, "Enter the house by pressing E"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,64,1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 65, 1));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,66,2));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.GoingToBlacksmith, "Go to the blacksmith to get tools"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GO_TO_BLACKSMITH:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 68,1));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.AtBlacksmith, "Enter the blacksmith's house"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_BLACKSMITH:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,69,1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 70, 1));
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,71,2));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_1:
				if(mission.getCurrentMissionEvent()==eventType) {
				bool puzzle1done = false;
				while (!puzzle1done) {
					//Insert puzzle #1
					puzzle1done = true;
				}
				actionQ.Enqueue(new TalkAction(mm.Blacksmith, currentAudio,mm.currentUI,73,1));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_2:
				if(mission.getCurrentMissionEvent()==eventType) {
				bool puzzle2done = false;
				while (!puzzle2done) {
					//Insert puzzle #2
					puzzle2done = true;
				}
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,74,1));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_3:
				if(mission.getCurrentMissionEvent()==eventType) {
				bool puzzle3done = false;
				while (!puzzle3done) {
					//Insert puzzle #1
					puzzle3done = true;
				}
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,75,1));
					actionQ.Enqueue(new ActiveAction(mm.Tools,true));
					actionQ.Enqueue(new GrabAction(mm.Player,GrabMe.kind.TOOLS, "Grab the tools with G"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.RETURN_TO_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingBlacksmith, "Return to the old man's house with your new tools"));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 76, 1));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.BackWithTools, ""));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.FINISH_TALKING_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,77,1));
				actionQ.Enqueue(new GiveAction(mm.Player,mm.MT3, GrabMe.kind.TOOLS));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,78,1));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideMT3House, "Go outside to help the other townsperson"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_FT1_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 79,1));
					actionQ.Enqueue(new EnterAction(mm.Player, mm.AtFT1House, "Head past the blacksmith's place to get to the house"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 80,1));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideFT1House, "Enter the townsperson's house"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_FT1:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,81,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,82,1));
					actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,83,2));
					//actionQ.Enqueue(new ActiveAction(mm.Letters,true));
					actionQ.Enqueue(new GrabAction(mm.Player,GrabMe.kind.LETTERS, "Grab the letters with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.EXIT_FT1_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType) {
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideFT1House, "Go to the tavern to deliver the letters"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,85,1));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.THUNDER_BEGINS:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.Thunder,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,86,1));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.RAIN_STARTS:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,87,1));
					actionQ.Enqueue(new ActiveAction(mm.turnBack,true));
					actionQ.Enqueue(new ActiveAction(mm.moveOn,true));
					EnterScript[] triggers = new EnterScript[2];
					triggers[0] = mm.turnBack.GetComponent<EnterScript>();
					triggers[1] = mm.moveOn.GetComponent<EnterScript>();
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Trigger",triggers,"Rain",2, false, "Will you keep going toward the tavern or turn back?"));
					//if (mm.choiceInRain==0){
					//	mission.setEventComplete(MissionManager.EventType.TURN_BACK);
					//	mission.setEventComplete(MissionManager.EventType.RETURN_TO_FT1);
					//	Debug.Log("Missions set complete mo");
					//}
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TURN_BACK:
				if (mission.getCurrentMissionEvent()==eventType) {
					//if (mm.choiceInRain==0) {
						actionQ.Enqueue(new TalkAction(mm.Torkana,currentAudio,mm.currentUI,88,1));
						EnterScript[] triggers = new EnterScript[2];
						triggers[0] = mm.shelterAtFT1.GetComponent<EnterScript>();
						triggers[1] = mm.moveOn.GetComponent<EnterScript>();
						actionQ.Enqueue(new ChoiceAction(mm.Player,"Trigger",triggers,"Rain",2, true, "Do you listen to Torkana or keep heading back?"));
					//if (mm.choiceInRain==0){
					//	mission.setEventComplete(MissionManager.EventType.DELIVER_LETTERS);
					//	mission.setEventComplete(MissionManager.EventType.WAIT_FOR_DRIZZLE);
					//	mission.setEventComplete(MissionManager.EventType.WAIT_FOR_END);
					//	Debug.Log("Missions set complete tb");
					//}
						isBusy = true;
					//}
					//else{
					//	isComplete = true;
						//actionQ.Enqueue(new NullAction());
						//isBusy = true;
					//}
				}
				break;
			case MissionManager.EventType.RETURN_TO_FT1:
				if (mission.getCurrentMissionEvent()==eventType) {
					//if (mm.choiceInRain==0) {
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.backAtFT1, ""));
						actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,89,1));
						actionQ.Enqueue(new GiveAction(mm.Player,mm.FT1, GrabMe.kind.LETTERS));
						actionQ.Enqueue(new ActiveAction(mm.Thunder,false));
						actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,95,1));
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingHouseAgain, "Step outside and return to the foreman's son"));
						isBusy = true;
					//}
					//else{
					//	isComplete = true;
						//actionQ.Enqueue(new NullAction());
						//isBusy = true;
					//}
				}
				break;
			case MissionManager.EventType.DELIVER_LETTERS:
				if (mission.getCurrentMissionEvent()==eventType) {
					//if (mm.choiceInRain==1) {
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.safeInTavern, "Get into the tavern quickly"));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,90,1));
						actionQ.Enqueue(new TalkAction(mm.Player,currentAudio,mm.currentUI,91,1));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,92,1));
						actionQ.Enqueue(new GiveAction(mm.Player,mm.TavernKeeper, GrabMe.kind.LETTERS));
						actionQ.Enqueue(new ActiveAction(mm.Thunder,false));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,93,1));
						actionQ.Enqueue(new ChoiceAction(mm.Player,"Button",new EnterScript[1],"TavernAfter",2,false, "Press 1 to leave now, or press 2 to wait until the rain completely stops"));
					if (mm.choiceInRain==0){
						mission.setEventComplete(MissionManager.EventType.WAIT_FOR_END);
						Debug.Log("Missions set complete wd");
					}
						isBusy = true;
					//}
					//else{
					//	isComplete = true;
						//actionQ.Enqueue(new NullAction());
						//isBusy = true;
					//}
				}
				break;
			case MissionManager.EventType.WAIT_FOR_DRIZZLE:
				if (mission.getCurrentMissionEvent()==eventType) {
					//if (mm.choiceInTavern==0) {
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingTavernAgain, "Step back outside and return to the foreman's son"));
						isBusy = true;
					//}
					//else{
					//	isComplete = true;
						//actionQ.Enqueue(new NullAction());
						//isBusy = true;
					//}
				}
				break;
			case MissionManager.EventType.WAIT_FOR_END:
				if (mission.getCurrentMissionEvent()==eventType) {
					//if (mm.choiceInTavern==1) {
						actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,94,1));
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingTavernAgain, "Step back outside and return to the foreman's son"));
						isBusy = true;
					//}
					//else{
					//	isComplete = true;
						//actionQ.Enqueue(new NullAction());
						//isBusy = true;
					//}
				}
				break;
			case MissionManager.EventType.HEAD_BACK_MT2:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new EnterAction(mm.Player,mm.hearGirlCrying, "Return to the foreman's son to tell him about your deeds"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,96,1));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearGirl, "Investigate the sounds of crying"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_FT2:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,97,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,98,2));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_FT2_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,100,1));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.insideGirlHouse, "Look around the girl's house to find examples of her parent's love"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_DRAWINGS:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,101,1));
					//Shut off the rain if still on
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
					actionQ.Enqueue(new EnterAction(mm.Player, mm.nearDrawings, "Search for something the girl made"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,102,1));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.DRAWINGS, "Pick up the drawings with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SHOW_DRAWINGS:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.drawingExit, "Show the girl the drawings"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,103,2));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,105,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true,106,1));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.inside2, "Go back inside to find something else"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_PICTURES:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,107,1));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearPictures, "Search for something more permanent"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,108,1));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.PICTURES, "Pick up the pictures with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SHOW_PICTURES:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.pictureExit, "Show the girl the pictures"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,109,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl, currentAudio,mm.currentUI,110,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true,111,1));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.inside3, "Go inside one more time to search"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_LOCKET:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,112,1));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearLocket, "Search for one last important item"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,113,1));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.LOCKET, "Pick up the locket with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_LOCKET:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.locketExit, "Show the girl the locket"));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,114,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl, currentAudio,mm.currentUI,115,1));
					//Parents come running in
					actionQ.Enqueue(new MoveAction(mm.GirlsFather,mm.fatherStop, AnimationEngine.Type.RUN));
					actionQ.Enqueue(new MoveAction(mm.GirlsMother,mm.motherStop, AnimationEngine.Type.RUN));
					actionQ.Enqueue(new StandAction(mm.YoungGirl,mm.YoungGirlStand));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,116,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,117,1));
					isBusy = true;
				}
				break;
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
	//		Debug.Log ("we have at least one action in the queue!");
	//		Debug.Log ("there are this many actions in queue: " + actionQ.Count);
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
	//		Debug.Log ("Executing mission actions for this event");
//			if (actionQ.Count == 0 && isComplete){
//				Debug.Log ("event is complete...");
//				currAction = null;
//			}
			//Action runs its own loop until it's completed
			//then execute will return true if successfully completed
			//DEBUG: Skip current action if PageUp is pressed.
			if(currAction.execute() || Input.GetKeyUp (KeyCode.PageUp)){
		    //return currAction.execute();
				if (actionQ.Count > 0){
					//Debug.Log("Dequeueing next action, last was completed");
					currAction = actionQ.Dequeue();
					isBusy = true;
				} else {
					currAction = null;
					//Debug.Log ("last action was completed, now event is complete...");
					return true;
				}
				//return true;
		     } else { //Otherwise, we can't continue the story.
//			    //may want to change this later to keep trying action
	//			Debug.Log ("Still executing action..please hold");
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
