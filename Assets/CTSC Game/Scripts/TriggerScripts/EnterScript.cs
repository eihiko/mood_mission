using UnityEngine;
using System.Collections;

public class EnterScript : MonoBehaviour {

	private const string DISABLE_OBJ = "disable";

	public bool isEntered = false;
	private GameObject willEnter;
	private string thisTag = "TagNotSet";
	public string type = "";
	public MissionManager.MissionType thisMissionType;
	public GameObject manipulateMe;
	private MissionManager mm;
	
	// Use this for initialization
	void Start () {
		mm = GameObject.Find ("MissionManager").GetComponent<MissionManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setWillEnter(GameObject willEnter){
		this.willEnter = willEnter;
		thisTag = willEnter.tag;
	}

	public void reset(){
		isEntered = false;
		willEnter = null;
		thisTag = "TagNotSet";
	}

	public void OnTriggerStay(Collider o){
//		Debug.Log ("Entered trigger for the enter script");
		if (o.tag == thisTag) {
			isEntered = true;
		} else {
			isEntered = false;
			return;
		}

		//determine what to do with this enter
		switch (type) {
			case DISABLE_OBJ:
				disableObj();
				Debug.Log ("Disabled object for enter script");
				break;
			default:
				break;
		}
	}

	void disableObj(){
		if (thisMissionType == mm.currMissionType) {
			manipulateMe.SetActive (false);
		}
		return;
	}

//	public void OnTriggerExit(Collider o){
//		if (o.tag == "Player") {
//			isEntered=false;
//		}
//	}
}
