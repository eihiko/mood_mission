using UnityEngine;
using System.Collections;

public class GrabMe : MonoBehaviour {

	public enum kind {WOOD, CANDLE, KEY, MATCH, TORCH, COMPASS, GOLD, KNIFE, SHIELD, MAP, HERB, HEALING_WATER,
	HEALTH_POTION}

	public GrabMe.kind myKind;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player" && Input.GetKeyDown(KeyCode.G)) {
			GameObject player = other.gameObject;
			player.GetComponent<CharacterOurs>().grab(this.gameObject);
		}
	}

	public GrabMe.kind getKind(){
		return this.myKind;
	}
}
