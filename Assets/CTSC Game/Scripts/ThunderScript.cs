using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ThunderScript : MonoBehaviour {

	public AudioClip A,B,C;
	public AudioSource source;
	public AudioSource rainMachine;
	private bool notEarly = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying && notEarly) {
			StartCoroutine("CloserThunder");
			//StartCoroutine ("CloserThunder");
		} else if (!source.isPlaying) {
			StartCoroutine ("FarOffThunder");
		}

		if(rainMachine.isPlaying == true){
			notEarly = true;
		}
	}

	IEnumerator CloserThunder(){
		source.Stop ();
		RandomThunder ();
		yield return new WaitForSeconds (2f);
		source.Play ();
	}

	IEnumerator FarOffThunder(){
		source.Stop ();
		yield return new WaitForSeconds (4f);
		source.Play ();
	}

	private void RandomThunder(){
		int thunder = UnityEngine.Random.Range (1, 3);
		if (thunder == 1) {
			this.source.clip = this.A;
		} else if (thunder == 2) {
			this.source.clip = this.B;
		} else if (thunder == 3) {
			this.source.clip = this.C;
		} else {
			this.source.clip = this.A;
		}
	}

}
