using UnityEngine;
using System.Collections;

public class SlowPulseInteractable : Interactable {
	private float pulse, rate;
	private Color start, end;
	private bool alive;
	void Update () {
		if (alive) {
			pulse = (pulse+Time.deltaTime*rate)%(Mathf.PI*2f);
			GetComponent<Renderer>().material.color = Color.Lerp(start, end, -.5f*Mathf.Cos(pulse)+.5f);
		}
	}
	override public void Active() {
		pulse = 0f;
		rate = 1f;
		start = Color.white;
		end = Color.yellow;
		alive = true;
	}
	override public void Inactive() {
		alive = false;
		pulse = 0f;
		GetComponent<Renderer>().material.color = start;
	}
}
