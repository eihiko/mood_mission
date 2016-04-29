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
        if (textToShow.Length > 0)
        {
            GUIStyle style = new GUIStyle();
            GUIStyleState state = new GUIStyleState();
            state.background = new Texture2D(2, 1);
            state.textColor = new Color(1, 1, 1);
            style.normal = state;
            style.padding = new RectOffset(4, 4, 4, 4);
            Color b = GUI.backgroundColor;
            GUI.backgroundColor = Color.black;
            GUI.Label(new Rect(100, 100, 6*textToShow.Length, 24), textToShow, style);
            GUI.backgroundColor = b;
            GUI.contentColor = Color.white;
        }
         
	}

	public void reset(){
		textToShow = "";
	}
}
