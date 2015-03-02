using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
[RequireComponent (typeof (Seeker))]
public class AStarPath : MonoBehaviour {
	private Seeker seeker;
	private NPC_FSM fsm;
	private CharacterController controller;
	//The calculated path
	public Path path;
	//The AI's speed per second
	public float speed = 100;
	public float turningSpeed = 3f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	/** Target point is Interpolated on the current segment in the path so that it has a distance of #forwardLook from the AI.
	  * See the detailed description of AIPath for an illustrative image */
	public float forwardLook = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	/** Point to where the AI is heading.
	  * Filled in by #CalculateVelocity */
	protected Vector3 targetPoint;
	/** Relative direction to where the AI is heading.
	 * Filled in by #CalculateVelocity */
	protected Vector3 targetDirection;
	Transform tr;
	
	private Transform target;
	public void Start () {
		fsm = GetComponent<NPC_FSM> ();
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
	}

	public void newPath(Transform target){
		if (target == null) {
			Debug.Log ("Target is null.");
		} else if (transform == null) {
			Debug.Log ("Transform is null.");
		} else if (seeker == null) {
			Debug.Log ("Seeker is null.");
		} else {
			//Start a new path to the targetPosition, return the result to the OnPathComplete function
			seeker.StartPath (transform.position, target.position, OnPathComplete);
		}
	}

	public void setSpeed(float speed){
		this.speed = speed;
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
//			fsm.setLastPathCompleted(true);
			return;
		}
		//CalculateRotateDirection (GetFeetPosition());
		//RotateTowards (targetDirection);
//		fsm.setLastPathCompleted (false);

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

	protected float XZSqrMagnitude (Vector3 a, Vector3 b) {
		float dx = b.x-a.x;
		float dz = b.z-a.z;
		return dx*dx + dz*dz;
	}

	public virtual Vector3 GetFeetPosition () {
		if (controller != null) {
			return transform.position - Vector3.up*controller.height*0.5F;
		}
		
		return transform.position;
	}
	
	/** Calculates desired velocity.
	 * Finds the target path segment and returns the forward direction, scaled with speed.
	 * A whole bunch of restrictions on the velocity is applied to make sure it doesn't overshoot, does not look too far ahead,
	 * and slows down when close to the target.
	 * /see speed
	 * /see endReachedDistance
	 * /see slowdownDistance
	 * /see CalculateTargetPoint
	 * /see targetPoint
	 * /see targetDirection
	 * /see currentWaypointIndex
	 */
	protected void CalculateRotateDirection (Vector3 currentPosition) {
		if (path == null || path.vectorPath == null || path.vectorPath.Count == 0) return; 
		
		List<Vector3> vPath = path.vectorPath;
		//Vector3 currentPosition = GetFeetPosition();
		
		if (vPath.Count == 1) {
			vPath.Insert (0,currentPosition);
		}
		
		//if (currentWaypoint >= vPath.Count) { currentWaypointIndex = vPath.Count-1; }
		
		//if (currentWaypoint <= 1) currentWaypoint = 1;
		
//		while (true) {
//			if (currentWaypoint < vPath.Count-1) {
//				//There is a "next path segment"
//				float dist = XZSqrMagnitude (vPath[currentWaypointIndex], currentPosition);
//				//Mathfx.DistancePointSegmentStrict (vPath[currentWaypointIndex+1],vPath[currentWaypointIndex+2],currentPosition);
//				if (dist < pickNextWaypointDist*pickNextWaypointDist) {
//					lastFoundWaypointPosition = currentPosition;
//					lastFoundWaypointTime = Time.time;
//					currentWaypointIndex++;
//				} else {
//					break;
//				}
//			} else {
//				break;
//			}
//		}
		
		Vector3 dir = vPath[currentWaypoint] - vPath[currentWaypoint-1];
		Vector3 targetPosition = CalculateTargetPoint (currentPosition,vPath[currentWaypoint-1] , vPath[currentWaypoint]);
		//vPath[currentWaypointIndex] + Vector3.ClampMagnitude (dir,forwardLook);
		
		
		
		dir = targetPosition-currentPosition;
		dir.y = 0;
		this.targetDirection = dir;
//		float targetDist = dir.magnitude;
		
//		float slowdown = Mathf.Clamp01 (targetDist / slowdownDistance);
		

//		this.targetPoint = targetPosition;
//		
//		if (currentWaypointIndex == vPath.Count-1 && targetDist <= endReachedDistance) {
//			if (!targetReached) { targetReached = true; OnTargetReached (); }
//			
//			//Send a move request, this ensures gravity is applied
//			return Vector3.zero;
//		}
//		
//		Vector3 forward = tr.forward;
//		float dot = Vector3.Dot (dir.normalized,forward);
//		float sp = speed * Mathf.Max (dot,minMoveScale) * slowdown;
//		
//		
//		if (Time.deltaTime	> 0) {
//			sp = Mathf.Clamp (sp,0,targetDist/(Time.deltaTime*2));
//		}
//		return forward*sp;
	}

	/** Rotates in the specified direction.
	 * Rotates around the Y-axis.
	 * \see turningSpeed
	 */
	protected virtual void RotateTowards (Vector3 dir) {
		
		if (dir == Vector3.zero) return;
		
		Quaternion rot = transform.rotation;
		Quaternion toTarget = Quaternion.LookRotation (dir);
		
		rot = Quaternion.Slerp (rot,toTarget,turningSpeed*Time.deltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		transform.rotation = rot;
	}

	/** Calculates target point from the current line segment.
	 * \param p Current position
	 * \param a Line segment start
	 * \param b Line segment end
	 * The returned point will lie somewhere on the line segment.
	 * \see #forwardLook
	 * \todo This function uses .magnitude quite a lot, can it be optimized?
	 */
	protected Vector3 CalculateTargetPoint (Vector3 p, Vector3 a, Vector3 b) {
		a.y = p.y;
		b.y = p.y;
		
		float magn = (a-b).magnitude;
		if (magn == 0) return a;
		
		float closest = AstarMath.Clamp01 (AstarMath.NearestPointFactor (a, b, p));
		Vector3 point = (b-a)*closest + a;
		float distance = (point-p).magnitude;
		
		float lookAhead = Mathf.Clamp (forwardLook - distance, 0.0F, forwardLook);
		
		float offset = lookAhead / magn;
		offset = Mathf.Clamp (offset+closest,0.0F,1.0F);
		return (b-a)*offset + a;
	}
}
