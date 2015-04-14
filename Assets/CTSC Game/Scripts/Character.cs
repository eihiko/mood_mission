using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Character : MonoBehaviour {
	
	public string name;
	public List<GameObject> inventory;
	
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
			if (g.GetComponent<CollectibleItem>().getName() == item.GetComponent<CollectibleItem>().getName()){
				return true;
			}
		}
		return false;
	}

	public void addItem(GameObject obj) 
	{
		inventory.Add (obj);
	}
}
