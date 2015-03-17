using UnityEngine;
using System.Collections;

public class TestMentorFollow : MonoBehaviour {

	public Transform player;
	public Transform pathPointers;
	private NavMeshAgent agent;
	private AnimationEx animationEx;
	private Vector3 previousLocation;
	private float velocity;
	//An arraylist to store the agent destinations
	private Vector3[] agentDests;
	private int arrIndex;
	public int numDests;
	void Start() {
		agentDests = new Vector3[numDests];
		agent = GetComponent<NavMeshAgent>();
		animationEx = GetComponent<AnimationEx> ();
		arrIndex = 0;
		//Iterate over the children of the pathPointers object
		foreach (Transform pointer in pathPointers) 
		{
			agentDests[arrIndex++] = pointer.position;
		}

		numDests = arrIndex;
		arrIndex = 0;
		agent.SetDestination(agentDests[0]);

		previousLocation = transform.position;
	}
	void Update() {

		/*RaycastHit hit;
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				agent.SetDestination(hit.point);
			Debug.Log ("Destination is: (" + hit.point.x + ", " + hit.point.y + ", " + hit.point.z + ")");
			
		}*/
		animationEx.setHasLimp (true);
		if (arrIndex == numDests) {
			animationEx.setMoveSpeed(0.0f);
			return;
		}

		if ((transform.position - player.position).magnitude < 8)
		{
			agent.SetDestination(agentDests[arrIndex]);
			//Debug.Log("Distance between agent and goal is: " + (transform.position - agentDests[arrIndex]).magnitude);
			Debug.Log("Distance between agent and player is: " + (transform.position - player.position).magnitude);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[arrIndex]).magnitude <= 0.5 && arrIndex < numDests - 1) {

				agent.SetDestination(agentDests[++arrIndex]);
			}
		}
		else 
		{
			Debug.Log("When distance is >= 8: " + (transform.position - player.position).magnitude);
			agent.SetDestination(transform.position);
			animationEx.setMoveSpeed(0.0f);
		}
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		Debug.Log ("Velocity is: " + velocity);
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(arrIndex == numDests || velocity < 0.1)
		{

			animationEx.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("attack");
		}
		//Else, set the movespeed to a value that will trigger the walking animation
		else {
			animationEx.setMoveSpeed (0.7f);
		}

		//animationEx.setMoveSpeed (velocity);
		previousLocation = transform.position;

	}
}
