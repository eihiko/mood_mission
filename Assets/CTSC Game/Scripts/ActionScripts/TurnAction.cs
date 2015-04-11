﻿using UnityEngine;
using System.Collections;

public class TurnAction : MissionAction {

	GameObject turns, to;

	public float damp = 5f;

	//move until within 2 degrees of target
	float angleTol = 2f;

	float time = 3f;
	
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
		Debug.Log ("Executing turn action");
		Transform turn = turns.transform;
		var rotate = Quaternion.LookRotation(to.transform.position - turn.position);
		turn.rotation = Quaternion.Slerp(turn.rotation, rotate, Time.deltaTime * damp);
		time -= Time.deltaTime;
		//this will work if the z axes are aligned similarly
	//	turns.transform.LookAt (to.transform.position);

//
//		Vector3.RotateTowards(turns.transform.position, to.transform.position, 
//	//	to.transform.position.normalized / Vector3.right
//
//
//		//wait until turn is complete
//		if (Vector3.Angle (turns.transform.forward,
//		to.transform.position - turn.position) > angleTol) {
//			return false;
//		}

//		Debug.Log ("Angle between turns and to is: " +
//		           Vector3.Angle (to.transform.eulerAngles, turn.eulerAngles));// < .1);// {
//			return true;
//		}
//		if (Quaternion.Dot(to.transform.rotation, turn.rotation) > 0.9){
//			// rotation is very close to targetRot
//			return true;
//		}

		if (time < 0)
			return true;

		return false;
	}

//	public bool turn(){
////	    float rads = Mathf.Atan2 (to.transform.position.y - turns.transform.position.y,
////		            to.transform.position.x - turns.transform.position.x); 
////		float degrees = (180 / Mathf.PI) * rads;
//
//
//	}

}
