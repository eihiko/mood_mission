using UnityEngine;
using System.Collections;

public class WaitAction : MissionAction {
	//Action waits until the given flag is set, then allows the mission to continue.

	StoredBool done;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		if (done.isSet()) {
			return true;
		}
		return false;
	}

	public WaitAction(StoredBool flag){
		done = flag;
	}
}
