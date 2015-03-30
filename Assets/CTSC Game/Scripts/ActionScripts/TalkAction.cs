using UnityEngine;
using System.Collections;

public class TalkAction : MissionAction {

	GameObject talker;
	AudioSource voice;
	GameObject currUI;
	Canvas canvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TalkAction(GameObject talker, AudioClip audio, GameObject currUI){
		this.talker = talker;
		this.currUI = currUI;
		this.canvas = currUI.GetComponent<Canvas>();

		//set the new audio clip to the talker's voice
		this.voice = talker.GetComponent<AudioSource> ();
		this.voice.clip = audio;
	}

	public bool execute(){
		//activate talking animation and faceplus

		//play audio clip
		voice.Play ();

		//execute while talking and ui is active
		while (voice.isPlaying && canvas.isActiveAndEnabled) {}
		return true;
	}
}
