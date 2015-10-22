using UnityEngine;
using System.Collections;

public class Viewer : MonoBehaviour {

	public float speed;
	private Rigidbody r;
	
	// Use this for initialization
	void Start () {
		r = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		float s = speed;
		if(Input.GetKey (KeyCode.LeftShift)){
			s *= 2;
		}
		r.AddForce (transform.forward.normalized * s * Input.GetAxis ("Vertical"));
		r.AddForce (transform.right.normalized * s * Input.GetAxis ("Horizontal"));
		
	
	}
}
