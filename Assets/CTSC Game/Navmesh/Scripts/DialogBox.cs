using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using System.IO; 

public class DialogBox : MonoBehaviour {


	private List<string> expoStrings = new List<string>();
	private List<string> dialogStrings = new List<string>();
	//Boolean that checks if the current displayed text is exposition
	private bool isExpo;
	//A boolean that checks if text is currently being displayed to the player
	private bool showText;
	//textNum stores the initial index of the paragraph we are displaying
	private int textNum;
	//Another field to keep track of how far a player has progressed through an episode of text
	//ie: the player can click to show the next section of dialog/exposition after he/she is done
	//reading the current one

	//pageNum is the current page we are displaying
	private int pageNum;
	private int numParagraphs;
	public Text expositionText;
	public Text dialogText;
	public GameObject prevButton;
	public GameObject doneButton;
	public GameObject nextButton;
	public GameObject repeatButton;
	public GameObject expoBackground;
	public GameObject dialogBackground;
	public bool textCompleted = true;


	// Use this for initialization
	void Start () {
		isExpo = true;
		Load ("Assets/CTSC Game/UI/begin_expo.txt");
		isExpo = false;
		Load ("Assets/CTSC Game/UI/begin_dialog.txt");
	}
	
	// Update is called once per frame
	void Update () {
		if(showText) 
		{
			Screen.lockCursor = false;
			Screen.showCursor = true;
			if(isExpo) 
			{
				expositionText.text = expoStrings[pageNum];
			}
			else
			{
				dialogText.text = "Torkana:\n " + dialogStrings[pageNum];
			}
		}
		/*else 
		{
			Screen.lockCursor = true;
			Screen.showCursor = false;
		}*/
	}

	private bool Load(string fileName)
	{
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			

			using (theReader)
			{
				// While there's lines left in the text file, do this:
				int i = 0;
				do
				{
					line = theReader.ReadLine();
					
					if (line != null)
					{
						if(isExpo) 
						{
							expoStrings.Add(line);
						}
						else
							dialogStrings.Add(line);
					}
				}
				while (line != null);
				
				// Done reading, close the reader and return true to broadcast success    
				theReader.Close();
				return true;
			}
		}
		catch(IOException e) 
		{
			Debug.Log("Reading FAILED!!");
			return false;
		}

	}

	//A general method to display text (either expo, or dialog)
	public void displayText(bool isExpo, int textNum, int numParagraphs) 
	{
		this.numParagraphs = numParagraphs;
		textCompleted = false;
		showText = true;
		if(isExpo) 
		{
			this.isExpo = true;
			prevButton.SetActive(true);
			nextButton.SetActive(true);
			doneButton.SetActive(true);
			expoBackground.SetActive(true);
			this.textNum = textNum;
			//Set the intial pageNum to the textNum (first paragraph we are displaying)
			this.pageNum = textNum;
		}
		else 
		{
			this.isExpo = false;
			this.prevButton.SetActive (false);
			this.doneButton.SetActive (true);
			this.nextButton.SetActive (true);
			this.dialogBackground.SetActive(true);
			this.textNum = textNum;
			this.pageNum = textNum;
		}

	}

	public void nextPage() 
	{
		if(this.pageNum < numParagraphs - 1)
			this.pageNum++;
	}

	public void prevPage()
	{
		if(pageNum > textNum) 
		{
			this.pageNum--;
		}
	}

	public void textDone()
	{
		showText = false;
		this.prevButton.SetActive (false);
		this.doneButton.SetActive (false);
		this.nextButton.SetActive (false);
		this.expoBackground.SetActive (false);
		this.dialogBackground.SetActive (false);
		this.expositionText.text = "";
		this.dialogText.text = "";
		this.textCompleted = true;
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}


	public void repeatDialog()
	{
		//Do some stuff here
	}
	
}

