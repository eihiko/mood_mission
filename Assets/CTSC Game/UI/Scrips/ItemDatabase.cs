using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	
	public List<Item> items = new List<Item>();
	
	void Start() 
	{
		items.Add(new Item("Hairclip", 1, "Found on the ground", false, Item.ItemType.Quest));
		items.Add(new Item("Jacks", 2, "Part of a puzzle", false, Item.ItemType.Quest));
		items.Add(new Item("Note", 3, "A doctor's note", true, Item.ItemType.Quest));
		items.Add(new Item("Locket", 4, "An iron locket", false, Item.ItemType.Quest));
		items.Add (new Item("Teddy", 5, "A teddy bear", false, Item.ItemType.Quest));
		items.Add (new Item("YoYo", 6, "A yoyo", false, Item.ItemType.Quest));
		items.Add (new Item("Note2", 7, "Another note", true, Item.ItemType.Quest));
	}
}
