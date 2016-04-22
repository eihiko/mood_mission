using UnityEngine;
using System.Collections;

public class SleepAction : MissionAction {

	GameObject Ogre;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		Ogre.GetComponent<Sleep> ().enabled = true;
		return true;
	}

	public SleepAction(GameObject sleeper){
		Ogre = sleeper;
	}
}
