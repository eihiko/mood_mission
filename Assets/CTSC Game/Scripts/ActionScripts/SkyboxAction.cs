using UnityEngine;
using System.Collections;

public class SkyboxAction : MissionAction {

	private GameObject mainCamera;
	private Color toShift;
	private Material toSet;
	private bool lightShift;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		if (lightShift) {
			RenderSettings.ambientLight = toShift;
		} else {
			mainCamera.GetComponent<Skybox> ().material = toSet;
		}
		return true;
	}

	public SkyboxAction(Material newbox){
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		toSet = newbox;
		lightShift = false;
	}

	public SkyboxAction(bool darkness){
		if (darkness)
			toShift = Color.gray;
		else
			toShift = Color.white;
		lightShift = true;
	}
}
