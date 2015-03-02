using UnityEngine;
using System.Collections;

public class SlidingWall : Togglable {
	public Vector3 slideVector;
	private Vector3 startVector;
	private Vector3 endVector;
	public float activeTime, inactiveTime;
	private float myTime;
	private bool state;
	private bool alive;
	void Start () {
		myTime = 0;
		state = false;
		alive = false;

		startVector = transform.position;
		endVector = startVector + slideVector;
	}
	void Update () {
		if (alive) {
			if (state) {
				myTime += Time.deltaTime/activeTime;
				if (myTime > 1f) {
					myTime = 1f;
					alive = false;
				}
			} else {
				myTime -= Time.deltaTime/inactiveTime;
				if (myTime < 0) {
					myTime = 0;
					alive = false;
				}
			}
			transform.position = Vector3.Lerp(startVector, endVector, myTime);
		}
	}
	override public void toggle() {
		state = !state;
		alive = true;
	}
}
