using UnityEngine;
using System.Collections;

public class OpenAction : MissionAction {

	GameObject open, closed, who;
	GameObject interactionManager;
	GUIHandler guiHandler;
	bool isOpened = false;
	bool first = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool execute(){
		isOpened = Input.GetKeyUp (KeyCode.Q);
		if (isOpened) {
			this.open.SetActive (true);
			this.closed.SetActive (false);
			//play any animation here and noises 
			guiHandler.reset();
			return true;
		}
		if (first) {
			guiHandler.setTextToShow ("Press Q to open");
			first = false;
		}	
		return false;
	}

	public OpenAction(GameObject who, GameObject closed, GameObject open){
		this.closed = closed;
		this.open = open;
		this.who = who;
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
	}
}
