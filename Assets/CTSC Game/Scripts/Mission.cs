using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Mission : IComparable<Mission> {

//	public string FILE_NAME = "Assets\CTSC Game\StoryDocuments/";

	private MissionManager.MissionType missionType;
	private int missionID;
	private List<GameObject> missionObjects;
	private bool isCurrentMission;
	private bool isComplete;

	//A map of events and whether they are complete.
	private SortedList<MissionManager.EventType, bool> missionEvents; 
	private Dictionary<MissionManager.EventType, Transform> eventTransforms;
	private List<String> missionBriefs = new List<String>();
	
	// Use this for initialization
	void Start () {
//		ReadStringFromFile (FILE_NAME);
	}
	
	public Mission(int missionID, MissionManager.MissionType missionType,
	               List<MissionManager.EventType> events,
	               Dictionary<MissionManager.EventType, Transform> eventTransforms){
		this.missionID = missionID;
		this.missionType = missionType;
		this.missionEvents = new SortedList<MissionManager.EventType, bool>();
		this.eventTransforms = eventTransforms;
		this.isCurrentMission = false;
		this.isComplete = false;

		//Add the given events to this mission's events table with a false value.
		foreach (MissionManager.EventType e in events) {
			missionEvents.Add(e, false);
		}
	}

	private void enableMissionEvent(MissionManager.EventType eventType){
		Debug.Log (eventType.ToString ());

		if (eventType != MissionManager.EventType.NULL) {
			if (eventTransforms [eventType].gameObject.activeSelf == false) {
				eventTransforms [eventType].gameObject.SetActive (true);
			}
		} else {
			isComplete = true;
		}
	}

	private void disableMissionEvent(MissionManager.EventType eventType){
		if (eventTransforms[eventType].gameObject.activeSelf == true){
			eventTransforms[eventType].gameObject.SetActive(false);
		}
	}
	
	public void execute(){
		MissionManager.EventType eventType = getCurrentMissionEvent ();
		if (eventType != MissionManager.EventType.NULL) {
			enableMissionEvent(eventType);
		} else {
			isComplete = true;
			Debug.Log ("Mission is complete via execute method.");
		}
	}

	public void setIsCurrentMission(bool current){
		this.isCurrentMission = current;
		//Begin this mission
		if (current == true) {
			enableMissionEvent(getCurrentMissionEvent());
		}
	}

	public bool getIsCurrentMission(){
		return this.isCurrentMission;
	}

	private MissionManager.EventType getCurrentMissionEvent(){
		foreach (KeyValuePair<MissionManager.EventType, bool> eventType in missionEvents) {
			if (!eventType.Value){
				Debug.Log("Current mission event is: " + eventType.ToString());
				return eventType.Key;
			}
		}
		Debug.Log ("Current mission event is null type. Mission finished.");
		return MissionManager.EventType.NULL;
	}

	void setID(int missionID){
		this.missionID = missionID;
	}

	public void setEventComplete(MissionManager.EventType completedEvent){
		missionEvents [completedEvent] = true;
		disableMissionEvent (completedEvent);
		Debug.Log ("Player has completed: " + completedEvent.ToString () + " event");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool getIsComplete(){
		//Check anyway to see if it is complete
		//Some missions have different criteria for completion.
		if (!isComplete){ 
			//Cases for where the player doesn't necessarily complete all tasks
			if (missionType == MissionManager.MissionType.ENTER_CITY_GATHER_SUPPLIES) {
			} else if (missionType == MissionManager.MissionType.HELP_TP_1){
			} else {
				//Iterate through all events and see if they are complete
				foreach (KeyValuePair<MissionManager.EventType, bool> kvp in missionEvents){
					if (!kvp.Value) {
						isComplete = false;
					}
				}
				isComplete = true;
			}
		}
		return isComplete;
	}

	public int getID(){
		return this.missionID;
	}

	public MissionManager.MissionType getMissionType(){
		return this.missionType;
	}
	
	public int CompareTo(Mission other){
		return this.getID ().CompareTo (other.getID ());
	}

	public List<String> getMissionBriefs(){
		return this.missionBriefs;
	}

	public void addMissionBrief(string brief){
		this.missionBriefs.Add(brief);
	}
	
	public override bool Equals(object obj){
		Mission other;
		if ((obj != null) && (obj is Mission)){
			other = (Mission)obj;
			return this.missionID == other.getID () &&
				this.missionType == other.getMissionType();
		}
		return false;
	}

	public override int GetHashCode ()
	{
		return missionID.GetHashCode () + missionType.GetHashCode();
	}

	public override string ToString ()
	{
		return string.Format ("the : " + missionID + " mission.");
	}
}
