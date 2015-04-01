using UnityEngine;
using System.Collections;

public class DialogTrigger : MonoBehaviour {


	private bool showDialog;
	private bool dialogStarted;
	// Use this for initialization
	void Start () {
		showDialog = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(dialogStarted) 
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;
		}
		if (Input.GetKeyDown("x") && !dialogStarted && showDialog)
		{
			dialogStarted = true;

		}
	}


	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			showDialog = true;
		}
	}
	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			showDialog = false;
			dialogStarted = false;
		}
	}

	void OnGUI() 
	{
		if (showDialog) 
		{
			GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.75f, 200.0f, 100.0f),"Press 'x' to talk");
		}

		if (dialogStarted) 
		{
			if (GUI.Button(new Rect(10, 70, 50, 30), "Drink poison")) 
			{

			}
		}
	}
}
