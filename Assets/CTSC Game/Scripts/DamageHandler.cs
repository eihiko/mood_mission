using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {

	private bool damageOn;
	private KeyCode[] watchCodes;
	public PlayerStatusBars status;

	// Use this for initialization
	void Start () {
		damageOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (damageOn) {
			if (Input.GetKeyDown(watchCodes[0])){
				status.DecreaseStat(20,"Health");
			}
			else if (Input.GetKeyDown(watchCodes[1])){
				status.DecreaseStat(10,"Health");
			}
		}
	}

	public void TurnDamageOn(string type){
		damageOn = true;
		if (type.Equals ("Swarm")) {
			watchCodes = new KeyCode[2];
			watchCodes [0] = KeyCode.R; //Swat
			watchCodes [1] = KeyCode.C; //Stomp
		} else if (type.Equals ("Hydra")) {
			watchCodes = new KeyCode[3];
			watchCodes [0] = KeyCode.Alpha1; //Compliment Scroll 1
			watchCodes [1] = KeyCode.Alpha2; //Compliment Scroll 2
			watchCodes [2] = KeyCode.V; //Shield or Kalatar Scroll, depending on what Hydra code looks like.  May need another key
		} else if (type.Equals ("Goblin")) {
			watchCodes = new KeyCode[1]; 
			watchCodes [0] = KeyCode.Alpha1; //Use Scroll (if Kalatar scrolls are left, uses one of them, else uses Compliment Scroll)
		} else if (type.Equals ("Dragon")) {
			watchCodes = new KeyCode[6];
			watchCodes [0] = KeyCode.Alpha1; //Compliment Scroll 1
			watchCodes [1] = KeyCode.Alpha2; //Compliment Scroll 2
			watchCodes [2] = KeyCode.Alpha3; //Compliment Scroll 3
			watchCodes [3] = KeyCode.Alpha4; //Compliment Scroll 4
			watchCodes [4] = KeyCode.Alpha5; //Compliment Scroll 5
			watchCodes [5] = KeyCode.V; //Shield/Kalatar Scroll
		} else if (type.Equals ("Kid")) {
			watchCodes = new KeyCode[5];
			watchCodes [0] = KeyCode.Alpha1; //Compliment Scroll 1
			watchCodes [1] = KeyCode.Alpha2; //Compliment Scroll 2
			watchCodes [2] = KeyCode.Alpha3; //Compliment Scroll 3
			watchCodes [3] = KeyCode.Alpha4; //Compliment Scroll 4
			watchCodes [4] = KeyCode.Alpha5; //Compliment Scroll 5
		}
	}

	public void TurnDamageOff(){
		damageOn = false;
	}

}
