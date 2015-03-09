using UnityEngine;
using System.Collections;

public class TestMentorFollow : MonoBehaviour {

	public Transform player;
	private NavMeshAgent agent;
	private AnimationEx animationEx;
	private Vector3 previousLocation;
	private float velocity;
	//An arraylist to store the agent destinations
	private Vector3[] agentDests = new Vector3[3];
	private int arrIndex = 0;
	void Start() {
		agent = GetComponent<NavMeshAgent>();
		animationEx = GetComponent<AnimationEx> ();
		//Set the first destination to be the hill above the bees
		agentDests[0] = new Vector3(-581.08f, -11.03f, 243.62f);
		//Second destination is the doctor's house
		agentDests[1] = new Vector3(-359.8f, -9.13f, 224.14f);
		//Third destination is the city
		agentDests[2] = new Vector3(-487.47f, 11.69f, 140.24f);
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




		if ((transform.position - player.position).magnitude < 10)
		{
			agent.SetDestination(agentDests[arrIndex]);
			//Check if the current agent's position is close enough to the previously set destination
			if ((transform.position - agentDests[arrIndex]).magnitude <= 0.5 && arrIndex < 2) {
				Debug.Log("Got close enough to destination");
				agent.SetDestination(agentDests[++arrIndex]);
			}
		}
		else 
		{
			agent.SetDestination(transform.position);

		}
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(velocity <= 0.5)
		{

			animationEx.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("attack");
		}
		//Walking animation if the agent's velocity is between 0.5 and 1.0
		else if(velocity > 0.5 && velocity <= 1.0) {
			animationEx.setMoveSpeed (0.9f);
		}
		//Running animation if the agent's speed is greater than 1.0
		else
		{

			animationEx.setMoveSpeed (0.9f);
		}
		//animationEx.setMoveSpeed (velocity);
		previousLocation = transform.position;
	}
}
