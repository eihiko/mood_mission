using UnityEngine;
using System.Collections;

public class MissionEvent : MonoBehaviour {

	private MissionManager missionManager;
	public MissionManager.MissionType missionType;
	public MissionManager.EventType eventType;
	private Mission mission;
	private bool isComplete = false;

	// Use this for initialization
	void Start () {
		missionManager = GameObject.Find("MissionManager").GetComponent<MissionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		//Handles completion of this game event.
		//Should probably check if it has been completed in the correct sequence but
		//save that for later..
		if (isComplete &&
		    missionManager.getCurrentMission().getMissionType() 
		    == missionType) {
			OnComplete();
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			setIsComplete (true);
		}
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
