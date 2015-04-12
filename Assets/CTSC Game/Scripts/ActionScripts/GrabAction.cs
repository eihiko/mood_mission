using UnityEngine;
using System.Collections;

public class GrabAction : MissionAction {

	GameObject who, what;
	Character whoIs;
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

	public GrabAction(GameObject who, GameObject what, string text){
		this.who = who;
		if (who.GetComponent<Character> () != null) {
			whoIs = who.GetComponent<Character> ();
		}
		this.what = what;
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
		this.text = text;
	}
	
	public bool execute(){
		Debug.Log ("Executing grab action");
		if (whoIs.has (what)) {
			guiHandler.reset ();
			return true;
		}
		if (first) {
			guiHandler.setTextToShow (text);
			first = false;
		}
		//what moves closer to who when picking up
		//then it is turned inactive (invisible) then
		//what is sent to who's inventory
		return false;
	}
}
