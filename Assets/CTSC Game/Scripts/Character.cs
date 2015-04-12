using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character : MonoBehaviour {
	
	public string name;
	public GameObject[] inventory;
	
	public Character(){
		this.name = "";
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Check if this character has the specified item.
	public bool has(GameObject item){
		foreach (GameObject g in inventory) {
			if (g.Equals(item)){
				return true;
			}
		}
		return false;
	}
}
