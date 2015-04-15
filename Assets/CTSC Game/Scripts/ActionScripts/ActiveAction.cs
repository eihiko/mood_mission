
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
	bool isFirst = true;

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
		this.currUI = thing;
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
			if (isFirst){
				isFirst = false;
				//call gui to display text
				dBox.displayText (false, startPar, numPar);
				Debug.Log ("Displaying text on Gui");
				return false;
			}
			//only return true when text box is completed.
			if (//canvas.isActiveAndEnabled &&
		       dBox.textCompleted) {
				Debug.Log ("Text box is completed!");
				return true;
			}
			Debug.Log ("Still displaying text.");
			//call gui to display text
			//dBox.displayText (true, startPar, numPar);
			return false;
		}
		return true;
	}
}
