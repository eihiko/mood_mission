using UnityEngine;
using System.Collections;

public class Leash : MonoBehaviour {

	public Transform target;
	public float range;
	
	// Update is called once per frame
	void Update () {
		if ( (transform.position - target.position).magnitude > range){
			transform.position = target.position;
		}
	
	}
	
	public void Setup(Transform target, float range){
		this.target = target;
		this.range = range;
	}
}
