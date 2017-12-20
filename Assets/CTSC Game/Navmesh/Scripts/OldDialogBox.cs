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
	public Text titleText;
	public GameObject prevButton;
	public GameObject doneButton;
	public GameObject nextButton;
	public GameObject repeatButton;
	public GameObject expoBackground;
	public GameObject dialogBackground;
	public Scrollbar scrollBar;
	public bool textCompleted = true;


	// Use this for initialization
	void Start () {
	//	isExpo = true;
	//	Load ("Assets/CTSC Game/UI/begin_expo.txt");
		isExpo = false;
    	Load ("Assets/CTSC Game/UI/begin_dialog.txt");
	}
	
	// Update is called once per frame
	void Update () {
		if (showText) {
			Screen.lockCursor = false;
			Cursor.visible = true;
	//		if (isExpo) {
	//			expositionText.text = expoStrings [pageNum];
	//		} else {
				dialogText.text = /*"Torkana:\n "*/ dialogStrings [pageNum];
	//		}
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
//		try
//		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.UTF8);
			

			using (theReader)
			{
			while((line = theReader.ReadLine()) != null){
				// While there's lines left in the text file, do this:
//							expoStrings.Add(line);
				dialogStrings.Add(line);
			}
				
			// Done reading, close the reader and return true to broadcast success    
			theReader.Close();
			return true;
			}
	//	}
//		catch(IOException e) 
//		{
//			Debug.Log("Reading FAILED!!");
//			return false;
//		}

	}

	//A general method to display text (either expo, or dialog)
	public void displayText(bool isExpo, int textNum, int numParagraphs) 
	{
		this.numParagraphs = numParagraphs;
		textCompleted = false;
		showText = true;
		if(!isExpo) 
		{
			this.isExpo = false;
			prevButton.SetActive(true);
			nextButton.SetActive(true);
			doneButton.SetActive(true);
			expoBackground.SetActive(true);
			scrollBar.value = 1;
			this.textNum = textNum;
			//Set the intial pageNum to the textNum (first paragraph we are displaying)
			this.pageNum = textNum;
		}
		else 
		{
			this.isExpo = true;
			this.prevButton.SetActive (false);
			this.doneButton.SetActive (true);
			this.nextButton.SetActive (true);
			this.dialogBackground.SetActive(true);
			scrollBar.value = 1;
			this.textNum = textNum;
			this.pageNum = textNum;
		}

	}

	public void nextPage() 
	{
		//go forward the current page number plus the number of paragraphs
		if (this.pageNum < (this.textNum + numParagraphs) - 1) {
			this.pageNum++;
			scrollBar.value = 1;
		}
	}

	public void prevPage()
	{
		if(pageNum > textNum) 
		{
			this.pageNum--;
			scrollBar.value = 1;
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
		//scrollRect.GetComponent<Text> ().text = "";
		this.expositionText.text = "";
		this.dialogText.text = "";
		this.textCompleted = true;
		Screen.lockCursor = true;
		Cursor.visible = false;
	}


	public void repeatDialog()
	{
		//Do some stuff here
	}
	
}

