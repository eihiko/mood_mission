using UnityEngine;
using System.Collections;

public class AnimateAction : MissionAction {

	public enum type {
		HEAL
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public AnimateAction(GameObject fromWho, GameObject toWho, AnimateAction.type type){

	}

	public bool execute(){
		return false;
	}
}
