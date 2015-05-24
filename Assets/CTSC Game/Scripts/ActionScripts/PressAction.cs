using UnityEngine;
using System.Collections;

public class PressAction : MissionAction {
	
	FloorSwitch switchScript;

	public PressAction(GameObject switchObj){
		switchScript = switchObj.GetComponent<FloorSwitch> ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		if (switchScript.isPressed) {
			return true;
		}
		return false;
	}
}
