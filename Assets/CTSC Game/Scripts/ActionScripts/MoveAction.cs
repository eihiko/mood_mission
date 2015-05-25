using UnityEngine;
using System.Collections;

public class MoveAction : MissionAction  {

	GameObject move;
	GameObject to;
	AnimationEngine.Type animType;
	AnimationEngine animEngine;
	bool animate = true;
	NavMeshAgent agent = null;
	bool moveMe = false;
	bool atDestination = false;
	float speed = 1f;
	bool primMove = false;

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
			if (animate && Vector3.Distance (move.transform.position, to.transform.position) > 3.3f) {
				//Debug.Log ("mover is moving to their destination");
				move.GetComponent<NavMeshAgent> ().enabled = true;
				//animate the npc
				if (animate) {
					//check for limp in walk
					if (animType == AnimationEngine.Type.LIMP) {
						animEngine.setHasLimp (true);
					}
					//set to walking
					animEngine.setMoveSpeed (.7f);
					//don't do this for now
					//agent.SetDestination (to.transform.position);
					//move.transform.position = to.transform.position;
					float step = speed * Time.deltaTime;
					move.transform.position = Vector3.MoveTowards (move.transform.position, to.transform.position, step);
				} 

				atDestination = false;
			} else if (!animate && Vector3.Distance (move.transform.position, to.transform.position) > 0.1f) {
				//stop the nav agent
				agent.Stop ();
				move.GetComponent<NavMeshAgent> ().enabled = false;

				move.transform.position = to.transform.position;

				//.Translate((move.transform.localPosition -
				//                        to.transform.localPosition)
				//                      * 5 * Time.deltaTime);
			} else {
				//Debug.Log ("mover is at destination");
				//stop the nav agent
				agent.Stop ();
				if (animate) {
					//set to idle
					animEngine.setMoveSpeed (0f);
				}

				atDestination = true;
			}
		}
//		} else if (moveMe) {
//			move.transform.Translate(to.transform.localPosition - move.transform.localPosition, Space.World);
//			if (Vector3.Distance (move.transform.position, to.transform.position) < 1f) {
//				atDestination = true;
//			}
//		}
	}
	
	//call pathfinding script with animator
	//return true when "move" is adjacent to "to" (other npc or waypoint)
	public bool execute(){
		//check type for more complex moves
		//Debug.Log ("Trying move action");

		if (!atDestination && agent != null) {
			if (animate) {
				moveMe = true;
				//Debug.Log ("Executing move action");
				Update ();
			} else {
				//stop the nav agent
				agent.Stop ();
				move.transform.position = to.transform.position;
				atDestination = true;
			}
		} else if (!atDestination) {

			//move.GetComponent<NavMeshAgent>().enabled = false;
			//stop the nav agent
			//agent.Stop ();
			move.transform.position = to.transform.position;
			atDestination = true;
		}
		if (atDestination) {
			//stop the nav agent
//			agent.Stop ();
			return true;
		}
		return false;
	}
}
