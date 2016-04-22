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

	public void resetEvent(){
		isComplete = false;
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
					actionQ.Enqueue(new SkyboxAction(mm.sunny)); //Just to make sure the sky doesn't randomly stay black or anything.
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				//Gui must ACTIVE(true, brief)
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 0, 3));
			    //Torkana must MOVE(currLoc, adjToPlayer, limp)
				actionQ.Enqueue (new MoveAction (mm.Torkana, mm.Player, AnimationEngine.Type.WALK));  
			    //Torkana must TALK(audio, guiToShow)
			    actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 3, 2));
				//Torkana must TURN(faceHouse)
				actionQ.Enqueue(new TurnAction(mm.Torkana, mm.TorkanaHouse, false, 0));
				//Torkana must MOVE(currLoc, adjToHouse)
				actionQ.Enqueue(new MoveAction(mm.Torkana, mm.TorkanaHouse, AnimationEngine.Type.WALK));
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
					//Darken the room
					actionQ.Enqueue(new SkyboxAction(true));
					actionQ.Enqueue(new ActiveAction(mm.Sun,false)); //Turn off the sun
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
					//Brighten again as you go up.
					actionQ.Enqueue(new ActiveAction(mm.Sun,true)); //Turn the sun back on.
					actionQ.Enqueue(new SkyboxAction(false));
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
					//Set minimap to point to Torkana
					actionQ.Enqueue(new StandAction(mm.Torkana,mm.inFrontTorkanaHouse)); //Allows for Torkana reset upon mission failure
					actionQ.Enqueue(new MinimapAction("Torkana"));
				actionQ.Enqueue(new FollowAction(0, 13, mm.Torkana));
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
					//Set minimap to point to the map.  NOTE: if we plan on disabling the minimap for this section, this line can be deleted.
					actionQ.Enqueue(new MinimapAction("Map"));
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
					EnterScript[] triggers = new EnterScript[2];
					triggers[0] = mm.continueBees.GetComponent<EnterScript>();
					triggers[1] = mm.turnBackBees.GetComponent<EnterScript>();
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Trigger",triggers,"Bees1",2,false,"Face your fears!  Tolerate the bees and get your map back."));
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
					actionQ.Enqueue(new MinimapAction("Torkana"));
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
					//Resets map to point to Doctor's House
					actionQ.Enqueue(new MinimapAction("Torkana"));
			//Torkana must MOVE(currLoc, adjToDoctorsHouse) iff IN_RANGE(Torkana, Player)
			//Player must MOVE(currLoc, adjToDoctorsHouse)
			//note that Torkana moves to the doctor's house so the player also must
				actionQ.Enqueue(new FollowAction(14, 29, mm.Torkana));
			
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
					//Turn off pointer indoors
					actionQ.Enqueue(new MinimapAction("nothing"));
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
					//Point to the garden
					actionQ.Enqueue(new MinimapAction("Garden"));
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
					EnterScript[] triggers = new EnterScript[2];
					triggers[0] = mm.continueHerb.GetComponent<EnterScript>();
					triggers[1] = mm.turnBackHerb.GetComponent<EnterScript>();
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Trigger",triggers,"Bees2",2,false,"Tolerate the bees again to grab the herb you need."));
				//Player must INTERACT(Gui, Bees, Action) and Bees must REACT(Player, Action) and
				//BEES must APPROVE(Action) 
				//actionQ.Enqueue(new PrintAction("Hold C while you move for courage\r\n" +
				//                                "Hold E while you move for compassion\r\n" +
				//                                "Hold Q while you move for health\r\n", 20));

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
					actionQ.Enqueue(new MinimapAction("nothing"));//Player must FREEZE
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
					//Turn sky dark again
					actionQ.Enqueue(new SkyboxAction(mm.darkness));
					//Also darken cave?
					actionQ.Enqueue(new SkyboxAction(true));
					//Turn off bees when player enters cave
					//actionQ.Enqueue(new MoveAction(mm.Bees, mm.TorkanaEnterDoctors)); <- so no changes have to be made when mission is restarted.
					//May need to change swarm focus, though.
					actionQ.Enqueue(new ActiveAction(mm.Bees, false));
				//Player must FREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 30, 1));
				//Player must UNFREEZE
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.BlockedPath, "Head deeper into the cave to find the healing spring"));
					actionQ.Enqueue(new FreezeAction(mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 120, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player, false));
				actionQ.Enqueue(new PressAction(mm.CaveSwitch));
				actionQ.Enqueue (new ActiveAction(mm.CaveGateClosed, false));
				actionQ.Enqueue (new ActiveAction(mm.CaveGateOpened, true));
					actionQ.Enqueue(new FreezeAction(mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,121,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.HEALING_WATER,"Jump into the water and grab some healing water with G"));
				//player must find the health potion to get out alive
				/*if(mm.Player.GetComponent<CharacterOurs>().health mm.Player.GetComponent<PlayerStatusBars>().Health() < 50){
					actionQ.Enqueue (new FreezeAction (mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 31, 1));
					actionQ.Enqueue (new FreezeAction (mm.Player, false));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.HEALTH_POTION, "Grab the health potion with G"));
					actionQ.Enqueue (new FreezeAction (mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 32, 1));
					actionQ.Enqueue (new FreezeAction (mm.Player, false));
					actionQ.Enqueue(new ApplyAction(mm.Player, mm.Player, GrabMe.kind.HEALTH_POTION));
				}*/  //completely unnecessary
				actionQ.Enqueue (new FreezeAction (mm.Player, true));
				actionQ.Enqueue (new ActiveAction (mm.currentUI, true, 33, 1));
				actionQ.Enqueue (new FreezeAction (mm.Player, false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				//player must exit healing cave
				actionQ.Enqueue(new EnterAction(mm.Player, mm.HealingCaveExit, "Press E near the cave entrance to exit"));
					//Turn the sun back on.
					actionQ.Enqueue(new SkyboxAction(mm.sunny));
					actionQ.Enqueue(new SkyboxAction(false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_DOCTOR_HERBS:
				if(mission.getCurrentMissionEvent()==eventType) {
				//Player must MOVE(currLoc, adjToDoctor)
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player, mm.BackToDoctors, "Give the herbs and water to the doctor"));
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
					actionQ.Enqueue(new ActiveAction(mm.Amulet,true));
				//actionQ.Enqueue(new GiveAction(mm.Torkana, mm.Player, GrabMe.kind.AMULET));
				actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.AMULET, "Grab the amulet by pressing G"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.Torkana, currentAudio, mm.currentUI, 41, 1));
					actionQ.Enqueue(new TalkAction(mm.Doctor,currentAudio,mm.currentUI,127,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
			case MissionManager.EventType.LEAVE_FOREST:
				if(mission.getCurrentMissionEvent()==eventType) {
			//Player must MOVE(currLoc, adjToDoor)
					actionQ.Enqueue(new FreezeAction(mm.Player,false)); //Failsafe to make sure player can move if mission gets reset due to failure later on
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.LeavingDoctor, "Head outside to start your journey to Merami"));
			//Player must ENTER(Forest)
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 42, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Point to the city
					actionQ.Enqueue(new MinimapAction("City"));
				isBusy = true;
				}
				break;
				//Mission 4 Actions
				//All EnterAction places need EnterScripts
			case MissionManager.EventType.ENTER_CITY:
				if(mission.getCurrentMissionEvent()==eventType) {
					//Reset minimap to point at nothing
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,138,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_MT1:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player,mm.nearInjuredPerson, "That person looks hurt.  You should see if they need help"));
				//Player and Townsperson must TALK(audio, guiToShow)
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 43, 2));
				actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 45, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Point to the tavern
					actionQ.Enqueue(new MinimapAction("Tavern"));
				actionQ.Enqueue(new EnterAction(mm.Player,mm.TavernEntrance, "Search for the tavern.  It should be nearby.  Look for the sign with a mug"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_TAVERN:
				if(mission.getCurrentMissionEvent()==eventType) {
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.insideTavern, "There's the tavern.  They should have the supplies and healing water you need."));
					//Turn off the minimp pointer indoors
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearTavernKeeper, "Talk to the tavern keeper about your supplies and the water for the townsperson"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GET_SUPPLIES:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,122,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,123,1));
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Button",new EnterScript[1],"TavernSupply",3,false,
					                                 "Press 1 to ask for the healing water first.  Press 2 to ask for both the water and your supplies.  Press 3 to ask for the supplies first and the water afterward."));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.NEEDS_MEDICINE:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,124,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.ReturningWithMedicine, "Hurry outside to give the healing water to the injured man."));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI,46,2));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,126,1));
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Button",new EnterScript[1],"Water",2,false,"Press 1 to give the man your healing water, or press 2 to keep it for yourself."));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_MEDICINE:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Torkana,currentAudio,mm.currentUI,128,1));
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Button",new EnterScript[1],"Water2",2,true,"Press 1 to listen to Torkana and give the man your healing water, or press 2 to keep the water to yourself."));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.FINISH_IN_TAVERN:
				if(mission.getCurrentMissionEvent()==eventType) {
					if(mm.choiceForSupply == 0){
						actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,130,1));
						actionQ.Enqueue(new FreezeAction(mm.Player,false));
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.ReturningWithMedicine, "Hurry outside to give the healing water to the injured man."));
					}
					else if(mm.choiceForSupply == 1){
						actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,132,1));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,133,1));
						actionQ.Enqueue(new FreezeAction(mm.Player,false));
						mm.Player.GetComponent<CharacterOurs>().canEnter = true;
						actionQ.Enqueue(new EnterAction(mm.Player,mm.ReturningWithMedicine, "Hurry outside to give the healing water to the injured man."));
					}
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FINISH_TALKING_MT1:
				if(mission.getCurrentMissionEvent()==eventType) {
					if (mm.choiceForSupply == 0 || mm.choiceForSupply == 1){
						actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI, 46, 1));
						actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI,48,1));
					}
					else if (mm.choiceForSupply == 2){
						actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI,131,1));
					}
					actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI,49,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,50,1));
					actionQ.Enqueue(new TalkAction(mm.InjuredPerson,currentAudio,mm.currentUI,51,3));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GATHER_SELF_SUPPLIES:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,129,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.ReturnToTavern, "Go back inside to get your supplies"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,134,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,135,1));
					actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,136,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,137,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.toTheSon, ""));
					isBusy = true;
				}
				break;

			//Fifth Mission Events
			case MissionManager.EventType.GO_TO_MT2_HOUSE:
				if(mission.getCurrentMissionEvent()==eventType) {
					//Point to the Son's house
					actionQ.Enqueue(new MinimapAction("Son"));
					//Freeze
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 54, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CONFRONT_MT2:
				if(mission.getCurrentMissionEvent()==eventType) {
				actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideSonHouse, ""));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI,55,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideSonHouse, "Enter the house by pressing E"));
					//Turn off pointer indoors
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI, 56, 1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 57, 1));
				actionQ.Enqueue(new TalkAction(mm.Son,currentAudio, mm.currentUI, 58, 4));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				//Player then exits the house
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingSonsHouse, "Leave the house to go help the townspeople"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.OFF_TO_HELP_PEOPLE:
				if(mission.getCurrentMissionEvent()==eventType) {
					//Point to the old man
					actionQ.Enqueue(new MinimapAction("MT3"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 62, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;

			//Sixth Mission Events
			case MissionManager.EventType.ENTER_MT3_HOUSE:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue (new ActiveAction(mm.currentUI, true, 63, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideMT3House, "Enter the house by pressing E"));
					//Reset pointer
					actionQ.Enqueue(new MinimapAction("nothing"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,64,1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 65, 1));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,66,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.GoingToBlacksmith, "Go to the blacksmith to get tools"));
					//Point to the blacksmith
					actionQ.Enqueue(new MinimapAction("Blacksmith"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.GO_TO_BLACKSMITH:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 68,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.AtBlacksmith, "Enter the blacksmith's house"));
					actionQ.Enqueue(new MinimapAction("nothing"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_BLACKSMITH:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,69,1));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 70, 1));
				actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,71,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_1:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new CameraAction(true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,false));
					actionQ.Enqueue(new BlacksmithPuzzleAction(mm.Grid, 3, mm.firstPic));
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,false));
					actionQ.Enqueue(new PuzzleClearAction(mm.Grid));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true));
					actionQ.Enqueue(new CameraAction(false));
					actionQ.Enqueue(new TalkAction(mm.Blacksmith, currentAudio,mm.currentUI,73,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_2:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new CameraAction(true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,false));
					actionQ.Enqueue(new BlacksmithPuzzleAction(mm.Grid, 4, mm.secondPic));
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,false));
					actionQ.Enqueue(new PuzzleClearAction(mm.Grid));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true));
					actionQ.Enqueue(new CameraAction(false));
					actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,74,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.CREATE_TOOL_3:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new CameraAction(true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,false));
					actionQ.Enqueue(new BlacksmithPuzzleAction(mm.Grid, 5, mm.thirdPic));
					actionQ.Enqueue(new ActiveAction(mm.PuzzleGridCanvas,false));
					actionQ.Enqueue(new PuzzleClearAction(mm.Grid));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true));
					actionQ.Enqueue(new CameraAction(false));
					actionQ.Enqueue(new TalkAction(mm.Blacksmith,currentAudio,mm.currentUI,75,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new ActiveAction(mm.Tools,true));
					actionQ.Enqueue(new GrabAction(mm.Player,GrabMe.kind.TOOLS, "Grab the tools with G"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.RETURN_TO_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.leavingBlacksmith, "Return to the old man's house with your new tools"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 76, 1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.BackWithTools, ""));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.FINISH_TALKING_MT3:
				if(mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,77,1));
				actionQ.Enqueue(new GiveAction(mm.Player,mm.MT3, GrabMe.kind.TOOLS));
				actionQ.Enqueue(new TalkAction(mm.MT3,currentAudio,mm.currentUI,78,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
				mm.Player.GetComponent<CharacterOurs>().canEnter = true;
				actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideMT3House, "Go outside to help the other townsperson"));
				isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_FT1_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new MinimapAction("FT1"));
					actionQ.Enqueue(new FreezeAction(mm.Player, true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 79,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player, mm.AtFT1House, "Head past the blacksmith's place to get to the house"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true, 80,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.InsideFT1House, "Enter the townsperson's house"));
					actionQ.Enqueue(new MinimapAction("nothing"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_FT1:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,81,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,82,1));
					actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,83,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//actionQ.Enqueue(new ActiveAction(mm.Letters,true));
					actionQ.Enqueue(new GrabAction(mm.Player,GrabMe.kind.LETTERS, "Grab the letters with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.EXIT_FT1_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType) {
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.OutsideFT1House, "Go to the tavern to deliver the letters"));
					//Bring in the clouds
					actionQ.Enqueue(new SkyboxAction(mm.rainy));
					//Point at the old man's house again, because otherwise the player doesn't pass the triggers.
					actionQ.Enqueue(new MinimapAction("MT3"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,85,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.THUNDER_BEGINS:
				if (mission.getCurrentMissionEvent()==eventType) {
					actionQ.Enqueue(new ActiveAction(mm.Thunder,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,86,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.RAIN_STARTS:
				if (mission.getCurrentMissionEvent()==eventType) {
					//NOW point at the tavern, because the important triggers are passed
					actionQ.Enqueue(new MinimapAction("Tavern"));
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,87,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.Torkana,currentAudio,mm.currentUI,88,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.FT1,currentAudio,mm.currentUI,89,1));
						actionQ.Enqueue(new GiveAction(mm.Player,mm.FT1, GrabMe.kind.LETTERS));
						actionQ.Enqueue(new ActiveAction(mm.Thunder,false));
						actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,95,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,90,1));
						actionQ.Enqueue(new TalkAction(mm.Player,currentAudio,mm.currentUI,91,1));
						actionQ.Enqueue(new TalkAction(mm.TavernKeeper,currentAudio,mm.currentUI,92,1));
						actionQ.Enqueue(new GiveAction(mm.Player,mm.TavernKeeper, GrabMe.kind.LETTERS));
						actionQ.Enqueue(new ActiveAction(mm.Thunder,false));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,93,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
					//Make it less cloudy
					actionQ.Enqueue(new SkyboxAction(mm.sunny));
						actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
						actionQ.Enqueue(new ActiveAction(mm.currentUI,true,94,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
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
				//Mission 8 events
			case MissionManager.EventType.HEAD_BACK_MT2:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new MinimapAction("Son"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.hearGirlCrying, "Return to the foreman's son to tell him about your deeds"));
					actionQ.Enqueue(new ActiveAction(mm.CryingSound,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,96,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Point to the girl's house
					actionQ.Enqueue(new MinimapAction("Girl"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearGirl, "Investigate the sounds of crying"));
					actionQ.Enqueue(new MinimapAction("nothing"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_FT2:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,97,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,98,2));

					isBusy = true;
				}
				break;
			case MissionManager.EventType.ENTER_FT2_HOUSE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,100,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.insideGirlHouse, "Look around the girl's house to find examples of her parent's love"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_DRAWINGS:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,101,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Shut off the rain if still on
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
					//Also make the clouds go away
					actionQ.Enqueue(new SkyboxAction(mm.sunny));
					actionQ.Enqueue(new EnterAction(mm.Player, mm.nearDrawings, "Search for something the girl made"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,102,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.DRAWINGS, "Pick up the drawings with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SHOW_DRAWINGS:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.drawingExit, "Show the girl the drawings"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,103,2));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,105,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true,106,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.inside2, "Go back inside to find something else"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_PICTURES:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,107,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearPictures, "Search for something more permanent"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,108,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.PICTURES, "Pick up the pictures with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SHOW_PICTURES:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.pictureExit, "Show the girl the pictures"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,109,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl, currentAudio,mm.currentUI,110,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI, true,111,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.inside3, "Go inside one more time to search"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_LOCKET:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,112,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearLocket, "Search for one last important item"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,113,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new GrabAction(mm.Player, GrabMe.kind.LOCKET, "Pick up the locket with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_LOCKET:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.locketExit, "Show the girl the locket"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,114,1));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl, currentAudio,mm.currentUI,115,1));
					//Parents come running in
					actionQ.Enqueue(new ActiveAction(mm.GirlsFather,true));
					actionQ.Enqueue(new ActiveAction(mm.GirlsMother,true));
					actionQ.Enqueue(new MoveAction(mm.GirlsFather,mm.fatherStop, AnimationEngine.Type.RUN));
					actionQ.Enqueue(new MoveAction(mm.GirlsMother,mm.motherStop, AnimationEngine.Type.RUN));
					actionQ.Enqueue(new StandAction(mm.YoungGirl,mm.YoungGirlStand));
					actionQ.Enqueue(new TalkAction(mm.YoungGirl,currentAudio,mm.currentUI,116,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,117,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false)); //May need to move depending on how the running works
					isBusy = true;
				}
				break;
			case MissionManager.EventType.RETURN_TO_SON:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new MinimapAction("Son"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.BackFromDeeds, "Return to the son's house"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_SON:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI,118,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.OnwardToCyclops, "Head outside to find and defeat the cyclops"));
					isBusy = true;
				}
				break;
			//Mission 9 Events
			case MissionManager.EventType.TOWARD_TOWN_CENTER:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.Cyclops,true));
					actionQ.Enqueue(new MinimapAction("Town Center"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearAppleLady,"Head towards the town center and see if you can get any information on the cyclops"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_FT3:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,139,1));
					actionQ.Enqueue(new TalkAction(mm.AppleLady,currentAudio,mm.currentUI,140,3));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,143,1));
					actionQ.Enqueue(new TalkAction(mm.AppleLady,currentAudio,mm.currentUI,144,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_MT4:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearKid,"Continue towards the town center."));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,145,1));
					actionQ.Enqueue(new TalkAction(mm.Kid,currentAudio,mm.currentUI,146,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,147,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.HEAR_LAUGHING:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new EnterAction(mm.Player,mm.hearLaugh,"Continue towards the town center."));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					//start laughter here
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,148,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearGuard,"Go and confront the cyclops."));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TALK_TO_GUARD:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,149,1));
					actionQ.Enqueue(new TalkAction(mm.Guard,currentAudio,mm.currentUI,150,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearCyclops,"Talk to the cyclops and see if you can get him to stop"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.GIVE_CYCLOPS_APPLES:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					//stop laughter here
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,151,1));
					actionQ.Enqueue(new TalkAction(mm.Cyclops,currentAudio,mm.currentUI,152,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,153,1));
					actionQ.Enqueue(new PrintAction("Press 1 to suggest that the cyclops become a guard.  Press 2 to suggest that the cyclops stop attacking the stands.  Press 3 to suggest that the cyclops keep attacking.",100));
					actionQ.Enqueue(new SetAction(mm.Cyclops.GetComponent<CyclopsChoiceLoop>().isChoosing,true));
					actionQ.Enqueue(new WaitAction(mm.cyclopsChoice));
					actionQ.Enqueue(new TalkAction(mm.Cyclops,currentAudio,mm.currentUI,158,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,159,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.GET_SEWER_OPEN:
				if (mission.getCurrentMissionEvent()==eventType){
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new MinimapAction("Son"));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.finalSonStop,"Return to the sewer foreman's son to get the sewer opened."));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI,161,3));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;

			//Mission 10 events
			case MissionManager.EventType.ENTER_SEWERS:
				if(mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Son,currentAudio,mm.currentUI,164,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.sewerEntrance,"Find the entrance to the sewers"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TOLERATE_SPIDERS:
				if(mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new ActiveAction(mm.Spiders1,true)); //Turn on spiders
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,165,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.keySpied,"Search the nearby rooms for the key"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,166,1));
					actionQ.Enqueue(new PrintAction("Press 1 to attack the spiders.  Press 2 to wait for the spiders to leave.  Press 3 to lure the spiders away with food.",100));
					actionQ.Enqueue(new SetAction(mm.Spiders1.GetComponent<SpiderLoopA>().isChoosing,true));
					actionQ.Enqueue(new WaitAction(mm.choiceSewerSpiders));
					actionQ.Enqueue(new MoveAction(mm.Spiders1,mm.spiderMovePoint));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,169,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new GrabAction(mm.Player,GrabMe.kind.SEWER_KEY,"Pick up the sewer key with G"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.OPEN_DOOR:
				if(mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,170,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new ActiveAction(mm.openDoor,true));
					actionQ.Enqueue(new ActiveAction(mm.closedDoor,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.officeDoor,"Enter the office and find the switch that opens the gate"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_NOTE:
				if(mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,171,1));
					actionQ.Enqueue(new TalkAction(mm.Foreman,currentAudio,mm.currentUI,172,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,173,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.ACTIVATE_LEVER:
				if(mission.getCurrentMissionEvent()==eventType){
					//Lever activating action
					actionQ.Enqueue(new EnterAction(mm.Player,mm.leverTrigger,"Pull the lever to open the door to the rest of the sewers"));
					//Lever moves
					actionQ.Enqueue(new ActiveAction(mm.openLever,false));
					actionQ.Enqueue(new ActiveAction(mm.thrownLever,true));
					actionQ.Enqueue(new ActiveAction(mm.hydraDoor,false)); //Open hydra arena
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,174,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Player heads to the front of the hydra area
					actionQ.Enqueue(new ActiveAction(mm.Hydra,true));
					actionQ.Enqueue(new FreezeAction(mm.Hydra,true));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.entranceToArena,"Continue on to find the foreman"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.HYDRA_FAIL:
				if(mission.getCurrentMissionEvent()==eventType){
					//Hydra spotted.  Make sure it isn't attacking yet.
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,175,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new FreezeAction(mm.Hydra,false));
					//Activate hydra and wait for player to get hit once by a negative thought.
					actionQ.Enqueue(new WaitAction(mm.hydraHit1));
					//Freeze hydra as well as Player.
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new FreezeAction(mm.Hydra,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,176,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new FreezeAction(mm.Hydra,false));
					//Wait for second negative thought hit.
					actionQ.Enqueue(new WaitAction(mm.hydraHit2));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new FreezeAction(mm.Hydra,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,177,1));
					//Player "faints"
					//Teleport player to dead end near arena, but don't end mission.
					actionQ.Enqueue(new TransportAction(mm.Player,mm.hydraTransport));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.COMPLIMENT_SCROLLS:
				if(mission.getCurrentMissionEvent()==eventType){
					//Torkana speaks to the player.
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Torkana,currentAudio,mm.currentUI,178,4));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Maybe some extra sequence?  Probably need to add in a fancy "You got a Compliment Scroll!" message after Cyclops and Townsperson missions
					actionQ.Enqueue(new EnterAction(mm.Player,mm.facingTheHydra,"Face your fears!  Defeat the hydra!"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.HYDRA_FIGHT:
				if(mission.getCurrentMissionEvent()==eventType){
					//Begin the fight!  Turn hydra's attack back on.
					actionQ.Enqueue(new FreezeAction(mm.Hydra,false));
					actionQ.Enqueue(new PrintAction("Press 1: 'That was smart of you to use apples to appease the cyclops!'  " +
					                                "Press 2: 'The townspeople told me they are extremely grateful for all your help! They really like you!'", 100));
					actionQ.Enqueue(new WaitAction(mm.hydraDefeated));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Torkana,currentAudio,mm.currentUI,182,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.FIND_FOREMAN:
				if (mission.getCurrentMissionEvent()==eventType){
					//Player heads off to find foreman
					actionQ.Enqueue(new EnterAction(mm.Player,mm.nearForeman, "Search for the foreman.  He should be nearby somewhere."));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Foreman,currentAudio,mm.currentUI,183,1));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,184,1));
					actionQ.Enqueue(new TalkAction(mm.Foreman,currentAudio,mm.currentUI,185,3));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new MoveAction(mm.Foreman,mm.ForemanStand));
					actionQ.Enqueue(new TransportAction(mm.Player,mm.ForemanHouseTransport));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.REUNION_WITH_SON:
				if (mission.getCurrentMissionEvent()==eventType){
					//At this point, player and foreman are in the foreman's house
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,188,1));
					actionQ.Enqueue(new TalkAction(mm.Foreman,currentAudio,mm.currentUI,189,3));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Player steps outside
					actionQ.Enqueue(new SkyboxAction(mm.night));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.toTheBoathouse,"Go to the boathouse to talk to the troll"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.GO_TO_TROLL:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new MinimapAction("Boathouse"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,192,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.atBoathouse,"Head on to the boathouse"));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.CALM_TROLL:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new MinimapAction("nothing"));
					actionQ.Enqueue(new ActiveAction(mm.Sun,false)); //Turn off the sun
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,193,2));
					actionQ.Enqueue(new BreatherAction(mm.breather));
					actionQ.Enqueue(new ActiveAction(mm.breather.gameObject,false));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,195,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.RECEIVE_BOAT:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new TalkAction(mm.Troll,currentAudio,mm.currentUI,196,2));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,198,1));
					actionQ.Enqueue(new TalkAction(mm.Troll,currentAudio,mm.currentUI,199,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new ActiveAction(mm.Sun,true)); //Turn off the sun
					mm.Player.GetComponent<CharacterOurs>().canEnter = true;
					actionQ.Enqueue(new EnterAction(mm.Player,mm.atBoat,"Grab one of the boats outside"));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,201,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Player is moved out to sea a bit, with a boat if possible
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,202,1));
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,true));
					actionQ.Enqueue(new ChoiceAction(mm.Player,"Button",new EnterScript[1],"Boat",3,false, "Press 1 to continue on through the storm.  Press 2 to wait until the storm lessens.  Press 3 to search for a different way to the island."));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.BRAVE_STORM:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,203,1));
					//Player is moved to island
					actionQ.Enqueue(new MoveAction(mm.Player,mm.islandDock));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SCARED_BY_STORM:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,204,1));
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
					//Player is moved to island
					actionQ.Enqueue(new MoveAction(mm.Player,mm.islandDock));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TRY_ALTERNATE_ROUTE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,205,1));
					//Fadeout?
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,206,1));
					actionQ.Enqueue(new ActiveAction(mm.RainMaker,false));
					//Player is moved to island
					actionQ.Enqueue(new MoveAction(mm.Player,mm.islandDock));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.SPOT_OGRE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,207,1));
					//Point minimap at Dragon
					actionQ.Enqueue(new MinimapAction("Dragon"));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					actionQ.Enqueue(new EnterAction(mm.Player,mm.atOgre,""));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,208,1));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.CALM_OGRE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new BreatherAction(mm.breather2));
					actionQ.Enqueue(new ActiveAction(mm.breather2.gameObject,false));
					actionQ.Enqueue(new SleepAction(mm.Ogre));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,209,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.TOLERATE_SNAKES_AGAIN:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new ActiveAction(mm.Snakes,true)); //Turn on snakes
					actionQ.Enqueue(new EnterAction(mm.Player,mm.pathToDragon,"Continue forward to face the dragon!"));
					//Here there be snakes
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,210,2));
					actionQ.Enqueue(new PrintAction("Press 1 to attack the snakes.  Press 2 to wait for the snakes to leave.",100));
					actionQ.Enqueue(new SetAction(mm.Snakes.GetComponent<SnakeLoop>().isChoosing,true));
					actionQ.Enqueue(new WaitAction(mm.choiceSnakes));
					actionQ.Enqueue(new MoveAction(mm.Snakes,mm.snakeMovePoint));
					actionQ.Enqueue(new ActiveAction(mm.Snakes,false));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,212,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.CONFRONT_DRAGON:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new EnterAction(mm.Player,mm.atDragon,"Face the Dragon!"));
					//Make sure Dragon isn't moving.
					actionQ.Enqueue(new FreezeAction(mm.Dragon,true));
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,213,2));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.DRAGON_BATTLE:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Dragon,false));
					actionQ.Enqueue(new PrintAction("Press 1: 'That was smart of you to use apples to appease the cyclops!'  " +
					                                "Press 2: 'The townspeople told me they are extremely grateful for all your help! They really like you!'  " +
					                                "Press 3: 'You were great in battle! You fought so well.'  " +
					                                "Press 4: 'You are such a smart and quick thinker to use breathing as a form of relaxation!'", 100));
					actionQ.Enqueue(new WaitAction(mm.dragonDefeated));
					isBusy = true;
				}
				break;
			case MissionManager.EventType.DRAGON_DEFEATED:
				if (mission.getCurrentMissionEvent()==eventType){
					actionQ.Enqueue(new FreezeAction(mm.Player,true));
					actionQ.Enqueue(new ActiveAction(mm.currentUI,true,215,1));
					actionQ.Enqueue(new FreezeAction(mm.Player,false));
					//Any last-minute end-game stuffs
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
