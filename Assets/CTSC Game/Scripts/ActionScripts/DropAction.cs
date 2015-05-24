using UnityEngine;
using System.Collections;

public class DropAction : MissionAction {

	GameObject who;
	GrabMe.kind what;
	CharacterOurs whoIs;
	bool showText;
	GameObject interactionManager;
	GUIHandler guiHandler;
	bool first = true;
	string whatStr = "";

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	public void Update () {
	}
	
	public DropAction(GameObject who, GrabMe.kind what){
		this.who = who;
		if (who.GetComponent<CharacterOurs> () != null) {
			whoIs = who.GetComponent<CharacterOurs> ();
		}
		this.what = what;
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
	}

	public DropAction(GameObject who, GrabMe.kind what, string whatStr) : this(who, what){
		this.whatStr = whatStr;
	}

	public bool execute(){
//		Debug.Log ("Executing drop action");
		if (!whoIs.has (what)) {
			guiHandler.reset ();
			if (what.Equals("map")){
				who.transform.FindChild("MapCamera").gameObject.SetActive(false);
			}
			return true;
		}
		if (first) {
			guiHandler.setTextToShow ("Drop " + what.ToString().ToLower() + " with F");
			first = false;
		}
		//who will drop what and lose it from inventory
		//what will display on the ground wherever they dropped it
		return false;
	}
}
