using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	public Texture2D cursor;
	public KeyCode key;

	private float smallRad = .005f;
	private float largeRad = .2f;
	private float cRad = .005f;
	void OnGUI() {
		GUI.DrawTexture(new Rect(getX(.5f-cRad), getY(.5f)-getX(cRad), 2f*getX(cRad), 2f*getX(cRad)), cursor);
	}
	void Update () {
		cRad = Mathf.Lerp(cRad, Input.GetKey(key)?largeRad:smallRad, .5f); 
	}

	private float getX(float percentX) {
		return percentX*Screen.width;
	}
	private float getY(float percentY) {
		return percentY*Screen.height;
	}
}
