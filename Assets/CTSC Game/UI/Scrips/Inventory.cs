using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public GUISkin skin;
	public int slotX, slotY;
	public List<Item> inventory = new List<Item>();
	public List<Item> slots = new List<Item>();
	private ItemDatabase database;
	private bool showInventory = false;
	private bool showTooltip;
	private string tooltip;
	private bool dragItem;
	private Item itemDragged;
	private int prevIndex;
	public Texture2D invTexture;
	bool showNote = false;
	private Texture2D currentNote;
	public Texture2D logTabTexture;
	public Vector2 scrollPosition = Vector2.zero;
	public Texture2D backButton;
	public Texture2D backButtonGlow;
	public Texture2D mapTabBackground;
	protected int tabSelected = -1;  //-1 is for when the inventory is closed 
	//0 is for the inventory tab, 1 is for the map tab, 2 is for the log tab
	private Texture2D mapTexture;
	private Texture2D mapMarkerTex;
	private Rect mapTexPos;
	private Rect mapMarkerTexPos;
	
	
	
	// Use this for initialization
	void Start () {

		Screen.lockCursor = true;
		for(int i = 0; i < slotX * slotY; i++) 
		{
			slots.Add(new Item());
			inventory.Add(new Item());
		}
		//database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
	}
	
	void Update() 
	{
		Event e = Event.current;
		//if(Time.timeScale == 1)
		//	Screen.lockCursor = true;

		//int canPause = GameObject.Find("Totem").GetComponent<GamePause>().canPaused;
		if (Input.GetButtonDown("Inventory") /*&& (canPause == 0 || canPause == 2)*/)
		{
			Time.timeScale = 0;
			//audio.Play();
			showNote = false;
			if(Time.timeScale == 0) //Turning the notebook off
				showInventory = false;
			else {
				showInventory = true;
				Time.timeScale = 0;
			}
			if(tabSelected == -1)
				tabSelected = 0;
			else if(tabSelected == 1)
			{
				tabSelected = -1;
				//GameObject.Find("MapPlane").GetComponent<Map>().turnMapOff();
			}
			else
				tabSelected = -1;
			togglePause();
		}
		
	}
	
	void OnGUI() 
	{
		tooltip = "";
		GUI.skin = skin;
		if(showInventory && Time.timeScale == 0) 
		{
			DrawInventory();
			if(showTooltip && tooltip != "")
			{
				float dynamicSize = skin.box.CalcHeight(new GUIContent(tooltip), 200);
				GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, dynamicSize + 10), tooltip, skin.GetStyle("tooltip"));
			}
		}
		if(dragItem) 
		{
			GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 70, 70), itemDragged.itemIcon);
		}
		if(showNote && currentNote != null)
		{
			int noteWidth = currentNote.width;
			int noteHeight = currentNote.height;
			scrollPosition = GUI.BeginScrollView(new Rect((Screen.width - Screen.width / 3) / 2, 0.0f, Screen.width / 2.2f,
			                                              Screen.height * 0.9f), scrollPosition, 
			                                     new Rect((Screen.width - Screen.width / 3) / 2, -150, noteWidth, noteHeight));
			//Don't forget to change this!! The vertical position of the note texture
			//is hard coded right now, must change before final!!!!
			
			
			//GUI COLOR HERE!!
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.80f);
			GUI.DrawTexture(new Rect((Screen.width - noteWidth) / 2, (Screen.height - noteHeight) / 2,
			                         noteWidth * 1.5f, noteHeight), currentNote);
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			GUI.EndScrollView();
			GUI.skin = skin;
			GUI.Label(new Rect((Screen.width - Screen.width / 3) / 2, Screen.height * 0.95f, Screen.width / 3, Screen.height / 10),
			          "Right click to close note");
		}
		
		Event ev = Event.current;
		if(tabSelected == 1) 
		{
			Rect logTab1 = new Rect((Screen.width / 3) * 0.902f , 0.4215f * Screen.height, 0.05798f * (Screen.width / 3),
			                        0.1686f * Screen.height);
			
			if(ev != null && ev.type == EventType.mouseUp && logTab1.Contains(ev.mousePosition))
			{
				tabSelected = 2;
				GetComponent<AudioSource>().Play();
				//GameObject.Find("MapPlane").GetComponent<Map>().turnMapOff();
			}
			GUI.DrawTexture(new Rect(0, 0, Screen.width / 3, Screen.height), mapTabBackground);
			GUI.DrawTexture (mapTexPos, mapTexture);
			//mapMarkerTexPos = GameObject.Find("MapPlane").GetComponent<Map>().markerTexPos;
			GUI.DrawTexture (mapMarkerTexPos, mapMarkerTex);
			Rect buttonArea = new Rect(Screen.width / 23, Screen.height / 19, backButton.width, backButton.height);
			GUI.DrawTexture(buttonArea, backButton);
			if(buttonArea.Contains(ev.mousePosition))
			{
				GUI.DrawTexture(buttonArea, backButtonGlow);
			}
			if(buttonArea.Contains(ev.mousePosition) && ev.isMouse && ev.type == EventType.mouseUp)
			{
				playNotebookFlipSound();
				backToInvTab();
				//GameObject.Find("Inventory").GetComponent<Inventory>().playNotebookFlipSound();
				//GameObject.Find("Inventory").GetComponent<Inventory>().backToInvTab();
			}
			
		}
		if(tabSelected == 2)
		{
			GUI.DrawTexture(new Rect(0, 0, (Screen.width / 3), (Screen.height)), logTabTexture);
			Rect buttonArea = new Rect(Screen.width / 23, Screen.height / 19, backButton.width, backButton.height);
			GUI.DrawTexture(buttonArea, backButton);
			if(buttonArea.Contains(ev.mousePosition))
			{
				GUI.DrawTexture(buttonArea, backButtonGlow);
			}
			if(buttonArea.Contains(ev.mousePosition) && ev.isMouse && ev.type == EventType.mouseUp)
			{
				GetComponent<AudioSource>().Play ();
				tabSelected = 1;
			}
			/*if(GUI.Button(new Rect(0, 0, Screen.width / 10, Screen.height / 12), "Back"))
			{
				audio.Play ();
				tabSelected = 1;
				GameObject.Find("MapPlane").GetComponent<Map>().turnMapOn();
			}*/
		}
	}
	
	void DrawInventory() 
	{
		Event e = Event.current;
		int i = 0;
		GUI.DrawTexture(new Rect(0, 0, (Screen.width / 3), (Screen.height)), invTexture); 
		for(int y = 0; y < slotY; y++)
		{
			for(int x = 0; x < slotX; x++)
			{
				Rect slotRect = new Rect(x * (Screen.width / 17) + (Screen.width / 10), y * (Screen.width / 17) + (Screen.width / 8), (Screen.width / 20), (Screen.width / 20));
				GUI.Box(slotRect, "", skin.GetStyle("Slot"));
				
				slots[i] = inventory[i];
				//For each slot that has an item in it, draw it
				if(slots[i].itemName != null) 
				{
					if(slotRect.Contains(e.mousePosition) && !showNote)
					{
						GUI.DrawTexture(slotRect, slots[i].glowIcon);
					}
					else
						GUI.DrawTexture(slotRect, slots[i].itemIcon);
					//Checks if the user places the mouse above a item slot that is currently being drawn
					if(slotRect.Contains(e.mousePosition) && !showNote)
					{
						//Create and show the tooltip
						//CreateTooltip(slots[i]);
						//showTooltip = true;
						//If the user is dragging the mouse over this slot, then we draw the item icon beneath the cursor
						if(e.button == 0 && e.type == EventType.mouseDrag && !dragItem)
						{
							dragItem = true;
							prevIndex = i;
							itemDragged = slots[i];
							inventory[i] = new Item();
						}
						
						//If the user releases the previously dragged item over a slot that contains an item,
						//swap the two item slots
						if(e.type == EventType.mouseUp && dragItem) 
						{
							inventory[prevIndex] = inventory[i];
							inventory[i] = itemDragged;
							dragItem = false;
							itemDragged = null;
						}
					}
					if(tooltip == "")
					{
						showTooltip = false;
					}
					//Show the note if the user left-double-clicks a readable item
					
					if(e.isMouse && e.button == 0 && e.clickCount == 2 && slots[i].isReadable && slotRect.Contains(e.mousePosition))
					{
						showNote = true;
						currentNote = slots[i].noteTexture;
					}
					if(showNote) {
						
						//Closes the note if the player right clicks once
						if(e.isMouse && e.button == 1 && e.clickCount == 1)
							showNote = false;
					}
				}
				else 
				{
					//In the case when the user positions the mouse above an empty slot
					if(slotRect.Contains(e.mousePosition))
					{
						//If the user was previously dragging an item, then drop the item into the empty slot here
						if(e.type == EventType.mouseUp && dragItem) 
						{
							inventory[i] = itemDragged;
							dragItem = false;
							itemDragged = null;
						}
					}
				}
				i++;
			}
		}
		//Now check if the mouse is clicking one of the tabs
		/*Rect invTab = new Rect((Screen.width / 3) * 0.902f , 0.0843f * Screen.height, 0.05798f * (Screen.width / 3),
		                       0.1686f * Screen.height);
		Rect mapTab = new Rect((Screen.width / 3) * 0.902f , 0.2529f * Screen.height, 0.05798f * (Screen.width / 3),
		                       0.1686f * Screen.height);
		Rect logTab = new Rect((Screen.width / 3) * 0.902f , 0.4215f * Screen.height, 0.05798f * (Screen.width / 3),
		                       0.1686f * Screen.height);
		if(e.type == EventType.mouseUp && !dragItem && invTab.Contains(e.mousePosition)) 
		{
			
		}
		else if(e.type == EventType.mouseUp && !dragItem && mapTab.Contains(e.mousePosition) && tabSelected != -1 && !showNote)
		{
			showInventory = false;
			audio.Play();
			tabSelected = 1;
		}
		else if(e.type == EventType.mouseUp && !dragItem && logTab.Contains(e.mousePosition) && tabSelected != -1 && !showNote)
		{
			showInventory = false;
			audio.Play();
			GUI.DrawTexture(new Rect(0, 0, (Screen.width / 3), (Screen.height)), logTabTexture);
			tabSelected = 2;
		}*/
	}
	
	string CreateTooltip(Item item) 
	{
		tooltip = "<color=#0000FF>"  + item.itemName + "</color>\n\n" + "<color=#663300>" + item.itemDescription + "</color>";
		return tooltip;
	}
	
	public void RemoveItem(int id) 
	{
		for(int i = 0; i < inventory.Count; i++) 
		{
			if(inventory[i].itemID == id) 
			{
				inventory[i] = new Item();
				break;
			}
		}
	}
	void AddItem(int id) 
	{
		for(int i = 0; i < inventory.Count; i++) 
		{
			if(inventory[i].itemID == id)
			{
				break;
			}
			if(inventory[i].itemName == null) 
			{
				for(int j = 0; j < database.items.Count; j++)
				{
					if(database.items[j].itemID == id) 
					{
						inventory[i] = database.items[j];
						break;
					}
				}
				break;
			}
		}
	}
	public bool InventoryContain(int id)
	{
		bool result = false;
		for(int i = 0; i < inventory.Count; i++)
		{
			result = inventory[i].itemID == id;
			if(result)
			{
				break;
			}
		}
		return result;
	}
	bool togglePause() 
	{
		if(Time.timeScale == 0)
		{
			//GameObject.Find("Totem").GetComponent<GamePause>().canPaused = 0;
			GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
			Screen.lockCursor = true;
			Time.timeScale = 1;
			return false;
		}
		else
		{
			//GameObject.Find("Totem").GetComponent<GamePause>().canPaused = 2;
			GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
			Screen.lockCursor = false;
			Time.timeScale = 0;
			return true;
		}
	}
	
	public void backToInvTab() {
		tabSelected = 0;
		showInventory = true;
	}
	
	public void playNotebookFlipSound() {
		GetComponent<AudioSource>().Play();
	}
}
