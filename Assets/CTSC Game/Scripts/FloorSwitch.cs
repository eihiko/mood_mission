using UnityEngine;
using System.Collections;
using System;

public class FloorSwitch : MonoBehaviour {
	const float ASCENT = 0.01f;
	const float DESCENT = 0.01f;

	public float MAX_CHANGE = 2.0f;
	float highPoint = 0.0f;
	float lowPoint = 0.0f;

	long lastTimeUpdate = 0;
	long lastTimeTrigger = 0;

	public bool isPressed = false;
	bool negativeCoords = false;
	
	// Use this for initialization

	//This can be done differently for ascent-oriented buttons, but ours is descent-oriented like a press
	void Start () {
		highPoint = transform.position.y;
		lowPoint = transform.position.y - MAX_CHANGE;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPressed) {
				Vector3 buttonPos = transform.position;
				//have the button stop at its low point
				if (buttonPos.y < highPoint) {
					//Debug.Log("Switch is lerping upward from being released.");
					buttonPos.y += ASCENT;
					transform.position = buttonPos;
				//	Vector3.Lerp (buttonPos, new Vector3 (buttonPos.x, buttonPos.y + timeStep, buttonPos.z), timeStep);
				}
		}
	}

	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			isPressed = false;
			//Debug.Log ("Floor switch has been released.");
		}
	}

	void OnTriggerStay(Collider c){
		//Debug.Log ("The trigger stay method is working.");
		if (c.tag == "Player") {
			//Debug.Log("Floor switch is pressed!");
			isPressed = true;

			//after standing on for 4 seconds, start moving the button downward
		//	if (elapsedTime > 4f) {
				Vector3 buttonPos = transform.position;
				//have the button stop at its low point
				if (buttonPos.y > lowPoint) {
					//Debug.Log("Switch is lerping downward into the ground.");
					buttonPos.y -= DESCENT;
					transform.position = buttonPos;
					//Vector3.Lerp (buttonPos, new Vector3 (buttonPos.x, buttonPos.y - timeStep, buttonPos.z), timeStep);
				}
		//	}
		}
	//	lastTime = System.DateTime.Today.Millisecond;
	}
}
