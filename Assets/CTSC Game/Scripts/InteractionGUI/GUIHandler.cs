using UnityEngine;
using System.Collections;


//handles displaying the guis for interactions
public class GUIHandler : MonoBehaviour {

	string textToShow = "";
	bool showText = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (textToShow.Equals ("")) {
			showText = false;
		} else {
			showText = true;
		}
		if (!showText) {
			GUI.enabled = false;
		}
	}

	public void setTextToShow(string text){
		//Debug.Log ("Set text to show as: " + text);
		this.textToShow = text;
	}
	
	void OnGUI()
	{
		//Debug.Log ("Gui should be showing text");
		GUI.Label (new Rect (100, 100, 300, 100), textToShow);
	}

	public void reset(){
		textToShow = "";
	}
}
