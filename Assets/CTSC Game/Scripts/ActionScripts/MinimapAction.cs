using UnityEngine;
using System.Collections;

public class MinimapAction : MissionAction {

	public MinimapUpdater minimap;
	string objective;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public MinimapAction(string pointTo){
		objective = pointTo;
	}

	public bool execute() {
		minimap.changeObjective (objective);
		return true;
	}


}
