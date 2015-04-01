using UnityEngine;
using System.Collections;

public class MoveAction : MissionAction {

	GameObject move;
	GameObject to;
	AnimationEngine.Type animType;
	AnimationEngine animEngine;
	bool animate = true;
	NavMeshAgent agent = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public MoveAction(GameObject move, GameObject to){
		this.move = move;
		this.to = to;
		this.animate = false;
	}

	public MoveAction(GameObject move, GameObject to, AnimationEngine.Type animType)
	: this (move, to) {
		this.animType = animType;
		agent = move.GetComponent<NavMeshAgent> ();
		if ((animEngine = move.GetComponent<AnimationEngine> ()) != null) {
			animate = true;
		} else {
			animate = false;
		}
	}

	//call pathfinding script with animator
	//return true when "move" is adjacent to "to" (other npc or waypoint)
	public bool execute(){
		//check type for more complex moves

		//animate the npc
		if (animate) {
			//check for limp in walk
			if (animType == AnimationEngine.Type.LIMP){
				animEngine.setHasLimp(true);
			}
			//set to walking
			animEngine.setMoveSpeed(.7f);
		}

		if (agent != null) {
			//begin the nav agent
			agent.SetDestination (to.transform.position);

			//wait until the nav agent completes
			while (Vector3.Distance(move.transform.position, to.transform.position) > 1.5f) {
				Debug.Log ("mover is moving to their destination");
			}

			//stop the nav agent
			agent.SetDestination (move.transform.position);
		} else {
			//wait until the player reaches destination
			while (Vector3.Distance(move.transform.position, to.transform.position) > 1.5f) {
				Debug.Log ("mover is moving to their destination");
			}
		}
		
		Debug.Log("mover has reached destination");

		return true;
	}
}
