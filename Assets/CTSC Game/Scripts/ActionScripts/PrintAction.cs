using UnityEngine;
using System.Collections;

public class PrintAction : MissionAction {

	string toPrint = "";
	GameObject interactionManager;
	GUIHandler guiHandler;
	int timeToStop = 0;
	int time = 0;

	public PrintAction(string printMe, int duration){
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
		timeToStop = duration;
		toPrint = printMe;
	}

	public bool execute(){
		if (time >= timeToStop) {
			toPrint = "";
		}
		guiHandler.setTextToShow (toPrint);
		time++;
		return time >= timeToStop;
	}
}
