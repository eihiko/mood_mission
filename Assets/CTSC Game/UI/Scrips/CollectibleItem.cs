using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {


	public string ObjectName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string getName() 
	{
		return this.ObjectName;
	}

	public void itemCollected() 
	{
		Destroy (gameObject);
	}
}
