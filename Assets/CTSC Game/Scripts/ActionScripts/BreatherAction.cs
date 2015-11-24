using UnityEngine;
using System.Collections;

public class BreatherAction : MissionAction {

	Breather breather;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		breather.gameObject.SetActive (true);
		if (breather.hasWon ())
			return true;
		else
			return false;
	}

	public BreatherAction(Breather breather){
		this.breather = breather;
	}
}
