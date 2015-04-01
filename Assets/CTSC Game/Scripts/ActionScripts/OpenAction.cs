using UnityEngine;
using System.Collections;

public class OpenAction : MissionAction {

	GameObject open, closed, who;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool execute(){
		this.open.SetActive (true);
		this.closed.SetActive (false);
		return true;
	}

	public OpenAction(GameObject who, GameObject closed, GameObject open){
		this.closed = closed;
		this.open = open;
		this.who = who;
	}
}
