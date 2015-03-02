using UnityEngine;
using System.Collections.Generic;

public class SwitchUser : MonoBehaviour {
	public KeyCode use;
	public float maxDistance;
	private Switch nearest;
	private Transform cam;
	void Start () {
		cam = transform.FindChild("Camera");
	}
	void Update () {
		Debug.DrawRay(transform.position, cam.forward, Color.green);
		updateNearest();
		if (Input.GetKeyDown(use) && nearest) {
			nearest.toggle();
		}
	}
	private void updateNearest() {
		Switch last = nearest;
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, ~LayerMask.NameToLayer("Switch"));
		if (hit.transform) {
			nearest = (Switch)hit.transform.GetComponent("Switch");
			if (nearest && (last != nearest)) nearest.Active();
		} else {
			nearest = null;
		}
		if (last && (last != nearest)) {
			last.Inactive();
		}
	}
}