using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform myFPSController;
	public float stoppingDistance;
	//public Animation currentAnimation;
	//Animator animator;
	private AnimationEx animationEx;
	private Vector3 previousLocation;
	private float velocity;
	private NavMeshAgent agent;



	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		previousLocation = transform.position;
		agent.stoppingDistance = stoppingDistance;
		animationEx = GetComponent<AnimationEx> ();
		//animationEx.setMoveSpeed(1.0f);
		//animator = GetComponent<Animator> ();
		//currentAnimation = this.animation;
	}
	
	// Update is called once per frame
	void Update () {

		agent.destination = myFPSController.position;

		//Manually calculate the veloctiy of the agent
		velocity = ((transform.position - previousLocation).magnitude) / Time.deltaTime;
		//Check if velocity low enough to play the idle animation (so he doesn't walk in place)
		if(velocity <= 0.5)
		{
			animationEx.setMoveSpeed(0.0f);
			//currentAnimation.animation.CrossFade("attack");
		}
		else
		{
			//animationEx.setMoveSpeed(Mathf.Abs(velocity));
			//currentAnimation.animation.CrossFade("move");
			animationEx.setMoveSpeed (1.0f);
		}
		previousLocation = transform.position;
	}
}
