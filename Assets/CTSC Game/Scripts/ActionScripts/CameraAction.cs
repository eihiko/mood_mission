using UnityEngine;
using System.Collections;

public class CameraAction : MissionAction {
	
	bool controllable;

	// Use this for initialization
	void Start () {
		controllable = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute() {
		Screen.lockCursor = !controllable;
		Cursor.visible = controllable;
		return true;
	}

	public CameraAction(bool control){
		this.controllable = control;
	}

}
