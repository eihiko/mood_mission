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
	private IList eve

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

	void OnTriggerEnter(Collider c){
		switch (eventType){
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
		case MissionManager.EventType.ENTER_GUIDES:
			//Player must FREEZE
			//Torkana must TALK(audio, guiToShow)
		case MissionManager.EventType.CANDLE:
			//Gui must ACTIVE(true, brief)
			//Player must GRAB(Candle)
			//Player must GRAB(Key)
			//Gui must ACTIVE(true, brief)
			//Player must ENTER(mentorBasement)
		case MissionManager.EventType.ENTER_GUIDE_BASEMENT:
			//Player must DROP(Key)
			//Candle must ACTIVE(false)
			//Gui must ACTIVE(true, brief)
			//Player must FIND(Match) to light candle
			//Gui must ACTIVE(true, brief)
			//Player must OPEN(Chest)
			//Player must Grab(Supply1, Supply2,... SupplyN)
			//


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
