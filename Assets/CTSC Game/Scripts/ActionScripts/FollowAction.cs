using UnityEngine;
using System.Collections;

public class FollowAction : MissionAction {

	private int startIndex = 0, endIndex = 0;
	GameObject Torkana;
	TestMentorFollow followScript;

	public FollowAction(int startIndex, int endIndex, GameObject mover){
		this.startIndex = startIndex;
		this.endIndex = endIndex;
		this.Torkana = mover;
		this.followScript = mover.GetComponent<TestMentorFollow> ();
	}
	
	public bool execute(){
		if (!followScript.enabled) {
			followScript.enabled = true;
			if (!followScript.isEnabled ()) {
				followScript.beginPath (startIndex, endIndex);
			}
		} else if (followScript.isPathComplete () && followScript.currIndex != startIndex) {
			followScript.beginPath (startIndex, endIndex);
		} else if (followScript.isPathComplete()){
			return true;
		}

		return false;
	}
}
