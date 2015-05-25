using UnityEngine;
using System.Collections;

public class injuredPerson : MonoBehaviour {
	private NavMeshAgent agent;
	private AnimationEngine animEngine;
	public bool needsMedicine = false;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animEngine = GetComponent<AnimationEngine> ();
		animEngine.setSitting (true);
		animEngine.animator.SetTrigger ("SitTrigger");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
