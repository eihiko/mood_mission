using UnityEngine;
using System.Collections;

public class SpiderLoopA : MonoBehaviour {

	public const int undecided = 0;
	public const int lure = 1;
	public const int wait = 2;
	public const int attack = 3;

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
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && isChoosing.isSet()) {
			playerChoice = lure;
			isChoosing.resetFlag();
		} else {
			playerChoice = undecided;
		}
		SpiderLoop ();
	}
	
	public bool SpiderLoop(){
		if (playerChoice == lure) {
			//Player lures the spiders with food
			this.mm.choiceSewerSpiders.setFlag();
			return true;
			//In MissionEvent, spiders move out door toward "food"
		} else if (playerChoice == wait) {
			//Spiders don't move.  Nothing changes.
			new ActiveAction(mm.currentUI,true,167,1).execute();
			isChoosing.setFlag ();
			return false;
		} else if (playerChoice == attack) {
			//More spiders come.
			new ActiveAction(mm.currentUI,true,168,1).execute();
			isChoosing.setFlag ();
			return false;
		} 
		else
			return false; //Else should not occur unless player hasn't made decision.
	}
}
