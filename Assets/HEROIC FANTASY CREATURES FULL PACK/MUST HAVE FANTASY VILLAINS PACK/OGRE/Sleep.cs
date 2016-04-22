using UnityEngine;
using System.Collections;

public class Sleep : MonoBehaviour {

	private float _rotation;
	// Use this for initialization
	void Start () {
	
		_rotation = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (_rotation >= -90.0f) {
			_rotation = _rotation - 2.0f;
			transform.localEulerAngles = new Vector3 (_rotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
		}
	}
}
