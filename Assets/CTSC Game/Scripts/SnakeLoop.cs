using UnityEngine;
using System.Collections;

public class SnakeLoop : MonoBehaviour {

	public const int undecided = 0;
	public const int wait = 1;
	public const int attack = 2;
	
	public MissionManager mm;
	public StoredBool isChoosing;
	
	private int playerChoice;
	
	// Use this for initialization
	void Start () {
		playerChoice = undecided; //Initialize to "not chosen"
		isChoosing = new StoredBool(false); //Player is not choosing right now.
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1) && isChoosing.isSet()) {
			playerChoice = attack;
			isChoosing.resetFlag();
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && isChoosing.isSet()) {
			playerChoice = wait;
			isChoosing.resetFlag();
		} else {
			playerChoice = undecided;
		}
		Loop ();
	}
	
	public bool Loop(){
		if (playerChoice == wait) {
			//Player waits for snakes to leave
			this.mm.choiceSnakes.setFlag();
			return true;
			//In MissionEvent, snakes move off to the side and disappear
		} else if (playerChoice == attack) {
			//More snakes come.
			new ActiveAction(mm.currentUI,true,216,1).execute();
			isChoosing.setFlag ();
			return false;
		} 
		else
			return false; //Else should not occur unless player hasn't made decision.
	}
}
