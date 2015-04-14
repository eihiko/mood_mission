using UnityEngine;
using System.Collections;


public class PickUp : MonoBehaviour {
	
	
	
	private float distance = 6.0F;
	private Inventory inv;
	public CheckItem check;
	public int textWidth = 250;
	public int textHeight = 50;
	private bool showText = false;
	public GUISkin guiSkin;
	public GameObject ghostG;
	
	void pickUpItem() 
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
		{
			if(hit.distance < distance)
			{
				inv = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
				CheckItem check = new CheckItem();
				hit.transform.SendMessage("setItemID", check, SendMessageOptions.DontRequireReceiver);
				if(check.itemID == 6)
				{
					ghostG.SetActive(true);
				}
				inv.SendMessage("AddItem", check.itemID, SendMessageOptions.RequireReceiver);
				hit.transform.SendMessage("itemCollected", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	void Update() 
	{
		if (Input.GetButtonDown("Fire3")) 
		{
			pickUpItem();
		}
		
		
	}
	
	void Start()
	{
		InvokeRepeating("checkItemProximity", 0.0f, 0.1f);
	}
	
	
	
	void checkItemProximity()
	{
		RaycastHit hit1;
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit1))
		{
			if(hit1.distance < distance && hit1.collider.tag == "Item") 
			{
				check = new CheckItem();
				hit1.transform.SendMessage("setItemName", check, SendMessageOptions.DontRequireReceiver);
				if(check.itemName != null)
				{
					showText = true;
				}
				else
				{
					showText = false;
				}
			}
			else
				showText = false;
		}
	}
	void OnGUI()
	{
		if(showText)
		{
			GUI.skin = guiSkin;
			GUI.Label(new Rect((Screen.width / 2) - (textWidth / 2), (Screen.height / 1.2f) - (textHeight / 2), textWidth, 
			                   textHeight), "Left click to pick up " + check.itemName);
		}
		
	}
}
