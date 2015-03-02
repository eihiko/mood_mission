using UnityEngine;
using System.Collections;

public class EventTrigger1 : MonoBehaviour {

	public NPC_FSM mentor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		mentor.Walk (collider.transform);
	}
}
