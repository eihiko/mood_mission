using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NPC_Character : MonoBehaviour {

	GrabMe.kind nextKind;
	public GameObject item;
	bool giveItem;
	bool takeAnItem;
	List<GrabMe.kind> inventory;
	public Transform placeToDrop;
	GameObject player;

	public NPC_Character(){

	}
	
	// Use this for initialization
	void Start () {
		inventory = new List<GrabMe.kind> ();
		player = GameObject.Find ("ControllerBody");
	}
	
	// Update is called once per frame
	void Update () {
		if (giveItem) {
			item.SetActive (true);
		}
	}

	public void grab(GameObject item){
		if (item != null && nextKind == item.GetComponent<GrabMe>().myKind) {
			Debug.Log("NPC grabbed item");
			takeItem (nextKind);
			item.SetActive (false);
		}
	}

	bool addItemToInventory(GrabMe.kind item){
		inventory.Add (item);
		return true;
	}

	public bool takeItem(GrabMe.kind item){
		if (takeAnItem) {
			addItemToInventory (item);
			return true;
		}
		return false;
	}
	
	public void setGiveItem(bool verdict){
		giveItem = verdict;
		takeAnItem = !giveItem;
	}

	public void setTakeItem(bool verdict){
		takeAnItem = verdict;
		giveItem = !takeAnItem;
	}

}
