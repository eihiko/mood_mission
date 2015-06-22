using UnityEngine;
using System.Collections;

public class StandUp : MonoBehaviour {
	private NavMeshAgent agent;
	private AnimationEngine animEngine;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animEngine = GetComponent<AnimationEngine> ();
		animEngine.setSitting (false);
		animEngine.animator.SetTrigger ("IdleTrigger");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
