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
//		Debug.Log ("Executing sit action");
		//Make the thing sit!
		animEngine.setSitting (true);
		//animEngine.animator.SetTrigger("SitTrigger");
		thingToSit.transform.position = sitPos.transform.position;
		return true;
	}
}
