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
	public int currIndex;
	private bool lockTorkana;
	public int numDests = 30;
	private int endIndex = 0;
	private bool isActive = false;
	private bool pathComplete = true;
	private float speed = 1.2f;

	public void Start() {
		agentDests = new Vector3[numDests];
		agent = gameObject.GetComponent<NavMeshAgent>();
		animationEngine = GetComponent<AnimationEngine> ();
		currIndex = 0;
		//Iterate over the children of the pathPointers object
		foreach (Transform pointer in pathPointers) 
		{
			agentDests[currIndex] = pointer.position;
			currIndex++;
		}

		numDests = currIndex;
		currIndex = 0;
		lockTorkana = false;
		previousLocation = transform.position;
		//InvokeRepeating ("moveTorkana", 0, 0.1f);
		isActive = true;
	}

	public void beginPath(int startIndex, int endIndex){
		this.currIndex = startIndex;
		this.endIndex = endIndex;
		this.pathComplete = false;
//		if (!agent.enabled)
//			agent.enabled = true;
		Vector3.MoveTowards (transform.position, agentDests [startIndex], speed);
	//	agent.SetDestination(agentDests[startIndex]);
	}


	public bool isEnabled(){
		return isActive;
	}

	public bool isGoal(){
		if (currIndex == endIndex && Vector3.Distance (transform.position, agentDests [endIndex]) < 1.0f) {
			return true;
		}
		return false;
	}

	void Update() {
		animationEngine.setHasLimp (true);
		if (isGoal()) {
			animationEngine.setMoveSpeed(0.0f);
			pathComplete = true;
			return;
		}

		Debug.Log ("(" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")    " + 
						"(" + player.position.x + ", " + player.position.y + ", " + player.position.z + ")");
		if (((transform.position - player.position).magnitude < 8.0f && !lockTorkana)
		    || (lockTorkana && (transform.position - player.position).magnitude < 7.5f))
		{
			Debug.Log("Torkana is moving to the destination: " + currIndex); 
			lockTorkana = false;
			//agent.SetDestination(agentDests[currIndex]);
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, agentDests[currIndex], step);
			//Debug.Log("Distance between agent and goal is: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Debug.Log("Distance between agent and player is: " + (transform.position - player.position).magnitude);
			//Debug.Log("Distance between agent and its dest: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[currIndex]).magnitude <= 0.5 && currIndex < numDests - 1) {
				transform.position = Vector3.MoveTowards(transform.position, agentDests[++currIndex], step);
				//agent.SetDestination(agentDests[++currIndex]);
			}

			Vector3 targetDir = agentDests[currIndex] - transform.position;
			float step_rot = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step_rot, 0.0F);
			Debug.DrawRay(transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
		else
		{
			Debug.Log("Torkana is done moving");
			//Debug.Log("When distance is >= 8: " + (transform.position - player.position).magnitude);
			//agent.Stop ();
			//agent.SetDestination(transform.position);
			animationEngine.setMoveSpeed(0.0f);
			lockTorkana = true;
		}
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		//Debug.Log ("Velocity is: " + velocity);
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(currIndex == numDests || velocity < 0.1)
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
		if (currIndex == numDests) {
			animationEngine.setMoveSpeed(0.0f);
			return;
		}
		Debug.Log("Torkana transform: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + 
		          "Player transform: " + player.position.x + ", " + player.position.y + ", " + player.position.z);
		if (((transform.position - player.position).magnitude < 8.0f && !lockTorkana) || (lockTorkana && 
		                                                                                   (transform.position - player.position).magnitude < 7.5f))
		{
			lockTorkana = false;
			agent.SetDestination(agentDests[currIndex]);
			//Debug.Log("Distance between agent and goal is: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Debug.Log("Distance between agent and player is: " + (transform.position - player.position).magnitude);
			//Debug.Log("Distance between agent and its dest: " + (transform.position - agentDests[arrIndex]).magnitude);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[currIndex]).magnitude <= 0.5 && currIndex < numDests - 1) {
				
				agent.SetDestination(agentDests[++currIndex]);
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
		if(currIndex == numDests || velocity < 0.1)
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
