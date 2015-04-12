using UnityEngine;
using System.Collections;

public class EnterAction : MissionAction {

	GameObject willEnter, toEnter;
	EnterScript collisionScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool execute(){
		Debug.Log ("Executing enter action");
		if (collisionScript.isEntered) {
			Debug.Log("Enter action is complete");
			return true;
		}
		return false;
	}

	//toEnter must have an EnterScript on it!
	public EnterAction(GameObject willEnter, GameObject toEnter){
		this.willEnter = willEnter;
		this.toEnter = toEnter;
	    this.collisionScript = toEnter.GetComponent<EnterScript> ();
		this.collisionScript.setWillEnter(willEnter);
	}
}
