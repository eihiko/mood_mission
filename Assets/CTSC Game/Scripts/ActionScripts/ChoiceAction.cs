using UnityEngine;
using System.Collections;

public class ChoiceAction : MissionEvent{

	private string mode;
	private int indicator;
	private int choices;
	private string text;
	private KeyCode[] buttons;
	GUIHandler guiHandler;
	GameObject interactionManager;
	bool first = true;
	public EnterScript[] triggers;
	public GameObject Player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute() {
		if (mode.Equals("Trigger")){
			for (int i=0;i<triggers.Length;i++){
				if (triggers[i].isEntered){
					guiHandler.reset();
					indicator = i;
					return true;
				}
				if (first){
					triggers[i].setWillEnter(Player);
					guiHandler.setTextToShow (text);
					first = false;
				}
				return false;
			}
		}
		else if (mode.Equals("Button")){
			//Figure out a way to get the button press thing to loop.  May need to make a separate script like GrabMe.
			if (first){
				guiHandler.setTextToShow (text);
				first = false;
			}
			return false;
		}
		return false;
	}

	public ChoiceAction(string mode, int indicator, int numChoices, string message) {
		this.mode = mode;
		this.indicator = indicator;
		this.choices = numChoices;
		this.text = message;
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
		if (mode.Equals ("Button")) {
			if (numChoices == 2) {
				this.buttons [0] = KeyCode.Alpha0;
				this.buttons [1] = KeyCode.Alpha1;
			}
		}
	}
}
