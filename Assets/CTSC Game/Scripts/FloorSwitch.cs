using UnityEngine;
using System.Collections;

public class FloorSwitch : MonoBehaviour {

	const float highPoint = -16.43f;
	const float lowPoint = -16.63f;

	float elapsedTime = 0.0f;
	float lastTime = 0.0f;
	float timeStep = 0.1f;

	bool isPressed = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPressed) {
			Debug.Log("Floor switch is not pressed.");
			//do some time calculations for the button move
			timeStep = System.DateTime.Today.Millisecond - lastTime;
			timeStep /= 1000f;
			elapsedTime += timeStep;
		
			//lerp button upward after not pressing for 2 seconds
			if (elapsedTime > 2f) {
				Vector3 buttonPos = transform.position;
				//have the button stop at its low point
				if (buttonPos.y > highPoint) {
					Debug.Log("Switch is lerping upward from being pressed.");
					buttonPos = buttonPos - (Vector3.down / 1000) * timeStep;
				//	Vector3.Lerp (buttonPos, new Vector3 (buttonPos.x, buttonPos.y + timeStep, buttonPos.z), timeStep);
				}
			}
		}

		lastTime = System.DateTime.Today.Millisecond;
		Debug.Log ("The last time is: " + lastTime);
	}

	void OnTriggerLeave(){
		isPressed = false;
		Debug.Log("Floor switch has been released.");
	}

	void OnTriggerStay(Collider c){
		if (c.tag == "Player") {
			Debug.Log("Floor switch is pressed!");
			isPressed = true;
			//do some time calculations for the button move
			timeStep = System.DateTime.Today.Millisecond - lastTime;
			timeStep /= 100000f;
			elapsedTime += timeStep;

			//after standing on for 4 seconds, start moving the button downward
			if (elapsedTime > 4f) {
				Vector3 buttonPos = transform.position;
				//have the button stop at its low point
				if (buttonPos.y < lowPoint) {
					Debug.Log("Switch is lerping downward into the groud.");
					Vector3.Lerp (buttonPos, new Vector3 (buttonPos.x, buttonPos.y - timeStep, buttonPos.z), timeStep);
				}
			}
		}
		lastTime = System.DateTime.Today.Millisecond;
	}
}
