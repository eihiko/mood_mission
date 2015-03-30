using UnityEngine;
using System.Collections;

public class DropAction : MissionAction {

	GameObject who, what;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	public void Update () {
	
	}

	public DropAction(GameObject who, GameObject what){
		this.who = who;
		this.what = what;
	}

	public bool execute(){
		//who will drop what and lose it from inventory
		//what will display on the ground wherever they dropped it
		return false;
	}
}
