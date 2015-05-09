using UnityEngine;
using System.Collections;

public class FollowAction : MissionAction {

	private int startIndex = 0, endIndex = 0;
	GameObject Torkana;
	TestMentorFollow followScript;
	NavMeshAgent navAgent;
	bool pathBegun = false;

	public FollowAction(int start_index, int end_index, GameObject mover){
		this.startIndex = start_index;
		this.endIndex = end_index;
		this.Torkana = mover;
		this.followScript = mover.GetComponent<TestMentorFollow> ();
		this.navAgent = mover.GetComponent<NavMeshAgent> ();
	}
	
	public bool execute(){
		if (!followScript.enabled && !pathBegun) {
//			if (navAgent.enabled){
//				navAgent.enabled = false;
//			}
			followScript.enabled = true;
			//followScript.Start ();
			if (followScript.isEnabled ()) {
				followScript.beginPath (startIndex, endIndex);
				pathBegun = true;
			}
		} else if (!pathBegun && followScript.isEnabled () && !followScript.isGoal()) {
			//followScript.Start ();
			followScript.beginPath (startIndex, endIndex);
			pathBegun = true;
		} else if (followScript.isGoal()){
			if (navAgent != null){
				navAgent.enabled = false;
			}
			return true;
		}

		return false;
	}
}
