 using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (AStarPath))]
public class NPC_FSM_C : MonoBehaviour {

	public enum NPC_State {  WALKING, RUNNING, FLYING, IDLING, STOPPED, ATTACK, ALARM }
	protected Animator animator;

	//Mechanim parameters
	public float moveSpeed, yaw, height;
	public bool hasLimp = false;

	public Transform[] waypoints;
	
	public float attackSpeed;
	private NPC_State state;
	//The point to move to
	//public Transform target;

	private bool collision;
	private Seeker seeker;
	private Vector3 targetPosition;
	private CharacterController controller;
	//The AI's speed per second
	public float walkSpeed = 100f;
	public float runSpeed = 300f;
	public float health = 100f;
	private bool lastPathCompleted;
	private AStarPath aStarPath;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	public void Start () {
		aStarPath = GetComponent<AStarPath> ();
		setLastPathCompleted (false);
		animator = GetComponent<Animator> ();
//		setYaw (yaw);
//		setMoveSpeed (moveSpeed);
//		setHeight (height);
//		setHasLimp (hasLimp);
		state = NPC_State.WALKING;
//		targetPosition = target.position;
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		aStarPath.Start ();
		switchState ();
	}

	public int generateRandomInt(int start, int end){
		return Random.Range (start, end);
	}

	public void finiteStateMachine(){
		if ((state == NPC_State.WALKING || state == NPC_State.RUNNING)
		    && lastPathCompleted == true){
			switchState ();
		}
	}
	
    public void switchState(){
		switch (state) {
		case NPC_State.ATTACK:
			//Attack (target.collider);
			break;
		case NPC_State.FLYING:
			//Fly (target);
			break;
		case NPC_State.IDLING:
			Idle ();
			break;
		case NPC_State.RUNNING:
			randomRun();
			break;
		case NPC_State.STOPPED:
			Stop();
			break;
		case NPC_State.WALKING:
			randomWalk ();
			break;	
		}
	}
	
	public void randomWalk(){
		visitRandomDestination (walkSpeed);
	}
	
	public void randomRun(){
		visitRandomDestination (runSpeed);
	}

	public void visitRandomDestination(float speed){
		visitNewDestination (speed);
	}

	/**
	 * Picks a random waypoint to visit and creates a new path to it with A*.
	 */
	public void visitNewDestination(float speed){
		Debug.Log ("Visiting a new destination.");
		Transform newDestination = findRandomWaypoint ();
		aStarPath.setSpeed (speed);
		aStarPath.newPath (newDestination);
	}

	/**
	 * Finds a random waypoint for this NPC to visit.
	 */
	public Transform findRandomWaypoint(){
		int waypointCt = waypoints.Length;
		int waypoint = generateRandomInt (0, waypointCt-1);
		Debug.Log ("Picked waypoint #" + waypoint);
		return waypoints[waypoint];
	}

	
	public void setLastPathCompleted(bool complete){
		lastPathCompleted = complete;
	}

	public void setYaw(float yaw){
		animator.SetFloat("Yaw", yaw);
	}

	public void setHeight(float height){
		animator.SetFloat("Height", height);
	}

	public void setMoveSpeed(float moveSpeed){
		animator.SetFloat("MoveSpeed", moveSpeed);
	}

	public void setSitting(bool sit){
		animator.SetBool("Sit", sit);
	}

	public void setStrafeLeft(bool strafeLeft){
		animator.SetBool("StrafeLeft", strafeLeft);
	}

	public void setStrafeRight(bool strafeRight){
		animator.SetBool("StrafeRight", strafeRight);
	}

	public void setWaveHand(bool waveHand){
		animator.SetBool("WaveHand", waveHand);
	}

	public void setWaveArms(bool waveArms){
		animator.SetBool ("WaveArms", waveArms);
	}

	public void setHasLimp(bool hasLimp){
		animator.SetBool("HasLimp", hasLimp);
	}

	public void setState(NPC_State state){
		this.state = state;
	}

	public void FixedUpdate () {

//		setYaw (yaw);
//		setMoveSpeed (moveSpeed);
//		setHeight (height);
//		setHasLimp (hasLimp);
		finiteStateMachine ();
	}

	public static void updateState(){
	}
			 
    public void OnTriggerEnter(Collider opponent){
			if (GetComponent<Collider>().tag.Equals("Player")){
				Attack(opponent);
			}
			collision = true;
		}

	public void Stop(){

	}

	public void Run(){

	}

	public void Fly(Transform destination){

	}

//	public void Walk(Transform destination){
//		MoveTo (destination, walkSpeed);
//	}

	public void Idle(){
		setMoveSpeed (0.0f);
	}

	public void Attack(Collider opponent){
//		MoveTo (opponent.transform, attackSpeed);
	}

//	public static void Talk(string words, File voice){
//	}


}

