using UnityEngine;
using System.Collections;

public class MapBorder : MonoBehaviour {

	public float width;
	public float height;
	private RectTransform trans;

	// Use this for initialization
	void Start () {
		trans = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		trans.sizeDelta = new Vector2(Screen.width * width+2, Screen.height * height+2);
	}
}
