using UnityEngine;
using System.Collections;

public class SitAction : MissionAction {
	
	GameObject thingToSit;
	GameObject sitPos; 
	AnimationEngine animEngine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public SitAction(GameObject thingToSit, GameObject sitPos){
		this.thingToSit = thingToSit;
		this.sitPos = sitPos;
		this.animEngine = thingToSit.GetComponent<AnimationEngine> ();
	}
	
	public bool execute(){
		//Make the thing sit!
		animEngine.setSitting (true);
		thingToSit.transform.position = sitPos.transform.position;
		return true;
	}
}
