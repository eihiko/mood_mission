using UnityEngine;
using System.Collections;

public class SetAction : MissionAction {

	StoredBool thingToSet;
	bool willBeSet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		if (willBeSet) {
			thingToSet.setFlag ();
		} else {
			thingToSet.resetFlag ();
		}
		return true;
	}

	public SetAction(StoredBool toSet, bool set){
		thingToSet = toSet;
		willBeSet = set;
	}
}
