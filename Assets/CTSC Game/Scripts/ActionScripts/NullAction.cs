using UnityEngine;
using System.Collections;

public class NullAction : MissionAction {
	//An action designed to be used when an event is only activated under certain circumstances.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		return true;
	}
}
