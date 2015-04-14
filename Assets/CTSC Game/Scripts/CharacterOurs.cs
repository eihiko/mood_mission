using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterOurs : MonoBehaviour {
	
	public string name;
	public ArrayList inventory;
	
	public CharacterOurs(){
		this.name = "";
	}

	// Use this for initialization
	void Start () {
		inventory = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void grab(GameObject item){
		if (item != null) {
			Debug.Log("grabbed item");
			inventory.Add (item);
			item.SetActive (false);
		}
	}

	public void drop(GrabMe.kind kind, Transform at){
		GameObject item = null;
		foreach (GameObject o in inventory){
			if (kind == o.GetComponent<GrabMe>().getKind()){
				item = o;
				break;
			}
		}
		if (item != null) {
			Debug.Log ("placed object of kind: " + kind);
			place (item, at);
		} else {
			Debug.Log("could not place object of kind: " + kind);
		}
	}

	private void place(GameObject item, Transform tPos){
		item.transform.position = tPos.position;
		item.SetActive (true);
		inventory.Remove (item);
	}

	//Check if this character has the specified item.
	public bool has(GrabMe.kind item){
		foreach (GameObject g in inventory) {
			GrabMe.kind k = g.GetComponent<GrabMe>().getKind();
			Debug.Log("The kind of this item is : " + k);
			if (item == k){
				return true;
			}
		}
		return false;
	}
}
