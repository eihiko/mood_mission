using UnityEngine;
using System.Collections;

public class MoveAction : MonoBehaviour, MissionAction  {

	GameObject move;
	GameObject to;
	AnimationEngine.Type animType;
	AnimationEngine animEngine;
	bool animate = true;
	NavMeshAgent agent = null;
	bool moveMe = false;
	bool atDestination = false;

	// Use this for initialization
	void Start () {
	
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

	void Update(){
		if (moveMe) {
			if (Vector3.Distance(move.transform.position, to.transform.position) > 3f) {
				Debug.Log ("mover is moving to their destination");
				
				//animate the npc
				if (animate) {
					//check for limp in walk
					if (animType == AnimationEngine.Type.LIMP){
						animEngine.setHasLimp(true);
					}
					//set to walking
					animEngine.setMoveSpeed(.7f);
				}
				agent.SetDestination (to.transform.position);
				atDestination = false;
			} else {
				Debug.Log ("mover is at destination");
				//stop the nav agent
				agent.SetDestination (move.transform.position);

				//set to idle
				animEngine.setMoveSpeed(0f);
				atDestination = true;
			}
		}
	}
	
	//call pathfinding script with animator
	//return true when "move" is adjacent to "to" (other npc or waypoint)
	public bool execute(){
		//check type for more complex moves
		Debug.Log ("Trying move action");

		if (agent != null) {
			moveMe = true;
			Debug.Log ("Executing move action");
			Update ();
		}
		if (atDestination) {
			return true;
		}
		return false;
	}
}
