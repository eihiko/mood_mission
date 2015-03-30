using UnityEngine;
using System.Collections;

public class StandAction : MissionAction {

	GameObject who, where;
	AnimationEngine animEngine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public StandAction(GameObject who, GameObject where){
		this.who = who;
		this.where = where;
		this.animEngine = who.GetComponent<AnimationEngine> ();
	}
	
	public bool execute(){
		//Make who stand!
		animEngine.setSitting (false);
		animEngine.setMoveSpeed (0.0f);
		who.transform.position = where.transform.position;
		return true;
	}
}
