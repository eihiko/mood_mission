using UnityEngine;
using System.Collections;

public class CyclopsChoiceLoop : MonoBehaviour {

	public const int undecided = 0;
	public const int correct = 1;
	public const int neutral = 2;
	public const int incorrect = 3;

	public Cyclops cyclops;
	public MissionManager mm;
	public AudioClip CyclopsThink;
	public AudioClip CyclopsSelfish;
	public AudioClip CyclopsCynical;
	public AudioClip CyclopsRage;
	public StoredBool isChoosing;

	private int playerChoice;

	// Use this for initialization
	void Start () {
		playerChoice = undecided; //Initialize to "not chosen"
		isChoosing = new StoredBool(false); //Cyclops does not know what to choose at the start
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1) && isChoosing.isSet()) {
			playerChoice = correct;
			isChoosing.resetFlag();
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && isChoosing.isSet()) {
			playerChoice = neutral;
			isChoosing.resetFlag();
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && isChoosing.isSet()) {
			playerChoice = incorrect;
			isChoosing.resetFlag();
		} else {
			playerChoice = undecided;
		}
		CyclopsChoice ();
	}

	public bool CyclopsChoice(){
		if (playerChoice == correct) {
			int decision = cyclops.CorrectSuggestion ();
			if (decision == Cyclops.APPLES) {
				this.mm.cyclopsChoice.setFlag ();
				return true;
				//In MissionEvent, cyclops agrees with player and thanks them.
			} else if (decision == Cyclops.SIT) {
				//Cyclops says "Hmmm... Let me think about that for a while."
				new TalkAction(mm.Cyclops,CyclopsThink,mm.currentUI,154,1).execute();
				isChoosing.setFlag ();
				return false;
			} else if (decision == Cyclops.ATTACK) {
				//Cyclops says "Why should I help people?  They never helped me!"
				new TalkAction(mm.Cyclops,CyclopsSelfish,mm.currentUI,155,1).execute();
				isChoosing.setFlag ();
				return false;
			} else
				return false; //Else should not occur
		} else if (playerChoice == neutral) {
			//Cyclops says "That won't change anything.  Nobody likes me."
			new TalkAction(mm.Cyclops,CyclopsCynical,mm.currentUI,156,1).execute();
			isChoosing.setFlag ();
			return false;
		} else if (playerChoice == incorrect) {
			cyclops.IncorrectSuggestion ();
			//Cyclops says "Fine, then!  I'll just keep attacking!"
			new TalkAction(mm.Cyclops,CyclopsRage,mm.currentUI,157,1).execute();
			isChoosing.setFlag ();
			return false;
		} 
		else
			return false; //Else should not occur unless player hasn't made decision.
	}
}
