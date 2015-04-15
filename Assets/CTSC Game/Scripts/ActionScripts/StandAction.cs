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
		//float y = where.transform.position.y - 3;
		//Make who stand!
		animEngine.setSitting (false);
		animEngine.setMoveSpeed (0.0f);
		who.transform.position = where.transform.position; //new Vector3(where.transform.position.x, where.transform.position.z);
		var rotationVector = who.transform.rotation.eulerAngles;
		rotationVector.x = 0;
		who.transform.rotation = Quaternion.Euler(rotationVector);
		//x = who.transform.position.x - 8;
		//who.transform.position.Set(x, who.transform.position.y, who.transform.position.z);
		//who.transform.rotation.Set (0, who.transform.rotation.y, who.transform.rotation.z, who.transform.rotation.w);
		return true;
	}
}
