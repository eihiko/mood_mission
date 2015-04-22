using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Transform menuPanel;
	//Resolution[] resolutions;
	string[] resolutions = new string[4] {"hi", "bye", "Bob", "Tom"};

	public GameObject buttonPrefab;
	// Use this for initialization
	void Start () {
		Debug.Log (resolutions.Length);
		buttonPrefab.GetComponent<LayoutElement> ().minWidth = Screen.width * 0.2f;
		for (int i = 0; i < resolutions.Length; i++) {
			GameObject button = (GameObject)Instantiate (buttonPrefab);
			button.GetComponentInChildren<Text>().text = resolutions[i];
			int index = i;
			button.GetComponent<Button>().onClick.AddListener (
				() => {/* Do stuff here */}
			);
			button.transform.parent = menuPanel;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
