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
		minimap = GameObject.Find ("MinimapUpdater").GetComponent<MinimapUpdater> ();
	}

	public bool execute() {
		minimap.changeObjective (objective);
		//Debug.Log("Pointing at " + objective.ToString());
		return true;
	}


}
