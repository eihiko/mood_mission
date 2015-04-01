using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour {

	private bool hasCalledDialog;
	public GameObject dialog;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) 
	{
		if (!hasCalledDialog) {
			this.dialog.GetComponent<DialogBox> ().displayText (true, 0, 3);
		}
	}
}
