using UnityEngine;
using System.Collections;

public class EnterAction : MissionAction {

	GameObject willEnter, toEnter;
	EnterScript collisionScript;
	GameObject interactionManager;
	GUIHandler guiHandler;
	bool first = true;
	string text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool execute(){
		Debug.Log ("Executing enter action");
		if (collisionScript.isEntered) {
			guiHandler.reset();
			Debug.Log("Enter action is complete");
			return true;
		}
		if (first) {
			guiHandler.setTextToShow (text);
			first = false;
		}
		return false;
	}

	//toEnter must have an EnterScript on it!
	public EnterAction(GameObject willEnter, GameObject toEnter, string text){
		this.willEnter = willEnter;
		this.toEnter = toEnter;
	    this.collisionScript = toEnter.GetComponent<EnterScript> ();
		this.collisionScript.setWillEnter(willEnter);

		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
		this.text = text;
	}
}
