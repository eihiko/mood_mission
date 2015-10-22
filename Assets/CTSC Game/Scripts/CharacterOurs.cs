using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterOurs : MonoBehaviour {
	
	public string name;
	public ArrayList inventory;
	public GrabMe.kind nextKind;
	public bool canEnter = false;
	public Transform menuPanel;
	public GameObject buttonPrefab;
	public GameObject inv;
	public int health = 100;
	public int courage = 100;
	public int compassion = 100;

	private bool showInv = false;
	public CharacterOurs(){
		this.name = "";
	}
	
	// Use this for initialization
	void Start () {
		inventory = new ArrayList();
		//inv.SetActive (false);
		buttonPrefab.GetComponent<LayoutElement> ().minWidth = Screen.width * 0.2f;
		for (int i = 0; i < inventory.Count; i++) {
			GameObject button = (GameObject)Instantiate (buttonPrefab);
			button.GetComponentInChildren<Text>().text = getItemName((GameObject)inventory[i]);
			int index = i;
			button.GetComponent<Button>().onClick.AddListener (
				() => {/* Do stuff here */}
			);
			button.transform.parent = menuPanel;
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*if(showInv)
			inv.SetActive(true);
		else if(!showInv)
			inv.SetActive(false);*/
	}
	
	public void grab(GameObject item){
		if (item != null && nextKind == item.GetComponent<GrabMe>().myKind) {
			//Debug.Log("grabbed item");
			inventory.Add (item);
			item.SetActive (false);
			updateInventory();
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
			//Debug.Log ("placed object of kind: " + kind);
			place (item, at);
		} else {
			//Debug.Log("could not place object of kind: " + kind);
		}
	}
	
	private void place(GameObject item, Transform tPos){
		item.transform.position = tPos.position;
		item.SetActive (true);
		inventory.Remove (item);
		updateInventory ();
	}
	
	//Check if this character has the specified item.
	public bool has(GrabMe.kind item){
		foreach (GameObject g in inventory) {
			GrabMe.kind k = g.GetComponent<GrabMe>().getKind();
			//Debug.Log("The kind of this item is : " + k);
			if (item == k){
				return true;
			}
		}
		return false;
	}
	
	public string getItemName (GameObject gameObj)
	{
		if(gameObj.GetComponent<GrabMe>() != null) 
		{
			return gameObj.GetComponent<GrabMe>().getKind().ToString();
		}
		else
			return "";
	}
	
	void updateInventory()
	{
		buttonPrefab.GetComponent<LayoutElement> ().minWidth = Screen.width * 0.2f;
		menuPanel.DestroyChildren();
		for (int i = 0; i < inventory.Count; i++) {
			GameObject button = (GameObject)Instantiate (buttonPrefab);
			button.GetComponentInChildren<Text>().text = getItemName((GameObject)inventory[i]);
			int index = i;
			button.GetComponent<Button>().onClick.AddListener (
				() => {/* Do stuff here */}
			);
			button.transform.parent = menuPanel;
		}
	}
	
	public void showInventory(bool showInv) 
	{
		this.showInv = showInv;
	}
}