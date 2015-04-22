using UnityEngine;
using System.Collections;

public class SlowGlowInteractable : Interactable {
	private float pulse = 0f, rate = 2f;
	private Color start = Color.white, end = Color.Lerp(Color.white, Color.yellow, .75f);
	private bool alive = false, on = false;
	void Update () {
		if (alive) {
			if (on) {
				if (pulse < 1f) {
					pulse += Time.deltaTime*rate;
				} else {
					pulse = 1f;
				}
			} else {
				pulse -= Time.deltaTime*rate;
				if (pulse < 0f) {
					pulse = 0f;
					alive = false;
				}
			}
			GetComponent<Renderer>().material.color = Color.Lerp(start, end, pulse);
		}
	}
	override public void Active() {
		alive = true;
		on = true;
	}
	override public void Inactive() {
		on = false;
	}
}
