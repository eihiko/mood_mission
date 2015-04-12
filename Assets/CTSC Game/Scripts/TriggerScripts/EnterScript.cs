using UnityEngine;
using System.Collections;

public class EnterScript : MonoBehaviour {

	public bool isEntered = false;
	private GameObject willEnter;
	private string tag = "null";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setWillEnter(GameObject willEnter){
		this.willEnter = willEnter;
		this.tag = willEnter.tag;
	}

	public void OnTriggerEnter(Collider o){
		if (o.tag == tag) {
			isEntered=true;
		}
	}

//	public void OnTriggerExit(Collider o){
//		if (o.tag == "Player") {
//			isEntered=false;
//		}
//	}
}
