 using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class NPC_FSM : MonoBehaviour {

	public enum NPC_State {  WALKING, RUNNING, FLYING, IDLING, STOPPED, ATTACK, ALARM }

	public float attackSpeed;
	private NPC_State state;
	private int health;
	//The point to move to
	public Transform target;
	private bool collision;
	private Seeker seeker;
	private Vector3 targetPosition;
	private CharacterController controller;
	//The calculated path
	public Path path;
	//The AI's speed per second
	public float walkSpeed = 100;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	public void Start () {
		state = NPC_State.IDLING;
		health = 100;
		targetPosition = target.position;
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, target.position, OnPathComplete);
	}

	public void setState(NPC_State state){
		this.state = state;
	}

	public void OnPathComplete (Path p) {
		Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	public void FixedUpdate () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			return;
		}
		switch (state) {
			case NPC_State.ATTACK:
				Attack (target.GetComponent<Collider>());
				break;
			case NPC_State.FLYING:
				Fly (target);
				break;
			case NPC_State.IDLING:
				Idle ();
				break;
			case NPC_State.RUNNING:
				Run();
				break;
			case NPC_State.STOPPED:
			Stop();
				break;
			case NPC_State.WALKING:
				Walk (target);
				break;	
		}
	}
	public static void updateState(){
	}
			 
//    public void OnTriggerEnter(Collider opponent){
//			if (collider.tag.Equals("Player")){
//				Attack(opponent);
//			}
//			collision = true;
//		}

	public void Stop(){

	}

	public void Run(){
		//	controller.animation.PlayQueued ("Run", QueueMode.PlayNow);
	}

	public void Fly(Transform destination){
		//	controller.animation.PlayQueued ("Fly", QueueMode.PlayNow);
	}

	public void Walk(Transform destination){
	//	controller.animation.PlayQueued ("Walk", QueueMode.PlayNow);
		MoveTo (destination, walkSpeed);
	}
	
	public void Idle(){
		controller.GetComponent<Animation>().PlayQueued ("Idle", QueueMode.PlayNow);
	}

	public void Attack(Collider opponent){
		controller.GetComponent<Animation>().PlayQueued ("Attack", QueueMode.PlayNow);
		MoveTo (opponent.transform, attackSpeed);
	}

	public void MoveTo(Transform destination, float speed){

		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.SimpleMove (dir);

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}

//	public static void Talk(string words, File voice){
//	}


}

