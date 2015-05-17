using UnityEngine;
using System.Collections;

public class MeshFilterSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {

		foreach(Transform child in this.transform)
		{
			if(child.gameObject.GetComponent<MeshFilter>() != null && child.gameObject.GetComponent<MeshFilter>().mesh == null)
			{

				Debug.LogWarning("Got inside the if statement");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
