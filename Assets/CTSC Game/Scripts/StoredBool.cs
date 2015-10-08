using UnityEngine;
using System.Collections;

public class StoredBool : MonoBehaviour {

	private bool flag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setFlag(){
		flag = true;
	}

	public void resetFlag(){
		flag = false;
	}

	public bool isSet(){
		return flag;
	}

	public StoredBool (bool flagged){
		flag = flagged;
	}
}
