using UnityEngine;
using System.Collections;

public class TurnAction : MissionAction {

	GameObject turns, to;

	//move until within 2 degrees of target
	float angleTol = 2f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TurnAction(GameObject turns, GameObject to){
		this.turns = turns;
		this.to = to;
	}
	
	public bool execute(){
		//this will work if the z axes are aligned similarly
		turns.transform.LookAt (to.transform.position);

		//wait until turn is complete
		while (Vector3.Angle(turns.transform.forward,
		to.transform.position - turns.transform.position) > angleTol)
		{}
		
		return true;
	}

//	public bool turn(){
////	    float rads = Mathf.Atan2 (to.transform.position.y - turns.transform.position.y,
////		            to.transform.position.x - turns.transform.position.x); 
////		float degrees = (180 / Mathf.PI) * rads;
//
//
//	}

}
