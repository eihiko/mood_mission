
using UnityEngine;
using System.Collections;

public class ActiveAction : MissionAction {

	public GameObject thing = null;
	public GameObject currUI;
	Canvas canvas;
	public bool active = false;
	public string brief = null;
	int startPar;
	int numPar;
	Transform text;
	DialogBox dBox;
	bool gui = false;

	// Use this for initialization
	void Start () {

	}

	public ActiveAction(GameObject thing, bool active){
		this.thing = thing;
		this.active = active;
		this.gui = false;
	}

	public ActiveAction(GameObject thing, bool active, int startPar, int numPar) 
	: this(thing, active){
		this.gui = true;
		this.currUI = currUI;
		this.canvas = currUI.GetComponent<Canvas>();
		
		//find the ui text element
		Transform t = currUI.transform;
		foreach (Transform child in t){
			if (child.name == "UIText"){
				this.text = child;
			}
		}
		
		this.dBox = text.GetComponent<DialogBox>();
		this.startPar = startPar;
		this.numPar = numPar;
	}

	public bool execute(){
		Debug.Log ("Executing active action");
		if (!gui && active) {
			thing.SetActive(true);
			if (thing.GetComponent<NavMeshAgent>() != null){
				thing.GetComponent<NavMeshAgent>().enabled = false;
			}
		} else if (!gui && !active) {
			thing.SetActive(false);
		} else {
			//call gui to display text
			dBox.displayText (true, startPar, numPar);   
		
			//execute while ui is active
			if (canvas.isActiveAndEnabled &&
		       !dBox.textCompleted) {
				return false;
			}
		}
		return true;
	}
}
