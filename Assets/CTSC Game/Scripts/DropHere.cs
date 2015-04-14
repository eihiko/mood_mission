using UnityEngine;
using System.Collections;

public class DropHere : MonoBehaviour {

	public GrabMe.kind kind;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider c){
		if (c.tag == "Player" && Input.GetKeyDown(KeyCode.F)) {
			GameObject player = c.gameObject;
			player.GetComponent<CharacterOurs>().drop(this.kind, transform);
		}
	}
}
