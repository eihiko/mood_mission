using UnityEngine;
using System.Collections;


public class FreezeAction : MissionAction {
	
	private bool FROZEN = false;
	private Vector3 FREEZEPOSITION = Vector3.zero;
	private GameObject player;
	private bool isControllable = true;
	private bool freeze;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isControllable)
		{
			Input.ResetInputAxes();
		}
		if (FROZEN)
		{
			player.transform.position = FREEZEPOSITION;
		}
	}
	
	public bool execute(){
		Debug.Log ("Executing freeze action");
		if (freeze) {
			FreezePlayer ();
		} else {
			Thaw ();
		}
		return true;
	}

	public FreezeAction(GameObject player, bool freeze){
		this.player = player;
		this.freeze = freeze;
	}

	void FreezePlayer()
	{
		isControllable = false; // disable player controls.
		FREEZEPOSITION = player.transform.position;
		FROZEN = true;
	}

	void Thaw()
	{
		FROZEN = false;
		isControllable = true; 
	}
}
