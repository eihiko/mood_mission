using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public KeyCode[] options;
	public ChoiceAction action;
	public int choice;

	// Use this for initialization
	void Start () {
		options = new KeyCode[3];
		options [0] = KeyCode.Alpha0;
		options [1] = KeyCode.Alpha1;
		options [2] = KeyCode.Alpha2;
		choice = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			choice = 0;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			choice = 1;
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			choice = 2;
		}
	}

	public void reset (){
		choice = 3;
	}

	public KeyCode GetChoice(){
		if (this.choice < this.options.Length) {
			return this.options [this.choice];
		} else {
			return KeyCode.SysReq; //Dummy value since null can't be used.  Equivalent to a null value.
		}
	}
}
