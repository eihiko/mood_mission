using UnityEngine;
using System.Collections;

public class GrabAction : MissionAction {

	GameObject who, what;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GrabAction(GameObject who, GameObject what){
		this.who = who;
		this.what = what;
	}
	
	public bool execute(){
		//what moves closer to who when picking up
		//then it is turned inactive (invisible) then
		//what is sent to who's inventory
		return false;
	}
}
