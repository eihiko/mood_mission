using UnityEngine;
using System.Collections;

public class ActiveAction : MissionAction {

	public GameObject thing = null;
	public Canvas uiCanvas;
	public bool active = false;
	public string brief = null;

	// Use this for initialization
	void Start () {

	}

	public ActiveAction(GameObject thing, bool active){
		this.thing = thing;
		this.active = active;
	}

	public ActiveAction(GameObject thing, bool active, string brief) 
	: this(thing, active){
		this.brief = brief;
		//we need to get the canvas of the gui
		//for this type of ui constructor
		this.uiCanvas = getCanvas (thing);
	}

	public Canvas getCanvas(GameObject gui){
		return gui.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	public void Update () {
	
	}

	public bool execute(){
		thing.SetActive (active);
		if (brief != null) {

		}
		return true;
	}
}
