using UnityEngine;
using System.Collections;

public class injuredPerson : MonoBehaviour {
	private NavMeshAgent agent;
	private AnimationEx animationEx;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animationEx = GetComponent<AnimationEx> ();
		animationEx.setSitting (true);
		animationEx.animator.SetTrigger ("SitTrigger");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
