using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {
	public float outerRadius = 5.0f;
	public float innerRadius = 5.0f;
	public float updateTime = 8.0f;
	public Transform player;
	public float stoppingDistance;
	public bool hasLimp;
	private NavMeshAgent agent;
	private AnimationEngine animationEngine;
	//public Animation currentAnimation;
	private Vector3 previousLocation;
	private float velocity;
	// Use this for initialization
	void Start () {
		//Initialize some values
		previousLocation = transform.position;
		agent = GetComponent<NavMeshAgent>();
		animationEngine = GetComponent<AnimationEngine> ();
		animationEngine.setHasLimp (hasLimp);
		InvokeRepeating("calculateRandomPoint", 0, updateTime);

	}
	
	// Update is called once per frame
	void Update () {
		//Manually calculate the velocity of the agent
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;


		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(velocity <= 0.1)
		{
			animationEngine.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("idle");
		}
		else
		{
			animationEngine.setMoveSpeed(0.7f);
			//currentAnimation.animation.CrossFade("walk");
		}
		previousLocation = transform.position;
	}

	void calculateRandomPoint()
	{
		//the code to calculate a random point for the agent to walk towards (within the boundaries of the navmes)
		Vector3 randomDirection = Random.insideUnitSphere * outerRadius;
		randomDirection += player.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, outerRadius, 1);
		Vector3 finalPosition = hit.position;
		agent.SetDestination(finalPosition);
	}

	void calculateRandomPointAway()
	{

	}

}
