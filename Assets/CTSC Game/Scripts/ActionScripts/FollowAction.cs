using UnityEngine;
using System.Collections;

public class FollowAction : MissionAction {

	private int startIndex = 0, endIndex = 0;
	GameObject Torkana;
	TestMentorFollow followScript;
	NavMeshAgent navAgent;

	public FollowAction(int startIndex, int endIndex, GameObject mover){
		this.startIndex = startIndex;
		this.endIndex = endIndex;
		this.Torkana = mover;
		this.followScript = mover.GetComponent<TestMentorFollow> ();
		this.navAgent = mover.GetComponent<NavMeshAgent> ();
	}
	
	public bool execute(){
		if (!followScript.enabled) {
			if (!navAgent.enabled){
				navAgent.enabled = true;
			}
			followScript.enabled = true;
			followScript.Start ();
			if (followScript.isEnabled ()) {
				followScript.beginPath (startIndex, endIndex);
			}
		} else if (followScript.isEnabled ()&& !followScript.isGoal() && followScript.currIndex != startIndex) {

			//followScript.beginPath (startIndex, endIndex);
		} else if (followScript.isGoal()){
			navAgent.enabled = false;
			return true;
		}

		return false;
	}
}
