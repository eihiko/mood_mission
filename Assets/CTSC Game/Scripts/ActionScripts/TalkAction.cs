using UnityEngine;
using System.Collections;

public class TalkAction : MissionAction {

	GameObject talker;
	AudioSource voice;
	GameObject currUI;
	Canvas canvas;
	Transform text;
	OldDialogBox dBox;
	int startPar;
	int numPar;
	bool hasBegun = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TalkAction(GameObject talker, AudioClip audio, GameObject currUI, int startPar, int numPar){
		this.talker = talker;
		this.currUI = currUI;
		this.canvas = currUI.GetComponent<Canvas>();

		//find the ui text element
		Transform t = currUI.transform;
		foreach (Transform child in t){
			if (child.name == "UIText"){
				this.text = child;
			}
		}

		this.dBox = text.GetComponent<OldDialogBox>();

		//set the new audio clip to the talker's voice
		this.voice = talker.GetComponent<AudioSource> ();
		this.voice.clip = audio;
		this.startPar = startPar;
		this.numPar = numPar;
	}

	public bool execute(){
//		Debug.Log ("Executing talk action");
		//activate talking animation and faceplus
		if (!hasBegun) {
			hasBegun = true;
			//call gui to display text
			dBox.displayText (true, startPar, numPar);   
			//play audio clip
			//hold off on audio for now, it sucks
		//	voice.Play ();
		}
		//execute while talking and ui is active
		if (voice.isPlaying || (canvas.isActiveAndEnabled && !dBox.textCompleted)){// || (canvas.isActiveAndEnabled &&
		   // !dBox.textCompleted)) {
			return false;
		}
		return true;
	}
}
