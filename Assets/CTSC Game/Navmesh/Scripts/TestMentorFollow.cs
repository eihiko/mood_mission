using UnityEngine;
using System.Collections;

public class TestMentorFollow : MonoBehaviour {

	public Transform player;
	public Transform pathPointers;
	private NavMeshAgent agent;
	private AnimationEngine animationEngine;
	private Vector3 previousLocation;
	private float velocity;
	//An arraylist to store the agent destinations
	private Vector3[] agentDests;
	private int arrIndex;
	private bool lockTorkana;
	public int numDests;
	void Start() {
		agentDests = new Vector3[numDests];
		agent = GetComponent<NavMeshAgent>();
		animationEngine = GetComponent<AnimationEngine> ();
		arrIndex = 0;
		//Iterate over the children of the pathPointers object
		foreach (Transform pointer in pathPointers) 
		{
			agentDests[arrIndex++] = pointer.position;
		}

		numDests = arrIndex;
		arrIndex = 0;
		agent.SetDestination(agentDests[0]);

		lockTorkana = false;
		previousLocation = transform.position;
		//InvokeRepeating ("moveTorkana", 0, 0.1f);
	}
	void Update() {
		animationEngine.setHasLimp (true);
		if (arrIndex == numDests) {
			animationEngine.setMoveSpeed(0.0f);
			return;
		}

		//Debug.Log ("(" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")    " + 
		//				"(" + player.position.x + ", " + player.position.y + ", " + player.position.z + ")");
		if (((transform.position - player.position).magnitude < 8.0f && !lockTorkana)
		    || (lockTorkana && (transform.position - player.position).magnitude < 7.5f))
		{
			lockTorkana = false;
			agent.SetDestination(agentDests[arrIndex]);
			//Debug.Log("Distance between agent and goal is: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Debug.Log("Distance between agent and player is: " + (transform.position - player.position).magnitude);
			//Debug.Log("Distance between agent and its dest: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[arrIndex]).magnitude <= 0.5 && arrIndex < numDests - 1) {
				
				agent.SetDestination(agentDests[++arrIndex]);
			}
		}
		else
		{
			//Debug.Log("When distance is >= 8: " + (transform.position - player.position).magnitude);
			agent.Stop ();
			//agent.SetDestination(transform.position);
			animationEngine.setMoveSpeed(0.0f);
			lockTorkana = true;
		}
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		//Debug.Log ("Velocity is: " + velocity);
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(arrIndex == numDests || velocity < 0.1)
		{
			
			animationEngine.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("attack");
		}
		//Else, set the movespeed to a value that will trigger the walking animation
		else if (velocity >= 1.0f){
			animationEngine.setMoveSpeed (0.7f);
		}
		
		//animationEngine.setMoveSpeed (velocity);
		previousLocation = transform.position;
		

	}

	void moveTorkana() {
		animationEngine.setHasLimp (true);
		if (arrIndex == numDests) {
			animationEngine.setMoveSpeed(0.0f);
			return;
		}
		Debug.Log("Torkana transform: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + 
		          "Player transform: " + player.position.x + ", " + player.position.y + ", " + player.position.z);
		if (((transform.position - player.position).magnitude < 8.0f && !lockTorkana) || (lockTorkana && 
		                                                                                   (transform.position - player.position).magnitude < 7.5f))
		{
			lockTorkana = false;
			agent.SetDestination(agentDests[arrIndex]);
			//Debug.Log("Distance between agent and goal is: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Debug.Log("Distance between agent and player is: " + (transform.position - player.position).magnitude);
			//Debug.Log("Distance between agent and its dest: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[arrIndex]).magnitude <= 0.5 && arrIndex < numDests - 1) {
				
				agent.SetDestination(agentDests[++arrIndex]);
			}
		}
		else
		{
			//Debug.Log("When distance is >= 8: " + (transform.position - player.position).magnitude);
			agent.SetDestination(transform.position);
			animationEngine.setMoveSpeed(0.0f);
			lockTorkana = true;
		}
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		//Debug.Log ("Velocity is: " + velocity);
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(arrIndex == numDests || velocity < 0.1)
		{
			
			animationEngine.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("attack");
		}
		//Else, set the movespeed to a value that will trigger the walking animation
		else if (velocity >= 1.0f){
			animationEngine.setMoveSpeed (0.7f);
		}
		
		//animationEngine.setMoveSpeed (velocity);
		previousLocation = transform.position;
	}
}
