using UnityEngine;
using System.Collections;

public class ApplyAction : MissionAction {

	GameObject from, to;
	GrabMe.kind kind;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public ApplyAction(GameObject from, GameObject to, GrabMe.kind kind){
		this.from = from;
		this.to = to;
		this.kind = kind;
	}

	public bool execute(){
		return false;
	}
}
