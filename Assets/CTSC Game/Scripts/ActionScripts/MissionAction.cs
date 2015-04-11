using UnityEngine;
using System.Collections;

public interface MissionAction {

	//MissionAction constructAction(ActionType type);

	// execute this action, return true when success
	bool execute ();

//	// Update is called once per frame
//	void Update ();
}
