using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	
	public string itemName;
	public int itemID;
	public string itemDescription;
	public bool isReadable;
	public Texture2D itemIcon;
	public Texture2D glowIcon;
	public ItemType itemType;
	public Texture2D noteTexture;
	
	public enum ItemType 
	{
		Weapon,
		Consumable,
		Quest
	}
	public Item(string name, int id, string description, bool isReadable, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDescription = description;
		this.isReadable = isReadable;
		if(isReadable) {
			noteTexture = Resources.Load<Texture2D>("Readable Notes/" + name);
		}
		
		itemType = type;
		//itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
		//glowIcon = Resources.Load<Texture2D>("Item Icons/" + "glow" + name);
	}

	public Item(string name, int id) 
	{
		itemName = name;
		itemID = id;
	}
	public Item() 
	{
		
	}
	
}
