using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour {

	private bool hasCalledDialog;
	public GameObject dialog;
	public GameObject character;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) 
	{
		character.GetComponent<CharacterOurs> ().showInventory (true);
		if (!hasCalledDialog) {
			this.dialog.GetComponent<DialogBox> ().displayText (true, 0, 3);
		}
		this.dialog.GetComponent<PlayerStatusBars> ().DecreaseStat(30, "Compassion");
	}
}
