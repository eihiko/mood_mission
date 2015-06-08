using UnityEngine;
using System.Collections;

public class ChoiceAction : MissionAction{

	private string mode;
	private MissionManager manager;
	private Mission currentMission;
	private string indicator;
	private int choices;
	private string text;
	private KeyCode[] buttons;
	public ButtonScript buttonScript; //Put ButtonScript on the same object as the last trigger in the event, for ease.
	GUIHandler guiHandler;
	GameObject interactionManager;
	bool first = true;
	public bool secondChoice;
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
			if (triggers[0].isEntered){
				guiHandler.reset();
				if (indicator.Equals("Rain")){
					manager.choiceInRain = 0;
					if(secondChoice){
						currentMission.setEventComplete(MissionManager.EventType.DELIVER_LETTERS);
						currentMission.setEventComplete(MissionManager.EventType.WAIT_FOR_DRIZZLE);
						currentMission.setEventComplete(MissionManager.EventType.WAIT_FOR_END);
						Debug.Log("Missions set complete lady");
					}
				}
				else{
					Debug.Log(indicator + " is a wrong key");}
				Debug.Log("Indicator set to " + 0);
				return true;
			}
			else if (triggers[1].isEntered){
				guiHandler.reset();
				if (indicator.Equals("Rain")){
					manager.choiceInRain = 1;
					currentMission.setEventComplete(MissionManager.EventType.TURN_BACK);
					currentMission.setEventComplete(MissionManager.EventType.RETURN_TO_FT1);
					Debug.Log("Mission set complete rain");
				}
				else{
					Debug.Log(indicator + " is a wrong key");}
				Debug.Log("Indicator set to " + 1);
				return true;
			}
			if (first){
				guiHandler.setTextToShow (text);
				first = false;
			}
/*			return false;
			for (int i=0;i<triggers.Length;i++){
				if (triggers[i].isEntered){
					guiHandler.reset();
					if (indicator.Equals("Rain")){
						manager.choiceInRain = i;}
					else if (indicator.Equals("TavernAfter")){
						manager.choiceInTavern = i;}
					else{
						Debug.Log(indicator + " is a wrong key");}
					Debug.Log("Indicator set to " + i);
					return true;
				}
				if (first){
					guiHandler.setTextToShow (text);
					first = false;
				}
				return false;
			}*/
		}
		else if (mode.Equals("Button")){
			if (buttonScript.GetChoice()==buttons[0]){
				guiHandler.reset();
				if (indicator.Equals("TavernAfter")){
					manager.choiceInTavern = 0;
					currentMission.setEventComplete(MissionManager.EventType.WAIT_FOR_END);
					Debug.Log("Missions set complete tavern");
				}
				else{
					Debug.Log(indicator + " is a wrong key");}
				return true;
			}
			else if (buttonScript.GetChoice()==buttons[1]){
				guiHandler.reset();
				if (indicator.Equals("TavernAfter")){
					manager.choiceInTavern = 1;
					currentMission.setEventComplete(MissionManager.EventType.WAIT_FOR_DRIZZLE);
					Debug.Log("Missions set complete stay");
				}
				else{
					Debug.Log(indicator + " is a wrong key");}
				return true;
			}
/*			for (int i=0;i<buttons.Length;i++){
				if (buttonScript.GetChoice()==buttons[i]){
					guiHandler.reset();
					if (indicator.Equals("Rain")){
						manager.choiceInRain = i;}
					else if (indicator.Equals("TavernAfter")){
						manager.choiceInTavern = i;}
					else{
						Debug.Log(indicator + " is a wrong key");}
					return true;
				}
			}*/
			if (first){
				guiHandler.setTextToShow (text);
				first = false;
			}
			return false;
		}
		return false;
	}

	public ChoiceAction(GameObject Player, string mode, EnterScript[] triggers,string indicator, int numChoices, bool second, string message) {
		manager = GameObject.Find ("MissionManager").GetComponent<MissionManager>();
		currentMission = manager.getCurrentMission ();
		Debug.Log ("Manager found");
		this.Player = Player;
		this.mode = mode;
		this.indicator = indicator;
		this.choices = numChoices;
		this.secondChoice = second;
		this.text = message;
		interactionManager = GameObject.Find ("InteractionManager");
		guiHandler = interactionManager.GetComponent<GUIHandler> ();
		if (mode.Equals ("Trigger")) {
			this.triggers = triggers;
			for (int i=0;i<triggers.Length;i++){
				triggers[i].setWillEnter(Player);
				Debug.Log(triggers[i] + " set.");
			}
		}
		if (mode.Equals ("Button")) {
			buttons = new KeyCode[numChoices];
			if (numChoices == 2) {
				this.buttons [0] = KeyCode.Alpha0;
				this.buttons [1] = KeyCode.Alpha1;
			}
			if (indicator.Equals("TavernAfter")){
				this.buttonScript = this.manager.tavernRainButton;
			}
		}
	}
}
