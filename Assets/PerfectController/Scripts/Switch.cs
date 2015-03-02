using UnityEngine;
using System.Collections;

public class Switch : SlowGlowInteractable {
	public Togglable[] affects;
	public SwitchUser effector;

	public void toggle() {
		foreach (Togglable t in affects) {
			t.toggle();
		}
		//affects.toggle();
	}
}
