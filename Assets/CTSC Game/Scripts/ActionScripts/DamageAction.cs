using UnityEngine;
using System.Collections;

public class DamageAction : MissionAction {

	private DamageHandler handler;
	private bool damageOn;
	private string damageType;

	public DamageAction (DamageHandler h, bool on, string type) {
		this.handler = h;
		this.damageOn = on;
		this.damageType = type;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute() {
		if (damageOn) {
			handler.TurnDamageOn (damageType);
		} else {
			handler.TurnDamageOff ();
		}
		return true;
	}
}
